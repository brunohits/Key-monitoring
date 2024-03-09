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

    public class ScheduleService : IScheduleService
    {

        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ScheduleService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IdListDTO> CreateWeek(DateTime start)
        {
            try
            {
                for(int i = 0; i < 6; ++i)
                {
                    var par = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.PairStart.Date == start.AddDays(i).Date);
                    if(par != null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Wrong week data");
                        throw exception;
                    }
                }

                var modifiedStart = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);

                var newPairsList = new List<ScheduleModel>();
                for(int i = 0;i < 6; ++i) 
                {
                    newPairsList.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        PairStart = modifiedStart.AddDays(i).AddHours(8).AddMinutes(45)
                    });
                    newPairsList.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        PairStart = modifiedStart.AddDays(i).AddHours(10).AddMinutes(35)
                    });
                    newPairsList.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        PairStart = modifiedStart.AddDays(i).AddHours(12).AddMinutes(25)
                    });
                    newPairsList.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        PairStart = modifiedStart.AddDays(i).AddHours(14).AddMinutes(45)
                    });
                    newPairsList.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        PairStart = modifiedStart.AddDays(i).AddHours(16).AddMinutes(35)
                    });
                    newPairsList.Add(new ScheduleModel
                    {
                        Id = Guid.NewGuid(),
                        PairStart = modifiedStart.AddDays(i).AddHours(18).AddMinutes(25)
                    });
                }
                foreach (var pair in newPairsList)
                {
                    await _dbContext.Schedule.AddAsync(pair);
                    await _dbContext.SaveChangesAsync();
                }
                var idList = new List<Guid>();
                foreach (var pair in newPairsList)
                {
                    idList.Add(pair.Id);
                }
                return new IdListDTO { idList = idList };
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<WeekDTO> GetWeek(DateTime start)
        {
            try
            {
                var pairs = await _dbContext.Schedule.Where(x => x.PairStart >=  start).Take(36).ToListAsync();
                if (pairs.Count == 0 || pairs == null || pairs.Count < 36)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Wrong week data");
                    throw exception;
                }
                return new WeekDTO{ pairs = pairs };
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}

