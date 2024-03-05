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
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



namespace Key_monitoring.Servises
{

    public class ApplicationService : IApplicationService
    {

        private ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ApplicationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> CreateApplication(ApplicationCreateDTO data)
        {
            var pair = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.Id == data.pairId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == data.userId);
            var key = await _dbContext.Keys.FirstOrDefaultAsync(x => x.Id == data.keyId);
            if(pair == null || user == null || key == null)
            {
                throw new ArgumentException("Ahtung! Wrong ID");
            }
            var newAppl = new ApplicationModel
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                UserId = user.Id,
                User = user,
                ScheduleId = pair.Id,
                Schedule = pair,
                KeyId = key.Id,
                Key = key,
                Repetitive = data.repetitive,
                Status = ApplicationStatusEnum.UnderConsideration
            };
            await _dbContext.Applications.AddAsync(newAppl);
            await _dbContext.SaveChangesAsync();

            if (user.Role != RoleEnum.Student || user.Role != RoleEnum.NotСonfirmed)
            {
                var applList = await _dbContext.Applications.Where(x => x.ScheduleId == newAppl.ScheduleId && x.KeyId == newAppl.KeyId && x.User.Role == RoleEnum.Student).ToListAsync();
                foreach (var applic in applList)
                {
                    applic.Status = ApplicationStatusEnum.Denied;
                    _dbContext.Applications.Update(applic);
                    await _dbContext.SaveChangesAsync();
                }
            }

            return newAppl.Id;
        }

        public async Task<Guid> ChangeApplicationStatus(ApplicationStatusDTO data)
        {
            var appl = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Id == data.id);
            if (appl == null || appl.UserId != data.userID)
            {
                throw new ArgumentException("Ahtung! Wrong ID");
            }
            if(data.status == ApplicationStatusEnum.Approved)
            {
                var applList = await _dbContext.Applications.Where(x => x.ScheduleId == appl.ScheduleId && x.KeyId == appl.KeyId).ToListAsync();
                foreach (var applic in applList)
                {
                    applic.Status = ApplicationStatusEnum.Denied;
                    _dbContext.Applications.Update(applic);
                    await _dbContext.SaveChangesAsync();
                }
            }
            appl.Status = data.status;
            _dbContext.Applications.Update(appl);
            await _dbContext.SaveChangesAsync();
            return appl.Id;
        }

        public async Task<ApplicationsListDto> GetApplicationsList(ApplicationStatusEnum? status, RoleEnum? role, int? cabinetNumber, string? partOfName, PaginationReqDTO pagination)
        {
            var allAppl = await _dbContext.Applications.ToListAsync();

            var appList = new ApplicationsListDto
            {
                List = new List<ApplicationsListElementDTO>(),
                Pagination = new PaginationDTO
                {
                    Size = pagination.Size,
                    Count = 0,
                    Current = pagination.Page
                }
            };

            if (allAppl == null ) 
            {
                return appList;
            }

            if(status != null) 
            {
                allAppl = allAppl.Where(x => x.Status == status).ToList();
                if (allAppl == null)
                {
                    return appList;
                }
            }

            if (role != null)
            {
                allAppl = allAppl.Where(x => x.User.Role == role).ToList();
                if (allAppl == null)
                {
                    return appList;
                }
            }

            if (cabinetNumber != null)
            {
                allAppl = allAppl.Where(x => x.Key.CabinetNumber == cabinetNumber).ToList();
                if (allAppl == null)
                {
                    return appList;
                }
            }

            if (partOfName != null)
            {
                allAppl = allAppl.Where(x => x.User.FullName.Contains(partOfName)).ToList();
                if (allAppl == null)
                {
                    return appList;
                }
            }

            if ((pagination.Page - 1) * pagination.Size + 1 > allAppl.Count)
            {
                throw new ArgumentException("To big pag");
            }
            allAppl = allAppl.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToList();

            var retList = new List<ApplicationsListElementDTO>();
            foreach (var app in allAppl) 
            {
                retList.Add(new ApplicationsListElementDTO
                {
                    Id = app.Id,
                    date = app.CreateTime,
                    OwnerId = app.UserId,
                    OwnerName = app.User.FullName,
                    OwnerRole = app.User.Role,
                    KeyId = app.KeyId,
                    KeyNumber = app.Key.CabinetNumber,
                    Repetitive = app.Repetitive,
                    PairStart = app.Schedule.PairStart,
                    Status = app.Status
                });
            }

            var pag = new PaginationDTO
            {
                Size = pagination.Size,
                Count = retList.Count,
                Current = pagination.Page
            };

            return new ApplicationsListDto { List = retList, Pagination = pag };
        }

        public async Task<ApplicationListForUserDTO> GetApplicationsListUser(ApplicationsListUserDTO data)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == data.userId);
            if (user == null)
            {
                throw new ArgumentException("Ahtung! Wrong ID");
            }
            var applications = await _dbContext.Applications.Where(x => x.User == user).ToListAsync();

            if(data.status != null)
            {
                applications = applications.Where(x => x.Status == data.status).ToList();
            }

            var retList = new List<ApplicationsListElementDTO>();
            foreach (var app in applications)
            {
                retList.Add(new ApplicationsListElementDTO
                {
                    Id = app.Id,
                    date = app.CreateTime,
                    OwnerId = app.UserId,
                    OwnerName = app.User.FullName,
                    OwnerRole = app.User.Role,
                    KeyId = app.KeyId,
                    KeyNumber = app.Key.CabinetNumber,
                    Repetitive = app.Repetitive,
                    PairStart = app.Schedule.PairStart,
                    Status = app.Status
                });
            }
             return new ApplicationListForUserDTO { List = retList };
        }

        public async Task<bool> ApplicationDelete(ApplicationDeleteDTO data)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == data.userId);
            var appl = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Id == data.applicationId);
            if (user == null || appl == null || appl.UserId != data.userId)
            {
                throw new ArgumentException("Ahtung! Wrong ID");
            }
            _dbContext.Applications.Remove(appl);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

