using System.Text.Json;
using ElectronicVoting.Admin.Application.Dtos.Paillier;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using ElectronicVoting.Admin.Infrastructure.Paillier;
using ElectronicVoting.Admin.Infrastructure.Paillier.Models;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.ElectionManagement;

public class GeneratePaillierKeys : IRequest<Result<PaillierKeysDto>>, ITransaction
{
    public int BitLength { get; set; }
    public int PrimeRounds { get; set; }
}

public class GeneratePaillierKeysHandler(
    IPaillierService paillierService,
    IPaillierKeysRepository paillierKeysRepository)
    : IRequestHandler<GeneratePaillierKeys, Result<PaillierKeysDto>>
{
    public async Task<Result<PaillierKeysDto>> Handle(GeneratePaillierKeys request, CancellationToken cancellationToken)
    {
        var paillierKeysResult = paillierService.Generate(request.BitLength, request.PrimeRounds);
        if (paillierKeysResult.IsFailed)
            return Result.Fail<PaillierKeysDto>(paillierKeysResult.Errors);

        await DeactivateAndInsertAsync(paillierKeysResult.Value, cancellationToken);
        return Result.Ok(MapPaillierKeysResultToDto(paillierKeysResult.Value));
    }

    private async Task DeactivateAndInsertAsync(PaillierKeysResult paillierKeys, CancellationToken cancellationToken)
    {
        try
        {
            var paillierKeysEntity = new PaillierKeys
            {
                IsActive = true,
                PublicKey = JsonSerializer.Serialize(paillierKeys.PublicKey),
                PrivateKey = JsonSerializer.Serialize(paillierKeys.PrivateKey)
            };

            await paillierKeysRepository.DeactivateAllAsync(cancellationToken);
            await paillierKeysRepository.AddAsync(paillierKeysEntity, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Błąd podczas deaktywacji lub wstawienia klucza Paillier.", ex);
        }
    }

    private PaillierKeysDto MapPaillierKeysResultToDto(PaillierKeysResult paillierKeys)
    {
        return new PaillierKeysDto
        {
            PublicKey = new PaillierPublicKeyDto
            {
                G = paillierKeys.PublicKey.G,
                N = paillierKeys.PublicKey.N,
                R = paillierKeys.PublicKey.R
            },
            PrivateKey = new PaillierPrivateKeyDto
            {
                Lambda = paillierKeys.PrivateKey.Lambda,
                Mi = paillierKeys.PrivateKey.Mi,
                P = paillierKeys.PrivateKey.P,
                Q = paillierKeys.PrivateKey.Q
            }
        };
    }
}