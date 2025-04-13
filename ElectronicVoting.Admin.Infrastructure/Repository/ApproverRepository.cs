using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;

namespace ElectronicVoting.Admin.Infrastructure.Repository;

public interface IApproverRepository : IRepository<Approver> { }

public class ApproverRepository(ElectionDbContext dbContext) 
    : Repository<Approver>(dbContext), IApproverRepository;