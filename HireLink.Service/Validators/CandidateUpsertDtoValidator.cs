using FluentValidation;
using HireLink.Service.DTOs;

namespace HireLink.Service.Validators;

public class CandidateUpsertDtoValidator : AbstractValidator<CandidateUpsertDto>
{
    public CandidateUpsertDtoValidator()
    {
        RuleFor(c => c.FirstName)
            .Length(3, 50);

        RuleFor(c => c.LastName)
            .Length(3, 50);

        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.Comment)
            .Length(10, 1000);
    }
}
