using AutoMapper;
using FluentValidation;
using HireLink.Data.IRepositories;
using HireLink.Domain.Entities;
using HireLink.Service.DTOs;
using HireLink.Service.Interfaces;
using HireLink.Service.Mappers;
using Microsoft.Extensions.Logging;
using Moq;

namespace HireLink.Service.Services;

public class CandidateServiceTests
{
    private readonly IMapper mapper;
    private readonly Mock<ILogger<CandidateService>> loggerMock;
    private readonly Mock<ICandidateRepository> repositoryMock;
    private readonly ICandidateService service;

    public CandidateServiceTests()
    {
        this.mapper = new MapperConfiguration(config => config.AddProfile<MappingProfile>()).CreateMapper();
        this.loggerMock = new Mock<ILogger<CandidateService>>();
        this.repositoryMock = new Mock<ICandidateRepository>();
        this.service = new CandidateService(this.loggerMock.Object, this.mapper, this.repositoryMock.Object);
    }

    [Fact]
    public async Task InsertAsync_ShouldReturnInsertedCandidate()
    {
        // Arrange
        var candidate = new CandidateUpsertDto
        (
            FirstName: "Saidkamol",
            LastName: "Saidjamolov",
            Email: "saidkamolxon@yahoo.com",
            Comment: "Software Engineer | .Net"
        );

        this.repositoryMock.Setup(r => r.InsertAsync(this.mapper.Map<Candidate>(candidate), It.IsAny<CancellationToken>()))
            .Callback<Candidate, CancellationToken>((candidate, token) => candidate.Id = 1)
                .Returns(Task.CompletedTask);

        // Act
        var result = await this.service.UpsertAsync(candidate);

        // Assert
        Assert.NotNull(result);

        // Verify
        this.repositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        this.repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Once);
        this.repositoryMock.Verify(r => r.Update(It.IsAny<Candidate>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedCandidate()
    {
        var email = "saidkamolxon@yahoo.com";

        // Arrange
        var existing = new Candidate
        {
            Id = 1,
            FirstName = "Some name",
            LastName = "Some last name",
            Email = email,
            Comment = "some comment"
        };

        var update = new CandidateUpsertDto
        (
            FirstName: "Saidkamol",
            LastName: "Saidjamolov",
            Email: email,
            Comment: ".NET Developer"
        );

        this.repositoryMock.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        // Act
        var result = await this.service.UpsertAsync(update);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, existing.Id);
        Assert.Equal(update.FirstName, existing.FirstName);
        Assert.Equal(update.LastName, existing.LastName);
        Assert.Equal(update.Comment, existing.Comment);

        // Verify
        this.repositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        this.repositoryMock.Verify(r => r.Update(It.IsAny<Candidate>()), Times.Once);
        this.repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpsertAsyncWithInvalidDto_ShouldThrowExceptions()
    {
        // Arrange
        var dto = new CandidateUpsertDto
        (
            FirstName: "",
            LastName: "",
            Email: "",
            Comment: ""
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(async () =>
            await this.service.UpsertAsync(dto));

        var errors = exception.Errors.ToList();

        Assert.NotEmpty(errors);
        Assert.Equal(4, errors.Count); // 1 firstname + 1 lastname + 1 email + 1 comment exceptions
        Assert.Contains("FirstName", exception.Errors.Select(e => e.PropertyName));
        Assert.Contains("LastName", exception.Errors.Select(e => e.PropertyName));
        Assert.Contains("Email", exception.Errors.Select(e => e.PropertyName));
        Assert.Contains("Comment", exception.Errors.Select(e => e.PropertyName));

        // Verify
        this.repositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        this.repositoryMock.Verify(r => r.InsertAsync(It.IsAny<Candidate>(), It.IsAny<CancellationToken>()), Times.Never);
        this.repositoryMock.Verify(r => r.Update(It.IsAny<Candidate>()), Times.Never);
    }
}
