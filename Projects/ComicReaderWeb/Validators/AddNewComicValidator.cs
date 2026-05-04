using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class AddNewComicValidator : AbstractValidator<AddNewComicDto>
{
    public AddNewComicValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.");

        RuleFor(x => x.Publisher)
            .MaximumLength(100).WithMessage("Publisher cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Publisher));

        RuleFor(x => x.Author)
            .MaximumLength(200).WithMessage("Author cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Author));

        RuleFor(x => x.Genre)
            .MaximumLength(100).WithMessage("Genre cannot exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Genre));

        RuleFor(x => x.YearReleased)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage($"Year released must be between 1900 and {DateTime.UtcNow.Year + 1}.")
            .When(x => x.YearReleased.HasValue);
    }
}