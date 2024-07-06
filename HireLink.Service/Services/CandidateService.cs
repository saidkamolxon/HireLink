using AutoMapper;
using FluentValidation;
using HireLink.Data.IRepositories;
using HireLink.Domain.Entities;
using HireLink.Service.DTOs;
using HireLink.Service.Interfaces;
using HireLink.Service.Validators;
using Microsoft.Extensions.Logging;

namespace HireLink.Service.Services;

public class CandidateService : ICandidateService
{
    private readonly ILogger<CandidateService> logger;
    private readonly IMapper mapper;
    private readonly ICandidateRepository repository;
    private readonly CandidateUpsertDtoValidator validator;

    public CandidateService(ILogger<CandidateService> logger, IMapper mapper, ICandidateRepository repository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.repository = repository;
        this.validator = new CandidateUpsertDtoValidator();
    }

    public async Task<CandidateResultDto> UpsertAsync(CandidateUpsertDto dto, CancellationToken cancellationToken = default)
    {
        await this.validator.ValidateAndThrowAsync(dto, cancellationToken);

        var candidate = await this.repository.GetByEmailAsync(dto.Email, cancellationToken);
        
        if (candidate is not null)
        { 
            return await this.UpdateAsync(dto, candidate, cancellationToken);
        }
        return await this.InsertAsync(dto, cancellationToken);
    }

    private async Task<CandidateResultDto> InsertAsync(CandidateUpsertDto dto, CancellationToken cancellationToken)
    {
        var newCandidate = this.mapper.Map<Candidate>(dto);
        await this.repository.InsertAsync(newCandidate, cancellationToken);
        await this.repository.SaveAsync(cancellationToken);
        this.logger.LogInformation("New candidate with email '{email}' has been successfully added.", dto.Email);
        return this.mapper.Map<CandidateResultDto>(newCandidate);
    }

    private async Task<CandidateResultDto> UpdateAsync(CandidateUpsertDto dto, Candidate candidate, CancellationToken cancellationToken)
    {
        this.mapper.Map(dto, candidate);
        this.repository.Update(candidate);
        await this.repository.SaveAsync(cancellationToken);
        this.logger.LogInformation("The candidate with email '{email}' has been successfully updated.", dto.Email);
        return this.mapper.Map<CandidateResultDto>(candidate);
    }
}
