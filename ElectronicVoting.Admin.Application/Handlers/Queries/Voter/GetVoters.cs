using ElectronicVoting.Admin.Application.Dtos.Voter;
using ElectronicVoting.Admin.Infrastructure.Pagination;
using ElectronicVoting.Admin.Infrastructure.Repository;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Queries.Voter;

public record GetVoters : SearchQuery, IRequest<PagedResult<VoterBaseDto>>;

public class GetVotersHandler(IVoterRepository voterRepository)
    : IRequestHandler<GetVoters, PagedResult<VoterBaseDto>>
{
    public async Task<PagedResult<VoterBaseDto>> Handle(GetVoters request, CancellationToken cancellationToken)
    {
        var resultVotersPaged =
            await voterRepository.GetFilteredPagedResultAsync(request.PageIndex, request.PageSize, request.Search,cancellationToken);
        
        return MapPagedResultToDto(resultVotersPaged);
    }

    private PagedResult<VoterBaseDto> MapPagedResultToDto(PagedResult<Domain.Entities.Voter> result,
        bool includeDetails = true)
    {
        return new PagedResult<VoterBaseDto>
        {
            PageSize = result.PageSize,
            PageIndex = result.PageIndex,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(MapToVoterDto).ToList()
        };
    }

    private VoterBaseDto MapToVoterDto(Domain.Entities.Voter voter)
    {
        return new VoterBaseDto
        {
            Id = voter.Id,
            Name = voter.Name,
            Lastname = voter.Lastname,
            PersonalIdentity = voter.PersonalIdentity,
            Status = voter.IsActive,
        };
    }
}