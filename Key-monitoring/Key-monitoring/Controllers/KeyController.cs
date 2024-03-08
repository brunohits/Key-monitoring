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
        //создать интерфейс
        private readonly IKeyService _keyService;

        public KeyController(IKeyService keyService)
        {
            _keyService = keyService;
        }


        [HttpPost]
        [Route("CreateKey")]
        public async Task<IActionResult> CreateKey([FromBody] KeyCreateDTO newKey)
        {
            try
            {
                return Ok(await _keyService.CreateKey(newKey));
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
        public async Task<IActionResult> KeyInfo([FromBody] GetKeyInfoDTO infoData)
        {
            try
            {
                return Ok(await _keyService.GetKeyInfo(infoData.id, infoData.Start));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangeKeyStatus")]
        public async Task<IActionResult> KeyStatusChange([FromBody] KeyStatusDto keyStatus)
        {
            try
            {
                return Ok(await _keyService.ChangeKeyStatus(keyStatus.KeyId, keyStatus.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetKeyInfoOnDayInfo")]
        public async Task<IActionResult> DayInfo([FromBody] GetKeyDayInfoDTO data)
        {
            try
            {
                return Ok(await _keyService.GetKeyDayInfo(data));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}


