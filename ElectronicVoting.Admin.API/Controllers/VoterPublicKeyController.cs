using ElectronicVoting.Admin.Application.Handlers.Commands.VotingPublicKey;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VoterPublicKeyController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("")]
    public async Task<IActionResult> AddVoterPublicKey()
    {
        return Ok();
    }
    
}