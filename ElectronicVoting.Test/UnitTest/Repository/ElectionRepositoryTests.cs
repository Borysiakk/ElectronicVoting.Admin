using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace ElectronicVoting.Test.Repository;

public class ElectionRepositoryTests :TestBase
{
    [Fact]
    public async Task AddElection_ShouldAddAndRetrieveElection()
    {
        var electionRepository = RepositoryFactory.CreateRepository<ElectionRepository>();
        
        var newElection = CreateTestElection();
        
        await electionRepository.AddAsync(newElection);
        await ElectionDbContext.SaveChangesAsync();
        
        var election = await electionRepository.GetByIdAsync(newElection.Id);
        Assert.NotNull(election);
        Assert.Equal(newElection.Name, election.Name);
    }

    [Fact]
    public async Task AddElections_ShouldAddAndRetrieveAllElections()
    {
        await ClearTableAsync<Election>();
        var electionRepository = RepositoryFactory.CreateRepository<ElectionRepository>();

        var newElections = CreateTestElections().ToList();
        
        await electionRepository.AddRangeAsync(newElections);
        await ElectionDbContext.SaveChangesAsync();
        
        var elections = await electionRepository.GetAllAsync();
        
        Assert.NotNull(elections);
        Assert.Equal(newElections.Count(), elections.Count());
    }
    
    private Election CreateTestElection()
    {
        return new Election
        {
            Name = "Test election",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            IsActive = true,
        };
    }

    private IEnumerable<Election> CreateTestElections()
    {
        return new List<Election>
        {
            new Election
            {
                Name = "Test election 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                IsActive = true,
            },
            new Election
            {
                Name = "Test election 2",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                IsActive = true,
            },
            new Election
            {
                Name = "Test election 3",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                IsActive = false,
            }
        };
    }
}