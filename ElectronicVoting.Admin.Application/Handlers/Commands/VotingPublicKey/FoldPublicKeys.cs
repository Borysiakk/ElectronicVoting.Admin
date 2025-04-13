using ElectronicVoting.Admin.Infrastructure.Lirisi;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.VotingPublicKey;

public class FoldPublicKeys: IRequest<Result<byte[]>>
{
    public long ElectionId { get; set; }
}

public class FoldPublicKeysHandler: IRequestHandler<FoldPublicKeys, Result<byte[]>>
{
    private readonly LirisiWrapper _lirisiWrapper;
    private readonly IVoterPublicKeyRepository _voterPublicKeyRepository;

    public FoldPublicKeysHandler(LirisiWrapper lirisiWrapper, IVoterPublicKeyRepository voterPublicKeyRepository)
    {
        _lirisiWrapper = lirisiWrapper;
        _voterPublicKeyRepository = voterPublicKeyRepository;
    }

    public async Task<Result<byte[]>> Handle(FoldPublicKeys request, CancellationToken cancellationToken)
    {
        var voterPublicKeys = await _voterPublicKeyRepository.GetByElectionIdAsync(request.ElectionId, cancellationToken);
        var publicKeys = voterPublicKeys.Select(a => a.PublicKey).ToArray();
        if (publicKeys.Length == 0)
            return Result.Fail<byte[]>("No public keys found");
        
        var foldPublicKeys = _lirisiWrapper.FoldPublicKeys(publicKeys, "sha3-256", "PEM", "hashes");
        
        return Result.Ok<byte[]>(foldPublicKeys.Content);
    }
}