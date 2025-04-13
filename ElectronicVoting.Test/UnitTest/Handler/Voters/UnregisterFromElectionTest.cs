using ElectronicVoting.Admin.Application.Handlers.Commands.Voter;
using ElectronicVoting.Admin.Application.Services;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;

namespace ElectronicVoting.Test.Handler.Voters;

public class UnregisterFromElectionTest : TestBase
{
    [Fact]
    public async Task UnregisterForElection_ShouldUnregisterVoterForElection_IsExecuted()
    {
        var item = await PrepareElectionTestData();

        var unregisterFromElection = new UnregisterFromElection()
        {
            VoterId = item.voterId,
            ElectionId = item.electionId
        };

        IVoterService voterService = new VoterService(ElectionVotersRepository);
        var unregisterFromElectionHandler = new UnregisterFromElectionHandler(voterService);
        var unregisterFromElectionResult = await unregisterFromElectionHandler.Handle(unregisterFromElection, CancellationToken.None);
        
        Assert.NotNull(unregisterFromElectionResult);
        Assert.True(unregisterFromElectionResult.IsSuccess);
    }
    
    private async Task<(long voterId, long electionId)> PrepareElectionTestData(CancellationToken cancellationToken = default)
    {
        var transaction = await ElectionDbContext.Database.BeginTransactionAsync(cancellationToken);
        
        var voter = new Voter
        {
            Name = "Test voter 1",
            Lastname = "Test voter 1",
            PersonalIdentity = "1234567890"
        };
        await VoterRepository.AddAsync(voter, cancellationToken);
        
        var candidates = new List<Candidate>
        {
            new Candidate
            {
                Name = "Test candidate 1",
                Description = "Test candidate 1 description",
                Age = 18,
                Party = "Test party 1"
            },
            new Candidate
            {
                Name = "Test candidate 2",
                Description = "Test candidate 2 description",
                Age = 20,
                Party = "Test party 2"
            }
        };
        await CandidateRepository.AddRangeAsync(candidates, cancellationToken);
        
        var election = new Election
        {
            Name = 
            "Test Election 1",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(7),
            PaillierKeys = new List<PaillierKeys>
            {
                new PaillierKeys
                {
                    IsActive = true,
                    PublicKey = "SamplePublicKey",
                    PrivateKey = "SamplePrivateKey"
                }
            },
            ElectionVoters = new List<ElectionVoters>
            {
                new ElectionVoters
                {
                    Voter = voter,
                    IsRegistered = true
                }
            },
            ElectionCandidates = new List<ElectionCandidates>
            {
                
            }
        };
        await ElectionRepository.AddAsync(election, cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        return (voter.Id, election.Id);
    }
}