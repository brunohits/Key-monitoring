using Key_monitoring.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Key_monitoring.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Key_monitoring.Controllers;
[ApiController]
[Route("api/account")]
public class СonfirmationController : ControllerBase
{
    private readonly IDeanOffice _deanOffice;

    public СonfirmationController(IDeanOffice deanOffice)
    {
        _deanOffice = deanOffice;
    }
    
    [Authorize]
    [HttpPut]
    [Route("Change/Role")]
    public async Task<IActionResult> ChangeRole(RoleEnum roleEnum, Guid idUser)
    {
        try
        {
            await _deanOffice.GiveRole(Guid.Parse(User.Identity.Name), roleEnum, idUser);
            return Ok();
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
        }

        return StatusCode(500, "Ошибка базы данных");
    }
}