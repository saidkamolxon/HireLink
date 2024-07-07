using HireLink.Data.IRepositories;
using HireLink.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace HireLink.Data.Repositories;

public class CachedCandidateRepository(IMemoryCache memoryCache, ICandidateRepository decorated) : ICandidateRepository
{
    private readonly IMemoryCache memoryCache = memoryCache;
    private readonly ICandidateRepository decorated = decorated;
    private readonly string cacheKey = "candidate-{0}";
    private readonly TimeSpan cacheLifeTime = TimeSpan.FromMinutes(3); 

    public async Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var key = string.Format(cacheKey, email);

        if (this.memoryCache.TryGetValue(key, out Candidate? cachedCandidate))
            return cachedCandidate;

        Candidate? candidate = await this.decorated.GetByEmailAsync(email, cancellationToken);
        this.memoryCache.Set(key, candidate, cacheLifeTime);

        return candidate;
    }

    public async Task InsertAsync(Candidate candidate, CancellationToken cancellationToken = default)
    {
        await this.decorated.InsertAsync(candidate, cancellationToken);
        var key = string.Format(cacheKey, candidate.Email);
        this.memoryCache.Set(key, candidate, cacheLifeTime);
    }

    public Task SaveAsync(CancellationToken cancellationToken = default)
        => this.decorated.SaveAsync(cancellationToken);

    public void Update(Candidate candidate)
    {
        this.decorated.Update(candidate);
        var key = string.Format(cacheKey, candidate.Email);
        this.memoryCache.Set(key, candidate, cacheLifeTime);
    }
}
