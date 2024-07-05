namespace HireLink.Domain.Entities;

public class Candidate : Auditable
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public string? TimeToCall { get; set; }
    public string? LinkedinUrl { get; set; }
    public string? GithubUrl { get; set; }
    public required string Comment { get; set; }
}
