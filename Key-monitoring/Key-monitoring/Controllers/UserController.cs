using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Key_monitoring.DTOs;

namespace Key_monitoring.Controllers;

[ApiController]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUser _user;

    public UserController(IUser user)
    {
        _user = user;
    }


    [HttpGet("Search/Users")]
    public async Task<IActionResult> SearchUser(NameWithPaginationDTO Name)
    {
        try
        {
            var result = _user.SearchUser(Name);
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
        }
        return StatusCode(500, "Внутренняя ошибка сервера.");
    }
}