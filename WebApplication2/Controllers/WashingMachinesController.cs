using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTOs;
using WebApplication2.Exceptions;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/washing-machines")]
public class WashingMachinesController(IWashingMachineService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddWashingMachine([FromBody] CreateWashingMachineRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await service.AddWashingMachineAsync(request, cancellationToken);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}
