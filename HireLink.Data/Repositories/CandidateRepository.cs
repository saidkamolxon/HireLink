using HireLink.Data.Contexts;
using HireLink.Data.IRepositories;
using HireLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireLink.Data.Repositories;

public class CandidateRepository(AppDbContext context) : ICandidateRepository
{
    private readonly AppDbContext context = context;

    public async Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await this.context.Candidates.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);

    public async Task InsertAsync(Candidate candidate, CancellationToken cancellationToken = default)
        => await this.context.Candidates.AddAsync(candidate, cancellationToken);

    public async Task SaveAsync(CancellationToken cancellationToken = default)
        => await this.context.SaveChangesAsync(cancellationToken);

    public void Update(Candidate candidate)
    {
        candidate.UpdatedAt = DateTime.UtcNow;
        this.context.Entry(candidate).State = EntityState.Modified;
    }
}
