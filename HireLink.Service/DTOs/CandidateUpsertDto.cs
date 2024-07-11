namespace HireLink.Service.DTOs;

public record CandidateUpsertDto(
    string FirstName,
    string LastName,
    string Email,
    string Comment,
    string? PhoneNumber = default,
    string? TimeToCall = default,
    string? LinkedinUrl = default,
    string? GithubUrl = default
);