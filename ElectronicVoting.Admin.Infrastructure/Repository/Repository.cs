using ElectronicVoting.Admin.Domain.Entities;
using ElectronicVoting.Admin.Infrastructure.EntityFramework;
using ElectronicVoting.Admin.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace ElectronicVoting.Admin.Infrastructure.Repository
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<long>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task RemoveAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        
        Task<PagedResult<T>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<PagedResult<T>> GetFilteredPagedResultAsync(int pageIndex, int pageSize, string search,CancellationToken cancellationToken = default);
    }

    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly DbSet<T> DbSet;
        protected readonly ElectionDbContext DbContext;

        public Repository(ElectionDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            SetCreateMetadata(entity);
            var addedEntity = await DbSet.AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken); 
            return addedEntity.Entity;
        }

        public async Task<IEnumerable<long>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await DbContext.BulkInsertAsync<T>(entities, new BulkConfig
            {
                SetOutputIdentity = true
            }, cancellationToken: cancellationToken);
            
            return entities.Select(x => x.Id);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            SetUpdateMetadata(entity);
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
                SetUpdateMetadata(entity);
            
            await DbContext.BulkUpdateAsync<T>(entities,  new BulkConfig
            {
                SetOutputIdentity = true
            }, cancellationToken: cancellationToken);
            
            await DbContext.SaveChangesAsync(cancellationToken); 
        }

        public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default) 
        {
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken); 
        }

        public async Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken); 
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }

        public Task<PagedResult<T>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsQueryable();
            return query.ToPagedResultAsync(pageIndex, pageSize, cancellationToken,true);
        }

        public Task<PagedResult<T>> GetFilteredPagedResultAsync(int pageIndex, int pageSize, string search, CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsQueryable();
            return query.ToFilteredPagedResultAsync(pageIndex, pageSize, search, cancellationToken,true);
        }

        private static void SetCreateMetadata(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
        }

        private static void SetUpdateMetadata(T entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
        }
    }
}