using HireLink.Service.DTOs;

namespace HireLink.Service.Interfaces;

public interface ICandidateService
{
    Task<CandidateResultDto> UpsertAsync(CandidateUpsertDto dto, CancellationToken cancellationToken = default);
}
