using ElectronicVoting.Admin.Application.Handlers.Commands.ElectionManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ElectionManagementController(IMediator mediator) :BaseController(mediator)
{
    [HttpPost("Paillier/generate-paillier-keys")]
    public async Task<IActionResult> GeneratePaillierKeys(GeneratePaillierKeys request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(request, cancellationToken);
        if(result.IsSuccess)
            return Ok(result.Value);
        
        return BadRequest(result.Errors);
    }

    public async Task<IActionResult> FoldPublicKeys()
    {
        return Ok();
    }
}