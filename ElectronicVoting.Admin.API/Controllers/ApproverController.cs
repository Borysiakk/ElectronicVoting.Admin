using ElectronicVoting.Admin.Application.Handlers.Queries.Approver;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")] 
[Route("api/[controller]")]
public class ApproverController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("paginated")]
    public async Task<IActionResult> GetApprovers([FromQuery] GetApprovers request,CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
}