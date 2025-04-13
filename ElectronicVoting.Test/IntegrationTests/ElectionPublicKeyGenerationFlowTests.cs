using System.Text;
using ElectronicVoting.Admin.Application.Handlers.Commands.Voter;
using ElectronicVoting.Admin.Application.Handlers.Commands.VotingPublicKey;
using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.Lirisi;

namespace ElectronicVoting.Test.IntegrationTests;

public class ElectionPublicKeyGenerationAnd1VerifyFlowTests :TestBase
{
    private const string _format = "PEM";
    private const string _curveName = "prime256v1";
    private readonly LirisiWrapper _lirisiWrapper = new LirisiWrapper();

    [Fact]
    public async Task Integration_AddVoterKeysAndGenerateElectionKey_VerifyElectionPublicKey()
    {
        var voterOnePrivateKey = _lirisiWrapper.GeneratePrivateKey(_curveName, _format);
        var voterOnePublicKey = _lirisiWrapper.DerivePublicKey(voterOnePrivateKey.Content, _format);
        
        var voterTwoPrivateKey = _lirisiWrapper.GeneratePrivateKey(_curveName, _format);
        var voterTwoPublicKey = _lirisiWrapper.DerivePublicKey(voterTwoPrivateKey.Content, _format);
        
        var item = await PrepareVoterHandlerTestData();
        var addPublicKeyForOneVoter = new AddPublicKeyForVoter()
        {
            VoterId = item.voterIds[0],
            ElectionId = item.electionId,
            PublicKey = voterOnePublicKey.Content
        };
        
        var addPublicKeyForTwoVoter = new AddPublicKeyForVoter()
        {
            VoterId = item.voterIds[1],
            ElectionId = item.electionId,
            PublicKey = voterTwoPublicKey.Content
        };
        
        var addPublicKeyForOneVoterHandler = new AddPublicKeyForVoterHandler(VoterPublicKeyRepository);
        var addPublicKeyForFirstVoterResult  = await addPublicKeyForOneVoterHandler.Handle(addPublicKeyForOneVoter, CancellationToken.None);
        var addPublicKeyForSecondVoterResult = await addPublicKeyForOneVoterHandler.Handle(addPublicKeyForTwoVoter, CancellationToken.None);
        
        Assert.NotNull(addPublicKeyForFirstVoterResult);
        Assert.NotNull(addPublicKeyForSecondVoterResult);
        Assert.True(addPublicKeyForFirstVoterResult.IsSuccess);
        Assert.True(addPublicKeyForSecondVoterResult.IsSuccess);
        
        var foldPublicKeys = new FoldPublicKeys()
        {
            ElectionId = item.electionId,
        };
        
        var foldPublicKeysHandler = new FoldPublicKeysHandler(_lirisiWrapper, VoterPublicKeyRepository);
        var foldPublicKeysResult = await foldPublicKeysHandler.Handle(foldPublicKeys, CancellationToken.None);
        
        
        Assert.NotNull(foldPublicKeysResult);
        Assert.True(foldPublicKeysResult.IsSuccess);

        const string messageFirst = "It is message one";
        const string messageSecond = "It is message two";
        byte[] messageFirstBytes = Encoding.UTF8.GetBytes(messageFirst);
        byte[] messageSecondBytes = Encoding.UTF8.GetBytes(messageSecond);
        
        var createSignatureResult = _lirisiWrapper.CreateSignature(foldPublicKeysResult.Value, voterOnePrivateKey.Content, messageFirstBytes, Encoding.UTF8.GetBytes("Round Nr1"), _format);
        var verifySignatureResult = _lirisiWrapper.VerifySignature(foldPublicKeysResult.Value, createSignatureResult.Content, messageFirstBytes, Encoding.UTF8.GetBytes("Round Nr1"));

        Assert.Equal(0, verifySignatureResult);
    }
    
    private async Task<(long electionId, long[] voterIds)> PrepareVoterHandlerTestData(CancellationToken cancellationToken = default)
    {
        const string format = "PEM";
        const string curveName = "prime256v1";
        
        var privateKeyA = _lirisiWrapper.GeneratePrivateKey(curveName, format);
        var publicKeyA = _lirisiWrapper.DerivePublicKey(privateKeyA.Content, format);
        
        var privateKeyB = _lirisiWrapper.GeneratePrivateKey(curveName, format);
        var publicKeyB = _lirisiWrapper.DerivePublicKey(privateKeyB.Content, format);
        
        var transaction = await ElectionDbContext.Database.BeginTransactionAsync(cancellationToken);
        
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
            PublicKeys = new List<VoterPublicKey>()
            {
                
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
            PublicKeys = new List<VoterPublicKey>()
            {
                
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
        
        return new ValueTuple<long, long[]>(election.Id, [voterA.Id, voterB.Id]);
    }
}