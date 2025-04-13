using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Voter;

public record AddPublicKeyForVoter: IRequest<Result>, ITransaction
{
    public long VoterId { get; set; }
    public long ElectionId { get; set; }
    public byte[] PublicKey { get; set; }
}

public class AddPublicKeyForVoterHandler : IRequestHandler<AddPublicKeyForVoter, Result>, ITransaction
{
    private readonly IVoterPublicKeyRepository _voterPublicKeyRepository;

    public AddPublicKeyForVoterHandler(IVoterPublicKeyRepository voterPublicKeyRepository)
    {
        _voterPublicKeyRepository = voterPublicKeyRepository;
    }

    public async Task<Result> Handle(AddPublicKeyForVoter request, CancellationToken cancellationToken)
    {
        await CreateVoterPublicKey(request.VoterId, request.ElectionId, request.PublicKey, cancellationToken);
        await DeactivateVoterPublicKeys(request.VoterId, request.ElectionId, cancellationToken);
        
        return Result.Ok();
    }

    private async Task CreateVoterPublicKey(long voterId, long electionId, byte[] publicKey, CancellationToken cancellationToken = default)
    {
        var voterPublicKey = new VoterPublicKey()
        {
            IsActive = true,
            VoterId = voterId,
            ElectionId = electionId,
            PublicKey = publicKey
        };
        
        await _voterPublicKeyRepository.AddAsync(voterPublicKey, cancellationToken);
    }
    
    private async Task DeactivateVoterPublicKeys(long voterId, long electionId, CancellationToken cancellationToken)
    {
        var voterPublicKeys = await _voterPublicKeyRepository.GetByVoterIdAndElectionIdAsync(voterId, electionId, cancellationToken);
        foreach (var voterPublicKey in voterPublicKeys)
            voterPublicKey.IsActive = false;
        
        await _voterPublicKeyRepository.UpdateRangeAsync(voterPublicKeys, cancellationToken);
    }
}