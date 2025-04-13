using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IUserCredentialsRepository : IRepository<UserCredentials>
{
    Task<UserCredentials> GetByVoterIdAsync(long voterId, CancellationToken cancellationToken = default);
    Task<UserCredentials> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}

public class UserCredentialsRepository(ElectionDbContext dbContext)
    : Repository<UserCredentials>(dbContext), IUserCredentialsRepository
{
    public Task<UserCredentials> GetByVoterIdAsync(long voterId, CancellationToken cancellationToken = default)
    {
        return DbSet.FirstOrDefaultAsync(x => x.VoterId == voterId, cancellationToken);
    }

    public async Task<UserCredentials> GetByEmailAsync(string email,CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}