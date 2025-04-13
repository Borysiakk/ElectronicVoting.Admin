using Microsoft.EntityFrameworkCore.Storage;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken);
    Task Commit(CancellationToken cancellationToken);
    Task Rollback(CancellationToken cancellationToken);
    Task BeginTransaction(CancellationToken cancellationToken);
}

public class UnitOfWork(ElectionDbContext databaseContext) : IUnitOfWork, IDisposable
{
    protected IDbContextTransaction Transaction;
    protected readonly ElectionDbContext DatabaseContext = databaseContext;
    
    public async Task BeginTransaction(CancellationToken cancellationToken)
    {
        if (Transaction != null)
            throw new InvalidOperationException("Transaction is already started");
        
        Transaction = await DatabaseContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task Commit(CancellationToken cancellationToken)
    {
        if (Transaction == null)
            throw new InvalidOperationException("Transaction is not created");

        await DatabaseContext.SaveChangesAsync(cancellationToken);
        await Transaction.CommitAsync(cancellationToken);
    }

    public async Task Rollback(CancellationToken cancellationToken)
    {
        if (Transaction == null)
            throw new InvalidOperationException("Transaction is not created");

        await Transaction.RollbackAsync(cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        await DatabaseContext.SaveChangesAsync(cancellationToken);
    }
    
    public void Dispose()
    {
        try
        {
            
        }
        finally
        {
            Transaction?.Dispose();
            Transaction = null;
        }
    }
}