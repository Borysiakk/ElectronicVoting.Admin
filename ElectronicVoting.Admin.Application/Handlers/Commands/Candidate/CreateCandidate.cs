using ElectronicVoting.Admin.Application.Constants;
using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Application.Mapper;
using ElectronicVoting.Admin.Infrastructure.MediatR;
using ElectronicVoting.Admin.Infrastructure.Repository;
using FluentResults;
using MediatR;

namespace ElectronicVoting.Admin.Application.Handlers.Commands.Candidate;

public record CreateCandidate : IRequest<Result<CandidateDto>>
{
    public int Age { get; set; }
    public string Party { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instagram { get; set; }
    public string Facebook { get; set; }
    public string Twitter { get; set; }
    public string Website { get; set; }
}

public class AddCandidateHandler(ICandidateRepository candidateRepository)
    : IRequestHandler<CreateCandidate, Result<CandidateDto>>, ITransaction
{
    public async Task<Result<CandidateDto>> Handle(CreateCandidate request, CancellationToken cancellationToken)
    {
        var resultAddCandidate = await candidateRepository.AddAsync(
            new Domain.Entities.Candidate(request.Name, request.Description, request.Age, request.Party,
                request.Instagram, request.Facebook, request.Twitter, request.Website), cancellationToken);

        return resultAddCandidate == null
            ? Result.Fail(Messages.Error.CandidateAdd)
               : Result.Ok(resultAddCandidate.ToDto());
    }
}