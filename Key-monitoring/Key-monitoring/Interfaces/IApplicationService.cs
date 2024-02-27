using Key_monitoring.DTOs;
using Key_monitoring.Enum;
using Key_monitoring.Models;
using Microsoft.AspNetCore.Mvc;

namespace Key_monitoring.Interfaces;

public interface IApplicationService
{
    Task<ApplicationsListDto> GetApplicationsList(ApplicationStatusEnum? status, RoleEnum? role, int? cabinetNumber, string? partOfName, PaginationReqDTO pagination);
}