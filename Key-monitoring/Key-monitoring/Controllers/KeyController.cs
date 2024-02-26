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
        private readonly KeyService _keyService;

        public KeyController(KeyService keyService)
        {
            _keyService = keyService;
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateKey([FromBody] KeyCreateDTO newKey)
        {
            try
            {
                var result = await _keyService.CreateKey(newKey);
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
        [Route("KeyList")]
        public async Task<IActionResult> KeyList([FromHeader] int page, [FromHeader] int size)
        {
            try
            {
                return Ok(await _keyService.GetList(page, size));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Key")]
        public async Task<IActionResult> KeyInfo([FromHeader] Guid id, [FromBody] DateTime start, [FromBody] DateTime finish)
        {
            try
            {
                return Ok(await _keyService.GetKeyInfo(id, start, finish));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("KeyStatus")]
        public async Task<IActionResult> KeyStatusChange([FromBody] Guid keyId, [FromBody] Guid? userId)
        {
            try
            {
                var result = await _keyService.ChangeKeyStatus(keyId, userId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


