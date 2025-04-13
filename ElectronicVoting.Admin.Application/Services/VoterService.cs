using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;

namespace ElectronicVoting.Admin.Application.Services;

public interface IVoterService
{
    Task<Result> RegisterForElection(long voterId, long electionId, CancellationToken cancellationToken = default);
    Task<Result> UnregisterFromElection(long voterId, long electionId, CancellationToken cancellationToken = default);
}

public class VoterService: IVoterService
{
    private readonly IElectionVotersRepository _electionVotersRepository;
    
    private const string VoterNotRegistered = "Voter is not registered for election";
    private const string VoterAlreadyRegistered = "Voter is already registered for election";
    private const string VoterNotYetRegistered = "Voter is not already registered for election";
    
    public VoterService(IElectionVotersRepository electionVotersRepository)
    {
        _electionVotersRepository = electionVotersRepository;
    }

    public async Task<Result> RegisterForElection(long voterId, long electionId, CancellationToken cancellationToken = default)
    {
        var electionVoter = await _electionVotersRepository.GetByElectionIdAndVoterIdAsync(electionId, voterId);
        if(electionVoter is null)
            return Result.Fail(VoterNotRegistered);
        
        if(electionVoter.IsRegistered)
            return Result.Fail(VoterAlreadyRegistered);

        electionVoter.IsRegistered = true;
        await _electionVotersRepository.UpdateAsync(electionVoter, cancellationToken);
        
        return Result.Ok();
    }

    public async Task<Result> UnregisterFromElection(long voterId, long electionId, CancellationToken cancellationToken = default)
    {
        var electionVoter = await _electionVotersRepository.GetByElectionIdAndVoterIdAsync(electionId, voterId);
        if(electionVoter is null)
            return Result.Fail(VoterNotRegistered);
        
        if(!electionVoter.IsRegistered)
            return Result.Fail(VoterNotYetRegistered);
        
        electionVoter.IsRegistered = false;
        await _electionVotersRepository.UpdateAsync(electionVoter, cancellationToken);
        
        return Result.Ok();
    }
    
}