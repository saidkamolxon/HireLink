using FluentValidation.TestHelper;
using HireLink.Service.DTOs;

namespace HireLink.Service.Validators;

public class CandidateUpsertDtoValidatorTests
{
    private readonly CandidateUpsertDtoValidator validator;

    public CandidateUpsertDtoValidatorTests()
    {
        this.validator = new CandidateUpsertDtoValidator();
    }

    [Theory]
    [InlineData("", "", "", "")] // Empty firstName & lastName & description & email
    [InlineData("Saidkamol", "", "", "")]    // Empty lastName & description & email
    [InlineData("Saidkamol", "Saidjamolov", "", "")]    // Empty description & email
    [InlineData("Saidkamol", "Saidjamolov", "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "")] // Empty email
    [InlineData("Saidkamol", "Saidjamolov", "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", "saidkamolxon")] // Invalid email
    public void ShouldReturnFalse_IfInvalidCandidate(string firstName, string lastName, string comment, string email)
    {
        // Arrange
        var candidate = new CandidateUpsertDto
        (
            FirstName: firstName,
            LastName: lastName,
            Email: email,
            Comment: comment
        );

        // Act
        var result = this.validator.Validate(candidate).IsValid;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShouldReturnTrue_IfValidCandidate()
    {
        // Arrange
        var candidate = new CandidateUpsertDto
        (
            FirstName: "Saidkamol",
            LastName: "Saidjamolov",
            Email: "saidkamolxon@yahoo.com",
            Comment: "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
        );

        // Act
        var result = this.validator.TestValidate(candidate);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
