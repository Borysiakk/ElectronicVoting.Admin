using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Dtos.Election;
using ElectronicVoting.Admin.Application.Dtos.Voter;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Queries.Voter;

public record GetVoter(long Id) : IRequest<Result<VoterDetailsDto>>;

public class GetVoterHandler : IRequestHandler<GetVoter, Result<VoterDetailsDto>>
{
    private readonly IVoterRepository _voterRepository;
    private readonly IElectionRepository _electionRepository;
    private readonly IVoterPublicKeyRepository _voterPublicKeyRepository;
    private readonly IUserCredentialsRepository _userCredentialsRepository;

    public GetVoterHandler(IVoterRepository voterRepository, IElectionRepository electionRepository,
        IVoterPublicKeyRepository voterPublicKeyRepository, IUserCredentialsRepository userCredentialsRepository)
    {
        _voterRepository = voterRepository;
        _electionRepository = electionRepository;
        _voterPublicKeyRepository = voterPublicKeyRepository;
        _userCredentialsRepository = userCredentialsRepository;
    }

    public async Task<Result<VoterDetailsDto>> Handle(GetVoter request, CancellationToken cancellationToken)
    {
        var voter = await _voterRepository.GetByIdAsync(request.Id, cancellationToken);
        return voter is null
            ? Result.Fail<VoterDetailsDto>("Voter not found")
            : Result.Ok(await GetCompleteVoterDetailsAsync(voter, cancellationToken));
    }

    private async Task<VoterDetailsDto> GetCompleteVoterDetailsAsync(Domain.Entities.Voter voter,
        CancellationToken cancellationToken)
    {
        var userCredentials = await _userCredentialsRepository.GetByVoterIdAsync(voter.Id, cancellationToken);
        var elections = await _electionRepository.GetAllByVoterIdAsync(voter.Id, cancellationToken);
        var publicKeys = await _voterPublicKeyRepository.GetPublicKeysByVoterIdAsync(voter.Id, cancellationToken);

        return new VoterDetailsDto()
        {
            Name = voter.Name,
            Lastname = voter.Lastname,
            PersonalIdentity = voter.PersonalIdentity,
            Role = userCredentials.Role,
            Email = userCredentials.Email,
            Elections = MapElectionsToDto(voter.Id, elections),
            PublicKeys = MapPublicKeysToDto(publicKeys)
        };
    }

    private IEnumerable<ElectionDetailsDto> MapElectionsToDto(long voterId, IEnumerable<Domain.Entities.Election> elections)
    {
        return elections?.Select(ep => new ElectionDetailsDto
        {
            Id = ep.Id,
            Name = ep.Name,
            IsActive = ep.IsActive,
            StartDate = ep.StartDate,
            EndDate = ep.EndDate,
            IsRegistered = ep.ElectionVoters.FirstOrDefault(a=>a.ElectionId == ep.Id && a.VoterId == voterId).IsRegistered,
        }) ?? Enumerable.Empty<ElectionDetailsDto>();
    }

    private IEnumerable<VoterPublicKeyDto> MapPublicKeysToDto(IEnumerable<Domain.Entities.VoterPublicKey> publicKeys)
    {
        return publicKeys?.Select(pk => new VoterPublicKeyDto
        {
            Id = pk.Id,
            ElectionId = pk.ElectionId,
            PublicKey = pk.PublicKey,
            PublicKeyBase64 = pk.PublicKeyBase64
        }) ?? Enumerable.Empty<VoterPublicKeyDto>();
    }
}