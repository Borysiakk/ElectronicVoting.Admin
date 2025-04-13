using ElectronicVoting.Admin.Application.Handlers.Commands.Voter;
using ElectronicVoting.Admin.Application.Services;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;

namespace ElectronicVoting.Test.Handler.Voters;

public class RegisterForElectionTest : TestBase
{
    [Fact]
    public async Task RegisterForElection_ShouldRegisterVoterForElection_IsExecuted()
    {
        var item = await PrepareElectionTestData();

        var registerForElection = new RegisterForElection()
        {
            VoterId = item.voterId,
            ElectionId = item.electionId
        };

        IVoterService voterService = new VoterService(ElectionVotersRepository);
        var registerForElectionHandler = new RegisterForElectionHandler(voterService);
        var registerForElectionResult = await registerForElectionHandler.Handle(registerForElection, CancellationToken.None);
        
        Assert.NotNull(registerForElectionResult);
        Assert.True(registerForElectionResult.IsSuccess);
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
                    IsRegistered = false
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