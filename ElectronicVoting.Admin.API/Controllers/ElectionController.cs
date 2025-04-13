using ElectronicVoting.Admin.Application.Handlers.Commands.Election;
using ElectronicVoting.Admin.Application.Handlers.Commands.VotingPublicKey;
using ElectronicVoting.Admin.Application.Handlers.Queries.Election;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[ApiController]
//[Authorize(Roles = "Admin")] 
[Route("api/[controller]")]
public class ElectionController(IMediator mediator) :BaseController(mediator)
{
    [HttpGet("paginated")]
    public async Task<IActionResult> GetElections([FromQuery] GetElections request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateElection([FromBody] CreateElection request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
    
    [HttpGet("fold-public-keys")]
    public async Task<IActionResult> FoldPublicKeys(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new FoldPublicKeys(), cancellationToken));
    }
}