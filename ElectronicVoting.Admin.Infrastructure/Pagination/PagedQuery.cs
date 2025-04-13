namespace ElectronicVoting.Admin.Infrastructure.Pagination;

public record PagedQuery
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}