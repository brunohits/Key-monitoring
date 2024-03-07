using Microsoft.AspNetCore.Mvc;
using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Authorization;

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

    public async Task<IActionResult> SpecialityGet(Guid id, int numberRoom)
    {
        try
        {
            bool isEmailSent = await _email.SendEmail(id, Guid.Parse(User.Identity.Name), numberRoom);
            if (isEmailSent)
                return Ok();
            else
                return BadRequest("Failed to send email.");
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
    [Authorize]
    [HttpPost("send/code")]
    public async Task<IActionResult> CheckCode(int number)
    {
        try
        {
            await _email.SendCode(number, Guid.Parse(User.Identity.Name));
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}