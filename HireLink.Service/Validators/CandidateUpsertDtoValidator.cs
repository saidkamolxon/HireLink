using FluentValidation;
using HireLink.Service.DTOs;

namespace HireLink.Service.Validators;

public class CandidateUpsertDtoValidator : AbstractValidator<CandidateUpsertDto>
{
    public CandidateUpsertDtoValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty();

        RuleFor(c => c.LastName)
            .NotEmpty();

        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.Comment)
            .NotEmpty();
    }
}
