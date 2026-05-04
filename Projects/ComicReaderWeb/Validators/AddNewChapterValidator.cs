using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;

public class AddNewChapterValidator : AbstractValidator<AddNewChapterDto>
{
    public AddNewChapterValidator()
    {
        RuleFor(x => x.ComicId)
            .GreaterThan(0).WithMessage("ComicId must be greater than 0.");

        RuleFor(x => x.ChapterNumber)
            .GreaterThan(0).WithMessage("Chapter number must be greater than 0.");
    }
}