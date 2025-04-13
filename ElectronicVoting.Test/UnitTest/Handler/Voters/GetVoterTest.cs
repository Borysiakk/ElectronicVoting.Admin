using System.Security.Cryptography.X509Certificates;
using ElectronicVoting.Admin.Application.Handlers.Queries.Voter;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.Paillier;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;

namespace ElectronicVoting.Test.Handler.Voters;

public class GetVoterTest : TestBase
{
    [Fact]
    public async Task GetVoterHandler_ShouldReturnVoterDetails_WhenVoterExists()
    {
        await ClearsTables();
        long voterId = await PrepareVoterHandlerTestData();
        
        var getVoter = new GetVoter(voterId);
        var getVoterHandler = new GetVoterHandler(VoterRepository, ElectionRepository, VoterPublicKeyRepository, UserCredentialsRepository);
        var getVoterResult = await getVoterHandler.Handle(getVoter, CancellationToken.None);
        
        Assert.NotNull(getVoterResult);
        Assert.NotNull(getVoterResult.Value);
        Assert.True(getVoterResult.IsSuccess);
        Assert.Equal("John", getVoterResult.Value.Name);
        Assert.Equal("Doe", getVoterResult.Value.Lastname);
        Assert.Equal("123456789", getVoterResult.Value.PersonalIdentity);
        Assert.Equal("Voter", getVoterResult.Value.Role);
        Assert.Equal("johndoe@example.com", getVoterResult.Value.Email);
        Assert.Single(getVoterResult.Value.Elections);
        Assert.Equal("Test election", getVoterResult.Value.Elections.First().Name);
        Assert.Single(getVoterResult.Value.PublicKeys);
    }

    private async Task<long> PrepareVoterHandlerTestData(CancellationToken cancellationToken = default)
    {
        var transaction = await ElectionDbContext.Database.BeginTransactionAsync(cancellationToken);
        
        var election = new Admin.Domain.Entities.Election()
        {
            Name = "Test election",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            IsActive = true,
            
        };
        await ElectionRepository.AddAsync(election, cancellationToken);
        
        var voter = new Voter()
        {
            Name = "John",
            Lastname = "Doe",
            PersonalIdentity = "123456789",
            City = "SampleCity",
            Province = "SampleProvince",
            DateOfBirth = new DateTime(1985, 5, 15),
            CreatedDate = DateTime.Now,
            UserCredentials = new UserCredentials
            {
                Role = "Voter",
                Email = "johndoe@example.com",
                PasswordHash = "hashed_password",
                PasswordSalt = "salt_value",
                CreatedDate = DateTime.Now,
            },
            PublicKeys = new List<VoterPublicKey>
            {
                new VoterPublicKey
                {
                    ElectionId = election.Id,
                    CreatedDate = DateTime.Now,
                    PublicKey = new byte[] { 1, 2, 3, 4 },
                    PublicKeyBase64 = Convert.ToBase64String(new byte[] { 1, 2, 3, 4 })
                }
            },
            ElectionVoters = new List<ElectionVoters>
            {
                new ElectionVoters
                {
                    ElectionId = election.Id,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                }
            }
        };
        
        await VoterRepository.AddAsync(voter, cancellationToken);
        
        var electionVoter = new ElectionVoters()
        {
            ElectionId = election.Id,
            IsActive = true,
            CreatedDate = DateTime.Now,
            VoterId = voter.Id,
        };
        
        await ElectionVotersRepository.AddAsync(electionVoter, cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        return voter.Id;
    }
}