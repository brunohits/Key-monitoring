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
    }
}

