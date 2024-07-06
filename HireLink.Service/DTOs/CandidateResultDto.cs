namespace HireLink.Service.DTOs;

public record CandidateResultDto(
    long Id,
    string FirstName,
    string LastName,
    string Email,
    string Comment,
    string? PhoneNumber = default,
    string? TimeToCall = default,
    string? LinkedinUrl = default,
    string? GithubUrl = default
);