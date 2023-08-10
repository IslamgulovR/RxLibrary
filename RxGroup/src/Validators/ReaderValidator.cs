using FluentValidation;
using RxGroup.Models;

namespace RxGroup.Validators;

public class ReaderValidator : AbstractValidator<Reader>
{
    public ReaderValidator()
    {
        RuleFor(reader => reader.Name).NotEmpty();
        RuleFor(reader => reader.BirthDate).NotEmpty();
    }
}