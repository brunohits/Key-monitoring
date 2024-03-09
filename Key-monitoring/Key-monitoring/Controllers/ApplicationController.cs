using System;
using Key_monitoring.DTOs;
using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Key_monitoring.Servises;
using Key_monitoring.Enum;

namespace Key_monitoring.Controllers
{

    [ApiController]
    [Route("api/application")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        [Route("CreateApplication")]
        public async Task<IActionResult> ApplicationCreate([FromBody] ApplicationCreateDTO data)
        {
            try
            {
                return Ok(await _applicationService.CreateApplication(data));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangeApplicationStatus")]
        public async Task<IActionResult> ApplicationChangeStatus([FromBody] ApplicationStatusDTO data)
        {
            try
            {
                return Ok(await _applicationService.ChangeApplicationStatus(data));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("applicationsList")]
        public async Task<IActionResult> ApplicationsList([FromHeader] ApplicationStatusEnum? status, [FromHeader] RoleEnum? role, [FromHeader] int? cabinetNumber, [FromHeader] string? partOfName, [FromHeader] int page, [FromHeader] int size)
        {
            try
            {
                return Ok(await _applicationService.GetApplicationsList(status, role, cabinetNumber, partOfName, page, size));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("applicationsList/User")]
        public async Task<IActionResult> ApplicationsListUser([FromHeader] Guid userId, [FromHeader] ApplicationStatusEnum? status)
        {
            try
            {
                return Ok(await _applicationService.GetApplicationsListUser(userId, status));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("application/delete")]
        public async Task<IActionResult> ApplicationDelete([FromBody] ApplicationDeleteDTO data)
        {
            try
            {
                return Ok(await _applicationService.ApplicationDelete(data));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


