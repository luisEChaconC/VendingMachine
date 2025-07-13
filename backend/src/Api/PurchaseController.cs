using Microsoft.AspNetCore.Mvc;
using Backend.Application.Commands.Purchase;
using Backend.Application.Dtos;
using Backend.Application.Errors;

namespace Backend.Api;

[ApiController]
[Route("api/purchase")]
public class PurchaseController(IPurchaseProductCommand purchaseProductCommand) : ControllerBase
{
    [HttpPost]
    public IActionResult Purchase([FromBody] PurchaseRequestDto purchaseRequest)
    {
        try
        {
            var changeResponse = purchaseProductCommand.Execute(purchaseRequest);
            return Ok(changeResponse);
        }
        catch (PurchaseException ex)
        {
            return BadRequest(new
            {
                error = ex.ErrorType.ToString(),
                message = ex.Message
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "InternalServerError", message = "An unexpected error occurred" });
        }
    }
}
