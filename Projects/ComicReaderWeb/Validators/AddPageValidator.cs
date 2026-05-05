using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class AddPageValidator : AbstractValidator<AddPageDto>
{
    public AddPageValidator()
    {
        RuleFor(x => x.ChapterId)
            .GreaterThan(0).WithMessage("ChapterId must be greater than 0.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageUrl)
            .NotEmpty().WithMessage("Image URL is required.")
            .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.")
            .Must(url => url.StartsWith("/") || url.StartsWith("http"))
            .WithMessage("Image URL must be a valid path (starting with / or http).");
    }
}
