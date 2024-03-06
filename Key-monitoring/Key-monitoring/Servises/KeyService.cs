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


        public async Task<Exception?> CreateKey(KeyCreateDTO newKey)
        {
            try
            {
                await _dbContext.Keys.AddAsync(new KeyModel
                {
                    Id = Guid.NewGuid(),
                    CreateTime = DateTime.UtcNow,
                    FacultyId = newKey.FacultyId,
                    CabinetNumber = newKey.CabinetNumber,
                    CabinetName = newKey.CabinetName,
                    Owner = null
                });

                await _dbContext.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public async Task<KeyListDTO> GetList(int page, int size)
        {
            var allKeys = await _dbContext.Keys.ToListAsync();
            if ((page - 1) * size + 1 > allKeys.Count)
            {
                throw new ArgumentException("To big pag");
            }
            allKeys = allKeys.Skip((page - 1) * size).Take(size).ToList();

            var KeyList = new KeyListDTO();
            foreach (var key in allKeys)
            {
                string stat;
                if (key.Owner == null)
                {
                    stat = "Available";
                }
                else
                {
                    stat = "Used";
                }
                Guid? ownID;
                string? ownName;
                RoleEnum? ownRole;
                if (key.Owner == null)
                {
                    ownID = null;
                    ownName = null;
                    ownRole = null;
                }
                else
                {
                    ownID = key.Owner.Id;
                    ownName = key.Owner.FullName;
                    ownRole = key.Owner.Role;
                }
                KeyList.List.Add(new KeyListElementDTO
                {
                    Id = key.Id,
                    CabinetNumber = key.CabinetNumber,
                    KeyStatus = stat,
                    OwnerId = ownID,
                    OwnerName = ownName,
                    OwnerRole = ownRole
                });
            }

            return KeyList;
        }


        public async Task<KeySchInfoDTO> GetKeyInfo(Guid id, DateTime start)
        {
            var allpairs = await _dbContext.Schedule.Where(x => x.PairStart >= start).Take(36).ToListAsync();
            if (allpairs.Count == 0 || allpairs == null || allpairs.Count < 36)
            {
                throw new ArgumentException("Wrong week data");
            }
            var Mon = new List<KeyFullModelDTO>();
            for (int i = 0; i < 6; ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allpairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allpairs[i].PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                    keyData.role = reserv.User.Role.ToString();
                }
                Mon.Add(keyData);
            }
            var Tue = new List<KeyFullModelDTO>();
            for (int i = 6; i < 12; ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allpairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allpairs[i].PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                    keyData.role = reserv.User.Role.ToString();
                }
                Tue.Add(keyData);
            }
            var Wed = new List<KeyFullModelDTO>();
            for (int i = 12; i < 18; ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allpairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allpairs[i].PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                    keyData.role = reserv.User.Role.ToString();
                }
                Wed.Add(keyData);
            }
            var Thu = new List<KeyFullModelDTO>();
            for (int i = 18; i < 24; ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allpairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allpairs[i].PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                    keyData.role = reserv.User.Role.ToString();
                }
                Thu.Add(keyData);
            }
            var Fri = new List<KeyFullModelDTO>();
            for (int i = 24; i < 30; ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allpairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allpairs[i].PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                    keyData.role = reserv.User.Role.ToString();
                }
                Fri.Add(keyData);
            }
            var Sat = new List<KeyFullModelDTO>();
            for (int i = 30; i < 36; ++i)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == allpairs[i].Id && x.Status == ApplicationStatusEnum.Approved);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = allpairs[i].PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                    keyData.role = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                    keyData.role = reserv.User.Role.ToString();
                }
                Sat.Add(keyData);
            }
            var retData = new KeySchInfoDTO
            {
                mn = Mon,
                tu = Tue,
                we = Wed,
                th = Thu,
                fr = Fri,
                st = Sat
            };
            return retData;
        }

        public async Task<Boolean> ChangeKeyStatus(Guid keyId, Guid? userId)
        {
            var key = await _dbContext.Keys.FirstOrDefaultAsync(x => x.Id == keyId);
            if (key == null)
            {
                throw new ArgumentException("wrong key Id");
            }
            if (userId == null)
            {
                key.Owner = null;
                _dbContext.Keys.Update(key);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new ArgumentException("wrong user Id");
                }
                key.Owner = user;
                _dbContext.Keys.Update(key);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<KeyDayInfoDTO> GetKeyDayInfo(GetKeyDayInfoDTO data)
        {
            var pairs = await _dbContext.Schedule.Where(x => x.PairStart.ToLongDateString() == data.day.ToLongDateString()).Take(6).ToListAsync();
            if(pairs == null || pairs.Count != 6)
            {
                throw new ArgumentException("Ahtung! Wrong date");
            }
            var statusList = new List<string>();
            
            foreach (var pair in pairs) 
            {
                var keyInfo = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Schedule.PairStart == pair.PairStart && x.Status == ApplicationStatusEnum.Approved);
                if(keyInfo == null)
                {
                    statusList.Add("Open");
                }
                else
                {
                    statusList.Add("Close");
                }
            }

            return new KeyDayInfoDTO { statuses = statusList };
        }
    }
}

