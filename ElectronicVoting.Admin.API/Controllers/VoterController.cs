using ElectronicVoting.Admin.Application.Handlers.Commands.Voter;
using ElectronicVoting.Admin.Application.Handlers.Queries.Voter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[ApiController]
//[Authorize(Roles = "Admin")] 
[Route("api/[controller]")]
public class VoterController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVoter(long id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetVoter(id), cancellationToken);
        if(result.IsFailed)
            return NotFound(result.Errors);
        
        return Ok(result.Value);
    }
    
    [HttpGet("paginated")]
    public async Task<IActionResult> GetVoters([FromQuery] GetVoters request,CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost("{voterId}/register-for-election/{electionId}")]
    public async Task<IActionResult> RegisterForElection([FromRoute] RegisterForElection request, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
    
    [HttpPost("{voterId}/unregister-from-election/{electionId}")]
    public async Task<IActionResult> UnregisterFromElection([FromRoute] UnregisterFromElection request,long voterId, long electionId, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }

    [HttpPost("{voterId}/public-key")]
    public async Task<IActionResult> AddPublicKeyForVoter(long voterId, CancellationToken cancellationToken)
    {
        return Ok();
    }
}