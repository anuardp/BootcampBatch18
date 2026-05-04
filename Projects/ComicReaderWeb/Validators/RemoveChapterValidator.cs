using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class RemoveChapterValidator : AbstractValidator<RemoveChapterDto>
{
    public RemoveChapterValidator()
    {
        RuleFor(x => x.ChapterId)
            .GreaterThan(0).WithMessage("ChapterId must be greater than 0.");
    }
}