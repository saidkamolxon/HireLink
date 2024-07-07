namespace HireLink.Service.DTOs;

public record CandidateResultDto(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Comment,
    string? Phone = default,
    string? CallTimeInterval = default,
    string? LinkedinUrl = default,
    string? GithubUrl = default
);