using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Mapper;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Queries.Candidate;

public record GetCandidate(long id): IRequest<Result<CandidateDto>>
{
    public long Id { get; set; } = id;
}

public class GetCandidateHandler(ICandidateRepository candidateRepository): IRequestHandler<GetCandidate, Result<CandidateDto>>
{
    public async Task<Result<CandidateDto>> Handle(GetCandidate request, CancellationToken cancellationToken)
    {
        var result = await candidateRepository.GetByIdAsync(request.Id, cancellationToken);
        return result == null ? Result.Fail<CandidateDto>("Candidate not found") : result.ToDto();
    }
}