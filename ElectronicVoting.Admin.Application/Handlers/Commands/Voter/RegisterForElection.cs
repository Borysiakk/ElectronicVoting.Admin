using ElectronicVoting.Admin.Application.Services;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Voter;

public record RegisterForElection : IRequest<Result>, ITransaction
{
    public long VoterId { get; init; }
    public long ElectionId { get; init; }
}

public record RegisterForElectionHandler : IRequestHandler<RegisterForElection, Result>, ITransaction
{
    private readonly IVoterService _voterService;

    public RegisterForElectionHandler(IVoterService voterService)
    {
        _voterService = voterService;
    }

    public async Task<Result> Handle(RegisterForElection request, CancellationToken cancellationToken)
    {
        return await _voterService.RegisterForElection(request.VoterId, request.ElectionId, cancellationToken);
    }
}