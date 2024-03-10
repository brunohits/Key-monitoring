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

namespace Key_monitoring.Controllers
{

    [ApiController]
    [Route("api/key")]
    public class KeyController : ControllerBase
    {
        private readonly IKeyService _keyService;

        public KeyController(IKeyService keyService)
        {
            _keyService = keyService;
        }


        [HttpPost]
        [Route("CreateKey")]
        [Authorize]
        public async Task<IActionResult> CreateKey([FromBody] KeyCreateDTO newKey)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                if (token == null)
                {
                    return BadRequest("Access token not found in the current context.");
                }

                return Ok(await _keyService.CreateKey(Guid.Parse(User.Identity.Name), token, newKey));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetFullKeyList")]
        public async Task<IActionResult> KeyList()
        {
            try
            {
                return Ok(await _keyService.GetList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetKeyInfoOnWeek")]
        public async Task<IActionResult> KeyInfo([FromHeader] Guid id, [FromHeader] DateTime Start)
        {
            try
            {
                return Ok(await _keyService.GetKeyInfo(id, Start));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangeKeyStatus")]
        [Authorize]
        public async Task<IActionResult> KeyStatusChange([FromBody] KeyStatusDto keyStatus)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                if (token == null)
                {
                    return BadRequest("Access token not found in the current context.");
                }

                return Ok(await _keyService.ChangeKeyStatus(Guid.Parse(User.Identity.Name), token, keyStatus.KeyId, keyStatus.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetKeyInfoOnDayInfo")]
        public async Task<IActionResult> DayInfo([FromHeader] Guid KeyId, [FromHeader] DateTime day)
        {
            try
            {
                return Ok(await _keyService.GetKeyDayInfo(KeyId, day));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


