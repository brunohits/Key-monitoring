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
        public async Task<IActionResult> ApplicationsList([FromHeader] ApplicationStatusEnum? status, [FromHeader] RoleEnum? role, [FromHeader] int? cabinetNumber, [FromHeader] string? partOfName, [FromHeader] PaginationReqDTO pagination)
        {
            try
            {
                var result = await _applicationService.GetApplicationsList(status, role, cabinetNumber, partOfName, pagination);
                if (result != null)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("applicationsList/User")]
        public async Task<IActionResult> ApplicationsListUser([FromBody] ApplicationsListUserDTO data)
        {
            try
            {
                var result = await _applicationService.GetApplicationsListUser(data);
                if (result != null)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result);
                }
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
                var result = await _applicationService.ApplicationDelete(data);
                if (result != null)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


