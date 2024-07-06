using AutoMapper;
using HireLink.Domain.Entities;
using HireLink.Service.DTOs;

namespace HireLink.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CandidateUpsertDto, Candidate>();
        CreateMap<Candidate, CandidateResultDto>();
    }
}
