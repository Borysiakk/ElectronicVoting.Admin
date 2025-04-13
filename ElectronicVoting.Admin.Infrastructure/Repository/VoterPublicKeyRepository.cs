using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IVoterPublicKeyRepository : IRepository<VoterPublicKey>
{
    Task<IEnumerable<VoterPublicKey>> GetByElectionIdAsync(long electionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VoterPublicKey>> GetPublicKeysByVoterIdAsync(long voterId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<VoterPublicKey>> GetByVoterIdAndElectionIdAsync(long voterId, long electionId, CancellationToken cancellationToken = default);
}

public class VoterPublicKeyRepository(ElectionDbContext dbContext) : Repository<VoterPublicKey>(dbContext), IVoterPublicKeyRepository
{
    public async Task<IEnumerable<VoterPublicKey>> GetByElectionIdAsync(long electionId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => x.ElectionId == electionId).ToListAsync(cancellationToken); 
    }

    public async Task<IEnumerable<VoterPublicKey>> GetPublicKeysByVoterIdAsync(long voterId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => x.VoterId == voterId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VoterPublicKey>> GetByVoterIdAndElectionIdAsync(long voterId, long electionId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(x => x.VoterId == voterId && x.ElectionId == electionId).ToListAsync(cancellationToken);
    }
}