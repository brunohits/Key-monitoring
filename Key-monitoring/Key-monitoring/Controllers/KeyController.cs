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
        public async Task<IActionResult> KeyInfo([FromHeader] Guid id)
        {
            try
            {
                return Ok(await _keyService.GetKeyInfo(id));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


