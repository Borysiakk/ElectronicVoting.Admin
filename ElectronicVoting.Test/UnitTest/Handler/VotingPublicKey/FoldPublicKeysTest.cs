using System.Text;
using ElectronicVoting.Admin.Application.Handlers.Commands.VotingPublicKey;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.Lirisi;

namespace ElectronicVoting.Test.Handler.VotingPublicKey;

public class FoldPublicKeysTest :TestBase
{
    private readonly LirisiWrapper _lirisiWrapper = new LirisiWrapper();
    
    [Fact]
    public async Task FoldPublicKeys_ShouldReturnSuccess_WhenFoldingPublicKeys()
    {
        var electionId = await PrepareVoterHandlerTestData();
        var foldPublicKeys = new FoldPublicKeys()
        {
            ElectionId = electionId,
        };

        var foldPublicKeysHandler = new FoldPublicKeysHandler(_lirisiWrapper, VoterPublicKeyRepository);
        var result = await foldPublicKeysHandler.Handle(foldPublicKeys, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
    
    private async Task<long> PrepareVoterHandlerTestData(CancellationToken cancellationToken = default)
    {
        const string format = "PEM";
        const string curveName = "prime256v1";
        const string caseIdentifier = "Round Nr1";
        byte[] caseIdentifierBytes = Encoding.UTF8.GetBytes("Round Nr1");
        
        var transaction = await ElectionDbContext.Database.BeginTransactionAsync(cancellationToken);
        
        var privateKeyA = _lirisiWrapper.GeneratePrivateKey(curveName, format);
        var publicKeyA = _lirisiWrapper.DerivePublicKey(privateKeyA.Content, format);
        
        var privateKeyB = _lirisiWrapper.GeneratePrivateKey(curveName, format);
        var publicKeyB = _lirisiWrapper.DerivePublicKey(privateKeyB.Content, format);
            
        var election = new Admin.Domain.Entities.Election()
        {
            Name = "Test election",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            IsActive = true,
            
        };
        await ElectionRepository.AddAsync(election, cancellationToken);
        
        var voterA = new Voter()
        {
            Name = "John1",
            Lastname = "Doe1",
            PersonalIdentity = "123456789",
            City = "SampleCity",
            Province = "SampleProvince",
            DateOfBirth = new DateTime(1985, 5, 15),
            CreatedDate = DateTime.Now,
            UserCredentials = new UserCredentials
            {
                Role = "Voter",
                Email = "johndoe1@example.com",
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
                    PublicKey = publicKeyA.Content,
                    PublicKeyBase64 = publicKeyA.ContentBase64,
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
        
        var voterB = new Voter()
        {
            Name = "John2",
            Lastname = "Doe2",
            PersonalIdentity = "123456789",
            City = "SampleCity",
            Province = "SampleProvince",
            DateOfBirth = new DateTime(1985, 5, 15),
            CreatedDate = DateTime.Now,
            UserCredentials = new UserCredentials
            {
                Role = "Voter",
                Email = "johndoe2@example.com",
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
                    PublicKey = publicKeyB.Content,
                    PublicKeyBase64 = publicKeyB.ContentBase64,
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
        
        await VoterRepository.AddAsync(voterA, cancellationToken);
        await VoterRepository.AddAsync(voterB, cancellationToken);
        
        
        await transaction.CommitAsync(cancellationToken);
        return election.Id;
    }
}