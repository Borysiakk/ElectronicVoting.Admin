using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicVoting.Admin.Infrastructure.Pagination;

public static class QueryablePaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default,
        bool includeTotalCount = true)
    {
        if (pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index cannot be negative.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");
        
        int totalCount = includeTotalCount ? await source.CountAsync(cancellationToken) : -1;

        var items = await source.Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public static async Task<PagedResult<T>> ToFilteredPagedResultAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        string searchTerm,
        CancellationToken cancellationToken = default,
        bool includeTotalCount = true)
    {
        if (pageIndex < 0) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index cannot be negative.");
        if (pageSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

        if (!string.IsNullOrEmpty(searchTerm))
        {
            source = source.ApplySearchFilter(searchTerm);
        }
        
        int totalCount = includeTotalCount ? await source.CountAsync(cancellationToken) : -1;

        var items = await source.Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
    
    private static IQueryable<T> ApplySearchFilter<T>(this IQueryable<T> source, string search)
    {
        Expression filter = null;
        var parameter = Expression.Parameter(typeof(T), "x");
        var properties = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string)); 
        
        foreach (var property in properties)
        {
            var propertyAccess = Expression.Property(parameter, property.Name);
            var notNullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsExpression = Expression.Call(propertyAccess, containsMethod!, Expression.Constant(search));

            var propertyFilter = Expression.AndAlso(notNullCheck, containsExpression);

            filter = filter == null ? propertyFilter : Expression.OrElse(filter, propertyFilter);
        }
        
        if (filter == null)
            return source;
        
        var lambda = Expression.Lambda<Func<T, bool>>(filter, parameter);
        return source.Where(lambda);
    }
    
} 