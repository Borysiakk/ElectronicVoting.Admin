using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Dtos.Election;
using ElectronicVoting.Admin.Infrastructure.Pagination;
using ElectronicVoting.Admin.Infrastructure.Repository;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Queries.Election;

public record GetElections : SearchQuery, IRequest<PagedResult<ElectionDto>>;

public class GetElectionsHandler(IElectionRepository electionRepository)
    : IRequestHandler<GetElections, PagedResult<ElectionDto>>
{
    public async Task<PagedResult<ElectionDto>> Handle(GetElections request, CancellationToken cancellationToken)
    {
        var pagedElectionsResult =
            await electionRepository.GetFilteredPagedResultAsync(request.PageIndex, request.PageSize, request.Search, cancellationToken);
        return MapPagedResultToDto(pagedElectionsResult);
    }

    private static PagedResult<ElectionDto> MapPagedResultToDto(PagedResult<Domain.Entities.Election> source)
    {
        return new PagedResult<ElectionDto>
        {
            PageIndex = source.PageIndex,
            PageSize = source.PageSize,
            TotalCount = source.TotalCount,
            Items = source.Items.Select(MapElectionToDto)
        };
    }

    private static ElectionDto MapElectionToDto(Domain.Entities.Election election)
    {
        return new ElectionDto
        {
            Id = election.Id,
            Name = election.Name,
            IsActive = election.IsActive,
            StartDate = election.StartDate,
            EndDate = election.EndDate
        };
    }
}