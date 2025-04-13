using ElectronicVoting.Admin.Infrastructure.EntityFramework;

namespace ElectronicVoting.Test.Repository;

public class RepositoryFactory
{
    private readonly ElectionDbContext _dbContext;
    
    public RepositoryFactory(ElectionDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public T CreateRepository<T>() where T : class
    {
        return Activator.CreateInstance(typeof(T), _dbContext) as T 
               ?? throw new InvalidOperationException($"Could not create repository of type {typeof(T).Name}");
    }
}