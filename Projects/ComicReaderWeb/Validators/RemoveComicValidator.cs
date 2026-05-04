using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class RemoveComicValidator : AbstractValidator<RemoveComicDto>
{
    public RemoveComicValidator()
    {
        RuleFor(x => x.ComicId)
            .GreaterThan(0).WithMessage("ComicId must be greater than 0.");
    }
}
