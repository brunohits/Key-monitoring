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

    public class KeyService
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
            if((page - 1) * size + 1 > allKeys.Count) 
            {
                throw new ArgumentException("To big pag");
            }
            allKeys = allKeys.Skip((page - 1) * size).Take(size).ToList();

            var KeyList = new KeyListDTO();
            foreach(var key in allKeys)
            {
                string stat;
                if(key.Owner == null)
                {
                    stat = "Open";
                }
                else
                {
                    stat = "Close";
                }
                Guid? ownID;
                string ownName;
                if(key.Owner == null)
                {
                    ownID = null;
                    ownName = null;
                }
                else
                {
                    ownID = key.Owner.Id;
                    ownName = key.Owner.FullName;
                }
                KeyList.List.Add(new KeyListElementDTO
                {
                    Id = key.Id,
                    CabinetNumber = key.CabinetNumber,
                    CabinetName = key.CabinetName,
                    KeyStatus = stat,
                    OwnerId = ownID,
                    OwnerName = ownName
                });
            }
            KeyList.size = size;
            KeyList.count = KeyList.List.Count;
            KeyList.current = page;

            return KeyList;
        }

        public async Task<List<KeyFullModelDTO>> GetKeyInfo(Guid id, DateTime start, DateTime finish)
        {
            var allpairs = await _dbContext.Raspisanies.Where(x => x.PairStart >= start && x.PairStart <= finish).ToListAsync();
            if(allpairs.Count == 0 || allpairs == null) 
            {
                throw new ArgumentException("To big pag");
            }
            var fullKey = new List<KeyFullModelDTO>();
            foreach (var pair in allpairs) 
            {
                var reserv = await _dbContext.Reservations.FirstOrDefaultAsync(x => x.key.Id == id && x.pair ==  pair);
                var keyData = new KeyFullModelDTO
                {
                    PairStart = pair.PairStart,
                    status = ""
                };
                if(reserv == null)
                {
                    keyData.status = "open";
                    keyData.userId = null;
                    keyData.userName = null;
                }
                else
                {
                    keyData.status = "reserved";
                    keyData.userId = reserv.user.Id;
                    keyData.userName = reserv.user.FullName;
                }
                fullKey.Add(keyData);
            }
            return fullKey;
        }
    }
}

