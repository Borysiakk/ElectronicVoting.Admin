using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IPaillierKeysRepository : IRepository<PaillierKeys>
{
    public Task<int> DeactivateAllAsync(CancellationToken cancellationToken);
}

public class PaillierKeysRepository(ElectionDbContext dbContext)
    : Repository<PaillierKeys>(dbContext), IPaillierKeysRepository
{
    public async Task<int> DeactivateAllAsync(CancellationToken cancellationToken)
    {
        return await DbSet.Where(e => e.IsActive)
            .ExecuteUpdateAsync(e => e.SetProperty(r => r.IsActive, false), cancellationToken);
    }
}