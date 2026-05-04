using FluentValidation;
using ComicReader.DTOs;

namespace ComicReader.Validators;
public class SubscribeToPremiumValidator : AbstractValidator<SubscribeToPremiumDto>
{
    public SubscribeToPremiumValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("CustomerId must be greater than 0.");

        RuleFor(x => x.DurationDays)
            .InclusiveBetween(1, 365).WithMessage("Duration must be between 1 and 365 days.");

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(50).WithMessage("Payment method cannot exceed 50 characters.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Payment method should contain only letters and spaces.")
            .When(x => !string.IsNullOrEmpty(x.PaymentMethod));
    }
}