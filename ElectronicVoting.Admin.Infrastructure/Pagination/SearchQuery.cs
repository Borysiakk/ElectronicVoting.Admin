namespace ElectronicVoting.Admin.Infrastructure.Pagination;

public record SearchQuery : PagedQuery
{
    public string Search { get; set; }
}