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
                    CabinetName = key.CabinetName,
                    KeyStatus = stat,
                    OwnerId = ownID,
                    OwnerName = ownName,
                    OwnerRole = ownRole
                });
            }
            var pag = new PaginationDTO
            {
                Size = size,
                Count = KeyList.List.Count,
                Current = page
            };
            KeyList.Pagination = pag;

            return KeyList;
        }


        public async Task<List<KeyFullModelDTO>> GetKeyInfo(Guid id, DateTime start, DateTime finish)
        {
            var allpairs = await _dbContext.Schedule.Where(x => x.PairStart >= start && x.PairStart <= finish).ToListAsync();
            if (allpairs.Count == 0 || allpairs == null)
            {
                throw new ArgumentException("To big pag");
            }
            var fullKey = new List<KeyFullModelDTO>();
            foreach (var pair in allpairs)
            {
                var reserv = await _dbContext.Applications.FirstOrDefaultAsync(x => x.KeyId == id && x.ScheduleId == pair.Id);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = pair.PairStart,
                    status = ""
                };
                if (reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.UserId;
                    keyData.userName = reserv.User.FullName;
                }
                fullKey.Add(keyData);
            }
            return fullKey;
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
    }
}

