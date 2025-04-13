using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IVoterRepository : IRepository<Voter>
{
    Task<IEnumerable<long>> GetAllIdsAsync(CancellationToken cancellationToken = default);
}

public class VoterRepository(ElectionDbContext dbContext) : Repository<Voter>(dbContext), IVoterRepository
{
    public async Task<IEnumerable<long>> GetAllIdsAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Select(x => x.Id).ToListAsync(cancellationToken);
    }
}