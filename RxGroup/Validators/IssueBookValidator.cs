using FluentValidation;
using RxGroup.Features.BooksIssuance.Dto;

namespace RxGroup.Validators;

public class IssueBookValidator : AbstractValidator<IssueBookDto>
{
    public IssueBookValidator()
    {
        RuleFor(issue => issue.Quantity).GreaterThan(0);
    }
}