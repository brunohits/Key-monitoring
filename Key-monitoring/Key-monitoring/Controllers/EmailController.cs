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
    [HttpPost("Search/Users")]

    public async Task<IActionResult> specialityGet(Guid id)
    {
        try
        {
            bool isEmailSent = await _email.SendEmail(id, Guid.Parse(User.Identity.Name));
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

}