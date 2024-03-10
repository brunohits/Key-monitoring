using Key_monitoring.DTOs;
using Key_monitoring.Enum;
using Key_monitoring.Models;
using Microsoft.AspNetCore.Mvc;

namespace Key_monitoring.Interfaces;

public interface IApplicationService
{
    Task<Guid> ChangeApplicationStatus(ApplicationStatusDTO data);
    Task<Guid> CreateApplication(ApplicationCreateDTO data);
    Task<ApplicationsListDto> GetApplicationsList(ApplicationStatusEnum? status, RoleEnum? role, int? cabinetNumber, string? partOfName, ApplicationSortEnum sort, int page, int size);
    Task<ApplicationListForUserDTO> GetApplicationsListUser(Guid userId, ApplicationStatusEnum? status);
    Task<bool> ApplicationDelete(ApplicationDeleteDTO data);
}