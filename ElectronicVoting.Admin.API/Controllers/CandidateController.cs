using ElectronicVoting.Admin.Application.Handlers.Commands.Candidate;
using ElectronicVoting.Admin.Application.Handlers.Queries.Candidate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")] 
[Route("api/[controller]")]
public class CandidateController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateCandidate(CreateCandidate addCandidate, CancellationToken cancellationToken)
    {
        var resultAddCandidate = await Mediator.Send(addCandidate, cancellationToken);
    
        if (resultAddCandidate.IsFailed)
            return BadRequest(resultAddCandidate.Errors);

        return CreatedAtAction(nameof(GetCandidate), new { id = resultAddCandidate.Value.Id }, resultAddCandidate.Value);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCandidate(long id, CancellationToken cancellationToken)
    {
        var resultGetCandidate = await Mediator.Send(new GetCandidate(id), cancellationToken);
        if(resultGetCandidate.IsFailed)
            return NotFound(resultGetCandidate.Errors);

        return Ok(resultGetCandidate.Value);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetCandidates([FromQuery] GetCandidates request,CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
    
    
}