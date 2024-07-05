using HireLink.Domain.Entities;

namespace HireLink.Data.IRepositories;

public interface ICandidateRepository
{
    Task InsertAsync(Candidate candidate, CancellationToken cancellationToken = default);
    void Update(Candidate candidate);
    Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
