namespace ElectronicVoting.Admin.Infrastructure.Pagination;

public record PagedResult<T>
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Items { get; set; } = [];
}