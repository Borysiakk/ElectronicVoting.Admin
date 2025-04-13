using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IElectionRepository : IRepository<Election>
{
    Task<IEnumerable<Election>> GetAllByVoterIdAsync(long voterId, CancellationToken cancellationToken = default);
}

public class ElectionRepository(ElectionDbContext dbContext) 
    : Repository<Election>(dbContext), IElectionRepository
{
    public async Task<IEnumerable<Election>> GetAllByVoterIdAsync(long voterId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(e => e.ElectionVoters)
            .Where(e => e.ElectionVoters.Any(ev => ev.VoterId == voterId))
            .ToListAsync(cancellationToken);
    }
}