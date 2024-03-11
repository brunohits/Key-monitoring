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
using Org.BouncyCastle.Pqc.Crypto.Crystals.Kyber;



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
            try
            {
                var pair = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.Id == data.pairId);
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == data.userId);
                var key = await _dbContext.KeyModels.FirstOrDefaultAsync(x => x.Id == data.keyId);
                if (pair == null || user == null || key == null)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ahtung! Wrong ID");
                    throw exception;
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
                    Clone = false,
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

                if (data.repetitive == true)
                {
                    var newappList = new List<ApplicationModel>();
                    for(int i = 0; i < 4; ++i)
                    {
                        var nextPair = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.PairStart == pair.PairStart.AddDays(7 * (i + 1)));
                        if(nextPair != null)
                        {
                            newappList.Add(new ApplicationModel
                            {
                                Id = Guid.NewGuid(),
                                CreateTime = newAppl.CreateTime,
                                UserId = user.Id,
                                User = user,
                                ScheduleId = nextPair.Id,
                                Schedule = nextPair,
                                KeyId = key.Id,
                                Key = key,
                                Repetitive = data.repetitive,
                                Clone = true,
                                Status = ApplicationStatusEnum.UnderConsideration
                            });
                        }
                    }
                    foreach(var app in newappList)
                    {
                        await _dbContext.Applications.AddAsync(app);
                        await _dbContext.SaveChangesAsync();
                    }

                    if (user.Role != RoleEnum.Student || user.Role != RoleEnum.NotСonfirmed)
                    {
                        foreach(var app in newappList) 
                        {
                            var applList = await _dbContext.Applications.Where(x => x.ScheduleId == app.ScheduleId && x.KeyId == app.KeyId && x.User.Role == RoleEnum.Student).ToListAsync();
                            foreach (var applic in applList)
                            {
                                applic.Status = ApplicationStatusEnum.Denied;
                                _dbContext.Applications.Update(applic);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }

                return newAppl.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
        public async Task<Guid> ChangeApplicationStatus(ApplicationStatusDTO data)
        {
            try
            {
                var appl = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Id == data.id);
                if (appl == null)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ahtung! Wrong ID");
                    throw exception;
                }

                if (data.status == ApplicationStatusEnum.Approved)
                {
                    var applList = await _dbContext.Applications.Where(x => x.ScheduleId == appl.ScheduleId && x.KeyId == appl.KeyId && x.Id != appl.Id).ToListAsync();
                    foreach (var applic in applList)
                    {
                        applic.Status = ApplicationStatusEnum.Denied;
                        _dbContext.Applications.Update(applic);
                        await _dbContext.SaveChangesAsync();
                        if(applic.Repetitive == true && applic.Clone == false)
                        {
                            var cloneList = await _dbContext.Applications.Where(x => x.CreateTime == applic.CreateTime && x.UserId == applic.UserId && x.KeyId == applic.KeyId && x.Clone == true).ToListAsync();
                            foreach (var ap in cloneList)
                            {
                                ap.Status = ApplicationStatusEnum.Denied;
                                _dbContext.Applications.Update(ap);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
                appl.Status = data.status;
                _dbContext.Applications.Update(appl);
                await _dbContext.SaveChangesAsync();

                if (appl.Repetitive == true && appl.Clone == false)
                {
                    var cloneApplList = await _dbContext.Applications.Where(x => x.CreateTime == appl.CreateTime && x.UserId == appl.UserId && x.KeyId == appl.KeyId && x.Clone == true).ToListAsync();
                    foreach (var applic in cloneApplList)
                    {
                        applic.Status = data.status;
                        _dbContext.Applications.Update(applic);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return appl.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public async Task<ApplicationsListDto> GetApplicationsList(ApplicationStatusEnum? status, RoleEnum? role, int? cabinetNumber, string? partOfName, ApplicationSortEnum sort, int page, int size)
        {
            try
            {
                var allAppl = await _dbContext.Applications.Where(x => x.Clone == false).ToListAsync();

                var appList = new ApplicationsListDto
                {
                    List = new List<ApplicationsListElementDTO>(),
                    Pagination = new PaginationDTO
                    {
                        Size = size,
                        Count = 0,
                        Current = page
                    }
                };

                if (allAppl == null)
                {
                    return appList;
                }

                if (status != null)
                {
                    var sluch = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Status == status);
                    if (sluch == null)
                    {
                        return appList;
                    }

                    allAppl = allAppl.Where(x => x.Status == status).ToList();
                    if (allAppl == null)
                    {
                        return appList;
                    }
                }

                if (role != null)
                {
                    var timedList = new List<ApplicationModel>();
                    foreach (var item in allAppl)
                    {
                        var creator = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == item.UserId);
                        if(creator != null && creator.Role == role)
                        {
                            timedList.Add(item);
                        }
                    }
                    if (timedList.Count == 0 || timedList == null)
                    {
                        return appList;
                    }
                    allAppl = timedList;
                }

                if (cabinetNumber != null)
                {
                    var key = await _dbContext.KeyModels.FirstOrDefaultAsync(x => x.CabinetNumber == cabinetNumber);
                    if(key == null)
                    {
                        return appList;
                    }
                    allAppl = allAppl.Where(x => x.KeyId == key.Id).ToList();
                    if (allAppl == null)
                    {
                        return appList;
                    }
                }

                if (partOfName != null)
                {
                    var timedList = new List<ApplicationModel>();
                    foreach (var item in allAppl)
                    {
                        var creator = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == item.UserId);
                        if (creator != null && creator.FullName != null && creator.FullName.Contains(partOfName))
                        {
                            timedList.Add(item);
                        }
                    }
                    if (timedList.Count == 0 || timedList == null)
                    {
                        return appList;
                    }
                    allAppl = timedList;
                }

                switch(sort)
                {
                    case ApplicationSortEnum.CreateAsc:
                        allAppl = allAppl.OrderBy(x => x.CreateTime).ToList();
                        break;
                    case ApplicationSortEnum.CreateDesc:
                        allAppl = allAppl.OrderByDescending(x => x.CreateTime).ToList();
                        break;
                }

                if ((page - 1) * size + 1 > allAppl.Count)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Wrong pag");
                    throw exception;
                }
                allAppl = allAppl.Skip((page - 1) * size).Take(size).ToList();

                var retList = new List<ApplicationsListElementDTO>();
                foreach (var app in allAppl)
                {
                    var sus = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == app.UserId);
                    if (sus == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Человек был не найден");
                        throw exception;
                    }

                    var usu = await _dbContext.KeyModels.FirstOrDefaultAsync(x => x.Id == app.KeyId);
                    if (usu == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ключ был не найден");
                        throw exception;
                    }

                    var usus = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.Id == app.ScheduleId);
                    if (usus == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Пара был не найден");
                        throw exception;
                    }

                    retList.Add(new ApplicationsListElementDTO
                    {
                        Id = app.Id,
                        date = app.CreateTime,
                        OwnerId = app.UserId,
                        OwnerName = sus.FullName,
                        OwnerRole = sus.Role,
                        KeyId = app.KeyId,
                        KeyNumber = usu.CabinetNumber,
                        Repetitive = app.Repetitive,
                        PairStart = usus.PairStart,
                        Status = app.Status
                    });
                }

                var pag = new PaginationDTO
                {
                    Size = size,
                    Count = retList.Count,
                    Current = page
                };

                return new ApplicationsListDto { List = retList, Pagination = pag };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationListForUserDTO> GetApplicationsListUser(Guid userId, ApplicationStatusEnum? status)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ahtung! Wrong ID");
                    throw exception;
                }
                var applications = await _dbContext.Applications.Where(x => x.User == user && x.Clone == false).ToListAsync();

                if (status != null)
                {
                    applications = applications.Where(x => x.Status == status).ToList();
                }

                var retList = new List<ApplicationsListElementDTO>();
                foreach (var app in applications)
                {
                    var sus = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == app.UserId);
                    if (sus == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Человек был не найден");
                        throw exception;
                    }

                    var usu = await _dbContext.KeyModels.FirstOrDefaultAsync(x => x.Id == app.KeyId);
                    if (usu == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ключ был не найден");
                        throw exception;
                    }

                    var usus = await _dbContext.Schedule.FirstOrDefaultAsync(x => x.Id == app.ScheduleId);
                    if (usus == null)
                    {
                        var exception = new Exception();
                        exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Пара был не найден");
                        throw exception;
                    }

                    retList.Add(new ApplicationsListElementDTO
                    {
                        Id = app.Id,
                        date = app.CreateTime,
                        OwnerId = app.UserId,
                        OwnerName = sus.FullName,
                        OwnerRole = sus.Role,
                        KeyId = app.KeyId,
                        KeyNumber = usu.CabinetNumber,
                        Repetitive = app.Repetitive,
                        PairStart = usus.PairStart,
                        Status = app.Status
                    });
                }
                return new ApplicationListForUserDTO { List = retList };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ApplicationDelete(ApplicationDeleteDTO data)
        {
            try
            {
                var appl = await _dbContext.Applications.FirstOrDefaultAsync(x => x.Id == data.applicationId);
                if (appl == null)
                {
                    var exception = new Exception();
                    exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ahtung! Wrong ID");
                    throw exception;
                }

                if (appl.Repetitive == true && appl.Clone == false)
                {
                    var cloneApplList = await _dbContext.Applications.Where(x => x.CreateTime == appl.CreateTime && x.UserId == appl.UserId && x.KeyId == appl.KeyId && x.Clone == true).ToListAsync();
                    foreach (var applic in cloneApplList)
                    {
                        _dbContext.Applications.Remove(applic);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                _dbContext.Applications.Remove(appl);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

