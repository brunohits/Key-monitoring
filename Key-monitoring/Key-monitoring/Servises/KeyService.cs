using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Key_monitoring.Interfaces;
using Key_monitoring.DTOs;
using System.Text.RegularExpressions;
using Key_monitoring.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Key_monitoring.Enum;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;



namespace Key_monitoring.Servises
{

    public class KeyService: IKeyService
    {

        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public KeyService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<Guid> CreateKey(KeyCreateDTO newKey)
        {
            try
            {
                var newId = Guid.NewGuid();
                await _dbContext.Keys.AddAsync(new KeyModel
                {
                    Id = newId,
                    CreateTime = DateTime.UtcNow,
                    FacultyId = newKey.FacultyId,
                    CabinetNumber = newKey.CabinetNumber,
                    Status = KeyStatusEnum.Available,
                    Owner = null
                });

                await _dbContext.SaveChangesAsync();

                return newId;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<KeyListDTO> GetList()
        {
            try
            {
                var allKeys = await _dbContext.Keys.ToListAsync();

                var KeyList = new KeyListDTO();
                KeyList.List = new List<KeyListElementDTO>();
                foreach (var key in allKeys)
                {
                    Guid? ownID;
                    string? ownName;
                    RoleEnum? ownRole;
                    if (key.Status == KeyStatusEnum.OnHands)
                    {
                        var own = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == key.OwnerId);

                        if (own == null)
                        {
                            var exception = new Exception();
                            exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Человек был не найден");
                            throw exception;
                        }

                        ownID = key.OwnerId;
                        ownName = own.FullName;
                        ownRole = own.Role;
                    }
                    else
                    {
                        ownID = null;
                        ownName = null;
                        ownRole = null;
                    }
                    var newKeyEl = new KeyListElementDTO
                    {
                        Id = key.Id,
                        CabinetNumber = key.CabinetNumber,
                        KeyStatus = key.Status,
                        OwnerId = ownID,
                        OwnerName = ownName,
                        OwnerRole = ownRole
                    };
                    KeyList.List.Add(newKeyEl);
                }

                return KeyList;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<List<KeyFullModelDTO>> GetDayInfo(Guid id, List<ScheduleModel> allPairs, int coef)
        {
            var day = new List<KeyFullModelDTO>();
            for (int i = 6 * coef; i < 6 * (coef + 1); ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allPairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allPairs[i].PairStart,
                    status = KeyStatusEnum.Available
                };
                if (reserv == null)
                {
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    var us = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == reserv.UserId);
                    if (us == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Человек был не найден");
                        throw exception;
                    }

                    keyData.status = KeyStatusEnum.Booked;
                    keyData.userId = reserv.UserId;
                    keyData.userName = us.FullName;
                    keyData.role = us.Role.ToString();
                }
                day.Add(keyData);
            }
            return day;
        }

        public async Task<KeySchInfoDTO> GetKeyInfo(Guid id, DateTime start)
        {
            try
            {
                var allpairs = await _dbContext.Schedule.Where(x => x.PairStart >= start).Take(36).ToListAsync();
                if (allpairs.Count == 0 || allpairs == null || allpairs.Count < 36)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Wrong week data");
                    throw exception;
                }
                var retData = new KeySchInfoDTO
                {
                    mn = await GetDayInfo(id, allpairs, 0),
                    tu = await GetDayInfo(id, allpairs, 1),
                    we = await GetDayInfo(id, allpairs, 2),
                    th = await GetDayInfo(id, allpairs, 3),
                    fr = await GetDayInfo(id, allpairs, 4),
                    st = await GetDayInfo(id, allpairs, 5)
                };
                return retData;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<KeyStatusEnum> ChangeKeyStatus(Guid id, string token, Guid keyId, Guid? userId)
        {
            try
            {
                var searchUser = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var checkToken = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.InvalidToken == token);
                if (checkToken != null)
                {
                    var ex = new Exception();
                    ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "Данный тонен устарел");
                    throw ex;
                }
                if (searchUser == null)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Пользователь был не найден");
                    throw exception;
                }

                var key = await _dbContext.Keys.FirstOrDefaultAsync(x => x.Id == keyId);
                if (key == null)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ключ был не найден");
                    throw exception;
                }
                if (userId == null)
                {
                    key.Status = KeyStatusEnum.Available;
                    key.OwnerId = id;
                    key.Owner = searchUser;
                    _dbContext.Keys.Update(key);
                    await _dbContext.SaveChangesAsync();
                    return KeyStatusEnum.Available;
                }
                else
                {
                    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    if (user == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Человек был не найден");
                        throw exception;
                    }
                    key.Status = KeyStatusEnum.OnHands;
                    key.OwnerId = user.Id;
                    key.Owner = user;
                    _dbContext.Keys.Update(key);
                    await _dbContext.SaveChangesAsync();
                    return KeyStatusEnum.OnHands;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<KeyDayInfoDTO> GetKeyDayInfo(Guid KeyId, DateTime day)
        {
            try
            {
                var pairs = await _dbContext.Schedule.Where(x => x.PairStart.Date == day.Date).ToListAsync();
                if (pairs == null || pairs.Count != 6)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ahtung! Wrong date");
                    throw exception;
                }
                var statusList = new List<KeyStatusEnum>();

                foreach (var pair in pairs)
                {
                    var keyInfo = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Schedule.PairStart == pair.PairStart && x.KeyId == KeyId && x.Status == ApplicationStatusEnum.Approved);
                    if (keyInfo == null)
                    {
                        statusList.Add(KeyStatusEnum.Available);
                    }
                    else
                    {
                        statusList.Add(KeyStatusEnum.OnHands);
                    }
                }

                return new KeyDayInfoDTO { statuses = statusList };
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}

