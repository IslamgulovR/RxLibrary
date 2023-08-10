using FluentValidation;
using RxGroup.Features.Dto.Issues;

namespace RxGroup.Validators;

public class IssueBookValidator : AbstractValidator<IssueBookDto>
{
    public IssueBookValidator()
    {
        RuleFor(issue => issue.Quantity).GreaterThan(0);
    }
}