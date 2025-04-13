using ElectronicVoting.Admin.Application.Handlers.Commands.Election;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.Paillier;

namespace ElectronicVoting.Test.Handler.Elections;


public class ElectionCreateTests : TestBase
{
    private readonly IPaillierService _paillierService = new PaillierService();

    [Fact]
    public async Task Election_CreateElectionCommand_Test()
    {
        await PrepareElectionTestData();
        var createElection = new CreateElection()
        {
            Name = "Test",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            CandidateIds = new List<long>() { 1, 2, 3 }
        };

        var createElectionHandler = new CreateElectionHandler(VoterRepository, _paillierService, ElectionRepository, PaillierKeysRepository, ElectionVotersRepository, ElectionCandidatesRepository);
        var createElectionResult = await createElectionHandler.Handle(createElection, CancellationToken.None);
        
        var electionVoters = await ElectionVotersRepository.GetAllAsync();
        var electionCandidates = await ElectionCandidatesRepository.GetAllAsync();

        Assert.NotNull(createElectionResult);
        Assert.True(createElectionResult.IsSuccess);

        Assert.NotNull(electionVoters);
        Assert.NotNull(electionCandidates);
        Assert.Equal(4, electionVoters.Count());
        Assert.Equal(3, electionCandidates.Count());
    }

    private async Task PrepareElectionTestData()
    {
        var voters = new List<Voter>
        {
            new Voter
            {
                Name = "Test voter 1",
                Lastname = "Test voter 1",
                PersonalIdentity = "1234567890"
            },
            new Voter
            {
                Name = "Test voter 2",
                Lastname = "Test voter 2",
                PersonalIdentity = "1234567891"
            },
            new Voter
            {
                Name = "Test voter 3",
                Lastname = "Test voter 3",
                PersonalIdentity = "1234567892"
            },
            new Voter
            {
                Name = "Test voter 4",
                Lastname = "Test voter 4",
                PersonalIdentity = "1234567893"
            },
        };

        await VoterRepository.AddRangeAsync(voters);
        await ElectionDbContext.SaveChangesAsync();

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
            },
            new Candidate
            {
                Name = "Test candidate 3",
                Description = "Test candidate 3 description",
                Age = 22,
                Party = "Test party 3"
            },
            new Candidate
            {
                Name = "Test candidate 4",
                Description = "Test candidate 4 description",
                Age = 24,
                Party = "Test party 4"
            },
        };

        await CandidateRepository.AddRangeAsync(candidates);
        await ElectionDbContext.SaveChangesAsync();
    }
    
}