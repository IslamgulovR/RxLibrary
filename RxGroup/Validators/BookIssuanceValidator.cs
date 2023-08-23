using FluentValidation;
using RxGroup.Models;

namespace RxGroup.Validators;

public class BookIssuanceValidator : AbstractValidator<BookIssuance>
{
    public BookIssuanceValidator()
    {
        RuleFor(issuance => issuance.ReturnDate).GreaterThan(issuance => issuance.IssueDate);
    }
}