using System;
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

                if (!IsValidEmail(userRegister.Email))
                {
                    return BadRequest("Неверный формат email.");
                }


                if (!IsValidPhoneNumber(userRegister.PhoneNumber))
                {
                    return BadRequest("Неверный формат телефонного номера. Используйте формат +7xxxxxxxxxx.");
                }


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



        private bool IsValidEmail(string email)
        {

            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Проверка формата телефонного номера
        private bool IsValidPhoneNumber(string phoneNumber)
        {

            string phonePattern = @"^\+7\d{10}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
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

                Console.WriteLine($"An error occurred while logging in: {ex}");


                return StatusCode(400, "An error occurred while logging in. Please check the console for more details.");
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

        
    }
}


