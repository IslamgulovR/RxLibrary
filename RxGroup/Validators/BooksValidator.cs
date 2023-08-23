using FluentValidation;
using RxGroup.Models;

namespace RxGroup.Validators;

public class BooksValidator : AbstractValidator<Book>
{
    public BooksValidator()
    {
        RuleFor(book => book.VendorCode).NotEmpty();
    }
}