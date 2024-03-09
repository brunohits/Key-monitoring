using System;
using System.ComponentModel.DataAnnotations;
using Key_monitoring.DTOs;
using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Key_monitoring.Servises;

namespace Key_monitoring.Controllers
{

    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        [Route("CreateWeek")]
        public async Task<IActionResult> CreateWeek([FromBody] GetWeekDTO start)
        {
            try
            {
                return Ok(await _scheduleService.CreateWeek(start.WeekStart));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetWeek")]
        public async Task<IActionResult> GetWeek([FromBody] GetWeekDTO start)
        {
            try
            {
                return Ok(await _scheduleService.GetWeek(start.WeekStart));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


