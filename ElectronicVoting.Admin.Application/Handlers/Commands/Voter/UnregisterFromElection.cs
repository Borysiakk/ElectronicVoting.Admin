using ElectronicVoting.Admin.Application.Services;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Voter;

public record UnregisterFromElection : IRequest<Result>, ITransaction
{
    public long VoterId { get; init; }
    public long ElectionId { get; init; }
}

public class UnregisterFromElectionHandler : IRequestHandler<UnregisterFromElection, Result>, ITransaction
{
    private readonly IVoterService _voterService;

    public UnregisterFromElectionHandler(IVoterService voterService)
    {
        _voterService = voterService;
    }
    
    public async Task<Result> Handle(UnregisterFromElection request, CancellationToken cancellationToken)
    {
        return await _voterService.UnregisterFromElection(request.VoterId, request.ElectionId, cancellationToken);
    }
}