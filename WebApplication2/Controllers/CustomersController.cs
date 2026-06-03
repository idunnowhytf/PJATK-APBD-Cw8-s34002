using Microsoft.AspNetCore.Mvc;
using WebApplication2.Exceptions;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IWashingMachineService service) : ControllerBase
{
    [HttpGet("{id:int}/purchases")]
    public async Task<IActionResult> GetCustomerPurchases(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await service.GetCustomerPurchasesAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}
