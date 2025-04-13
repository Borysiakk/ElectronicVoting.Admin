using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Infrastructure.Pagination;
using ElectronicVoting.Admin.Infrastructure.Repository;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Queries.Approver;

public record GetApprovers : PagedQuery, IRequest<PagedResult<ApproverDto>>;

public class GetApproversHandler(IApproverRepository approverRepository) : IRequestHandler<GetApprovers, PagedResult<ApproverDto>>
{
    public async Task<PagedResult<ApproverDto>> Handle(GetApprovers request, CancellationToken cancellationToken)
    {
        var resultApproversPaged =  await approverRepository.GetPagedAsync(request.PageIndex, request.PageSize, cancellationToken);

        return MapPagedResultToDto(resultApproversPaged);
    }
    
    private PagedResult<ApproverDto> MapPagedResultToDto(PagedResult<Domain.Entities.Approver> result)
    {
        return new PagedResult<ApproverDto>()
        {
            PageSize = result.PageSize,
            PageIndex = result.PageIndex,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(MapToVoterDto).ToList()
        };
    }

    private ApproverDto MapToVoterDto(Domain.Entities.Approver approver)
    {
        return new ApproverDto()
        {
            Name = approver.Name,
            Host = approver.Host,
            Description = approver.Description,
        };
    }
}