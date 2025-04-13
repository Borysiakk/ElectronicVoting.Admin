using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface ICandidateRepository : IRepository<Candidate>
{

}

public class CandidateRepository(ElectionDbContext dbContext): Repository<Candidate>(dbContext), ICandidateRepository
{
    
}