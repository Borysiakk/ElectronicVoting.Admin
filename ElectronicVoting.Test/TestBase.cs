using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.Repository;
using ElectronicVoting.Test.Repository;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace ElectronicVoting.Test;

public class TestBase :IAsyncLifetime
{
    protected ElectionDbContext ElectionDbContext;
    protected RepositoryFactory RepositoryFactory;
    protected readonly MsSqlContainer MsSqlContainer;

    protected IVoterRepository VoterRepository;
    protected IElectionRepository ElectionRepository;
    protected IVoterPublicKeyRepository VoterPublicKeyRepository;
    protected ICandidateRepository CandidateRepository;
    protected IPaillierKeysRepository PaillierKeysRepository;
    protected IElectionVotersRepository ElectionVotersRepository;
    protected IElectionCandidatesRepository ElectionCandidatesRepository;
    protected IUserCredentialsRepository UserCredentialsRepository;
    
    protected TestBase()
    {
        MsSqlContainer = new MsSqlBuilder()
            .WithCleanUp(true)
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Your_password123")
            .Build();
    }

    protected void RegisterRepositories()
    {
        VoterRepository = RepositoryFactory.CreateRepository<VoterRepository>();
        ElectionRepository = RepositoryFactory.CreateRepository<ElectionRepository>();
        CandidateRepository = RepositoryFactory.CreateRepository<CandidateRepository>();
        PaillierKeysRepository = RepositoryFactory.CreateRepository<PaillierKeysRepository>();
        ElectionVotersRepository = RepositoryFactory.CreateRepository<ElectionVotersRepository>();
        VoterPublicKeyRepository = RepositoryFactory.CreateRepository<VoterPublicKeyRepository>();
        UserCredentialsRepository = RepositoryFactory.CreateRepository<UserCredentialsRepository>();
        ElectionCandidatesRepository = RepositoryFactory.CreateRepository<ElectionCandidatesRepository>();
    }
    
    protected async Task ClearsTables()
    {
        await ClearTableAsync<UserCredentials>();
        await ClearTableAsync<VoterPublicKey>();
        await ClearTableAsync<PaillierKeys>();
        await ClearTableAsync<ElectionCandidates>();
        await ClearTableAsync<ElectionVoters>();
        await ClearTableAsync<Election>();
        await ClearTableAsync<Candidate>();
        await ClearTableAsync<Voter>();
    }
    
    public async Task InitializeAsync()
    {
        await MsSqlContainer.StartAsync();
        
        var options = new DbContextOptionsBuilder<ElectionDbContext>()
            .UseSqlServer(MsSqlContainer.GetConnectionString())
            .Options;

        ElectionDbContext = new ElectionDbContext(options);
        await ElectionDbContext.Database.EnsureCreatedAsync();
        
        RepositoryFactory = new RepositoryFactory(ElectionDbContext);
        RegisterRepositories();
        await ClearsTables();
    }

    public async Task DisposeAsync()
    {
        await MsSqlContainer.StopAsync();
    }
    
    protected async Task ClearTableAsync<T>(CancellationToken cancellationToken = default) where T : Entity
    {
        await ElectionDbContext.Set<T>().ExecuteDeleteAsync(cancellationToken);
        
        var tableName = ElectionDbContext.Model.FindEntityType(typeof(T))?.GetTableName();
        if (tableName == null)
        {
            throw new InvalidOperationException("Nie można znaleźć nazwy tabeli dla typu " + typeof(T).Name);   
        }
        
        var sql = $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)";
        await ElectionDbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }
}