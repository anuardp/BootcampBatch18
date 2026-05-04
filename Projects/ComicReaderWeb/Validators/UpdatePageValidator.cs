using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class UpdatePageValidator : AbstractValidator<UpdatePageDto>
{
    public UpdatePageValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Page Id must be greater than 0.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image URL is required.")
            .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.")
            .Must(url => url.StartsWith("/") || url.StartsWith("http"))
            .WithMessage("Image URL must be a valid path (starting with / or http).");
    }
}