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

namespace Key_monitoring.Controllers
{

    [ApiController]
    [Route("api/account")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService usersService)
        {
            _authService = usersService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegister)
        {
            try
            {
                var token = await _authService.Register(userRegister);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine("Ошибка обновления базы данных: " + ex.Message);
                return StatusCode(400, "Пользователь с такими данными уже существует.");
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains(StatusCodes.Status409Conflict.ToString()))
                {
                    return Conflict(ex.Data[StatusCodes.Status409Conflict.ToString()]);
                }


                if (ex.Data.Contains(StatusCodes.Status400BadRequest.ToString()))
                {
                    return BadRequest(ex.Data[StatusCodes.Status400BadRequest.ToString()]);
                }
                Debug.WriteLine("Произошла ошибка: " + ex.Message);
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO forSuccessfulLogin)
        {
            try
            {
                if (forSuccessfulLogin.Email == null)
                {
                    return StatusCode(401, "Email cannot be null.");
                }

                var token = await _authService.Login(forSuccessfulLogin);

                if (token != null)
                {
                    return Ok(token);
                }
                else
                {
                    return StatusCode(401, "Invalid credentials.");
                }
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(401, "User does not exist.");
            }
            catch (Exception ex)
            {

              //  Console.WriteLine($"An error occurred while logging in: {ex}");
                return StatusCode(400, "Неверные данные");
            }
        }





        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (token == null)
            {
                return BadRequest("Access token not found in the current context.");
            }
            else
            {
                try
                {
                    await _authService.Logout(token);
                    return Ok("Logged out successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(401, ex.Message);
                }
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetInfoAboutUser()
        {
            string token = await HttpContext.GetTokenAsync("access_token");
           
            try{
                var result = await _authService.GetInfoUser(Guid.Parse(User.Identity.Name), token);
                return Ok(result);
            }
            catch(Exception ex)
            {
                if (ex.Data.Contains(StatusCodes.Status400BadRequest.ToString()))
                {
                    return BadRequest(ex.Data[StatusCodes.Status400BadRequest.ToString()]);
                }
                else if (ex.Data.Contains(StatusCodes.Status404NotFound.ToString()))
                {
                    return NotFound(ex.Data[StatusCodes.Status404NotFound.ToString()]);
                }
                else if (ex.Data.Contains(StatusCodes.Status401Unauthorized.ToString()))
                {
                    return Unauthorized(StatusCodes.Status401Unauthorized.ToString());
                }
                return StatusCode(500, "Внутренняя ошибка сервера.");
            }
        }

        [Authorize]
        [HttpGet("Change/Profile")]
        public async Task<IActionResult> ChangeUserInfo(ChangeUserInfoDTO changeUserInfoDto)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            try
            {

                if (!User.Identity.IsAuthenticated || !Guid.TryParse(User.Identity.Name, out Guid userId))
                {
                    return Unauthorized("User is not authenticated");
                }

                await _authService.ChangeInfoAboutUser(userId, changeUserInfoDto, token);

                return Ok("Profile updated successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ex.Message == "Token is already invalid")
                {
                    return Unauthorized("Token is invalid");
                }
                throw; 
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains(StatusCodes.Status409Conflict.ToString()))
                {
                    return Conflict(ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }

    }
}


