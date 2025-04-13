using System.Text.Json;
using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Dtos.Election;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using ElectronicVoting.Admin.Infrastructure.Paillier;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Election;

public record CreateElection : IRequest<Result<ElectionDto>>, ITransaction
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<long> CandidateIds { get; set; }
}

public class CreateElectionHandler(
    IVoterRepository voterRepository,
    IPaillierService paillierService,
    IElectionRepository electionRepository,
    IPaillierKeysRepository paillierKeysRepository,
    IElectionVotersRepository electionVotersRepository,
    IElectionCandidatesRepository electionCandidatesRepository)
    : IRequestHandler<CreateElection, Result<ElectionDto>>, ITransaction
{
    public async Task<Result<ElectionDto>> Handle(CreateElection request, CancellationToken cancellationToken)
    {
        var election = await CreateElection(request, cancellationToken);
        
        var paillierKeysResult= await CreatePaillierKeys(election.Id, cancellationToken);
        if (!paillierKeysResult)
            return Result.Fail<ElectionDto>("Error creating Paillier keys");
        
        await CreateElectionVoters(election.Id, request.CandidateIds, cancellationToken);
        await CreateElectionCandidates(election.Id, request.CandidateIds, cancellationToken);
        
        return Result.Ok();
    }

    private async Task<Domain.Entities.Election> CreateElection(CreateElection request,
        CancellationToken cancellationToken)
    {
        var election = new Domain.Entities.Election()
        {
            Name = request.Name,
            EndDate = DateTime.Now.AddDays(1),
            StartDate = DateTime.Now.AddDays(2),
        };
        return await electionRepository.AddAsync(election, cancellationToken);
    }

    private async Task<IEnumerable<ElectionCandidates>> CreateElectionCandidates(long electionId,
        IEnumerable<long> candidateIds, CancellationToken cancellationToken)
    {
        var electionCandidates = candidateIds.Select(candidateId => new ElectionCandidates()
        {
            ElectionId = electionId, 
            CandidateId = candidateId,
            CreatedDate = DateTime.Now,
        }).ToList();
        
        await electionCandidatesRepository.AddRangeAsync(electionCandidates, cancellationToken);
        
        return electionCandidates;
    }

    private async Task CreateElectionVoters(long electionId, IEnumerable<long> voterIds, CancellationToken cancellationToken)
    {
        var votersIds = await voterRepository.GetAllIdsAsync(cancellationToken);
        
        var electionVoters = votersIds.Select(voterId => new ElectionVoters() 
        { 
            VoterId = voterId,
            ElectionId = electionId,
            CreatedDate = DateTime.Now,
        }).ToList();
        
        await electionVotersRepository.AddRangeAsync(electionVoters, cancellationToken);
    }

    private async Task<bool> CreatePaillierKeys(long electionId, CancellationToken cancellationToken)
    {
        var resultPaillierKeys = paillierService.Generate(80, 10);
        if(resultPaillierKeys.IsFailed)
            return false;

        var paillierKeys = resultPaillierKeys.Value;
        var paillierKeysEntity = new PaillierKeys
        {
            IsActive = true,
            ElectionId = electionId, 
            PublicKey = JsonSerializer.Serialize(paillierKeys.PublicKey),
            PrivateKey = JsonSerializer.Serialize(paillierKeys.PrivateKey),
        };
        
        await paillierKeysRepository.AddAsync(paillierKeysEntity, cancellationToken);
        return true;
    }
}