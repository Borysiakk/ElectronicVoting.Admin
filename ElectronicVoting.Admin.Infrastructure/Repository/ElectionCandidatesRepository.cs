using ElectronicVoting.Admin.Domain.Entities.Elections;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IElectionCandidatesRepository: IRepository<ElectionCandidates>
{
    
}

public class ElectionCandidatesRepository(ElectionDbContext dbContext)
    : Repository<ElectionCandidates>(dbContext), IElectionCandidatesRepository
{
    
}