using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Key_monitoring.Controllers;
[ApiController]
[Route("api/account")]
public class EmailController : ControllerBase 
{
    private readonly IEmail _email;

    public EmailController(IEmail email)
    {
        _email = email;
    }
    [Authorize]
    [HttpPost("send/email")]

    public async Task<IActionResult> SpecialityGet([FromQuery]Guid? id, [FromQuery]int? numberRoom)
    {
        try
        {  
            if (id.HasValue && numberRoom.HasValue)
            {
                bool isEmailSent = await _email.SendEmail(id.Value, Guid.Parse(User.Identity.Name), numberRoom.Value);
                if (isEmailSent)
                    return Ok();
                else
                    return BadRequest();
            }
            else
            {
                return BadRequest("Both id and numberRoom are required."); 
            }
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return NotFound("404 Error: Resource not found");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return BadRequest("An error occurred while processing your request.");
        }
    }
    [Authorize]
    [HttpPost("send/code")]
    public async Task<IActionResult> CheckCode(int number)
    {
        try
        {
            await _email.SendCode(number, Guid.Parse(User.Identity.Name));
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while processing your request.");
        }
    }

}