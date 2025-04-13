using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IElectionVotersRepository : IRepository<ElectionVoters>
{
    Task<ElectionVoters> GetByElectionIdAndVoterIdAsync(long electionId, long voterId);
}

public class ElectionVotersRepository(ElectionDbContext dbContext)
    : Repository<ElectionVoters>(dbContext), IElectionVotersRepository
{
    public Task<ElectionVoters> GetByElectionIdAndVoterIdAsync(long electionId, long voterId)
    {
        return DbSet.FirstOrDefaultAsync(x => x.ElectionId == electionId && x.VoterId == voterId);
    }
}