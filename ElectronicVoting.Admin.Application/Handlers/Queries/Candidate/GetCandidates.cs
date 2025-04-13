using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Mapper;
using ElectronicVoting.Admin.Infrastructure.Pagination;
using ElectronicVoting.Admin.Infrastructure.Repository;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Queries.Candidate;

public record GetCandidates : SearchQuery, IRequest<PagedResult<CandidateDto>>
{
}

public class GetCandidatesHandler(ICandidateRepository candidateRepository)
    : IRequestHandler<GetCandidates, PagedResult<CandidateDto>>
{
    public async Task<PagedResult<CandidateDto>> Handle(GetCandidates request, CancellationToken cancellationToken)
    {
        var resultCandidatePaged = await candidateRepository.GetFilteredPagedResultAsync(request.PageIndex, request.PageSize, request.Search, cancellationToken);
        
        return MapPagedResultToDto(resultCandidatePaged);
    }

    private PagedResult<CandidateDto> MapPagedResultToDto(PagedResult<Domain.Entities.Candidate> candidatePaged)
    {
        return new PagedResult<CandidateDto>()
        {
            PageSize = candidatePaged.PageSize,
            PageIndex = candidatePaged.PageIndex,
            TotalCount = candidatePaged.TotalCount,
            Items = candidatePaged.Items.Select(a => a.ToDto())
        };
    }
}