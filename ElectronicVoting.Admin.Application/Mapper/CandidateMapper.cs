using ElectronicVoting.Admin.Application.Dtos;
using ElectronicVoting.Admin.Domain.Entities;

namespace ElectronicVoting.Admin.Application.Mapper;

public static class CandidateMapper
{
    public static CandidateDto ToDto(this Candidate candidate)
    {
        return new CandidateDto
        {
            Id = candidate.Id,
            Name = candidate.Name,
            Description = candidate.Description,
            Age = candidate.Age,
            Party = candidate.Party,
            Instagram = candidate.Instagram,
            Facebook = candidate.Facebook,
            Twitter = candidate.Twitter,
            Website = candidate.Website
        };
    }
}