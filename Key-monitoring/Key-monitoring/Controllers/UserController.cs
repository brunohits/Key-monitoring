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
    public async Task<IActionResult> specialityGet([FromQuery] NameAndPaginGetDTO specialityGetDTO)
    {
        try
        {
            var result = await _user.SearchUser(specialityGetDTO);
            return Ok(result);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return BadRequest("An error occurred while processing your request.");
        }
    }
}