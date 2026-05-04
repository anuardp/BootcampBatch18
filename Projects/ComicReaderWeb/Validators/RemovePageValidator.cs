using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class RemovePageValidator : AbstractValidator<RemovePageDto>
{
    public RemovePageValidator()
    {
        RuleFor(x => x.PageId)
            .GreaterThan(0).WithMessage("PageId must be greater than 0.");
    }
}