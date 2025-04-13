using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

public class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
}