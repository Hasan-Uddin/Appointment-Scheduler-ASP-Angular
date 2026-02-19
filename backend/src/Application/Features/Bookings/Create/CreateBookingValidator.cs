using FluentValidation;

namespace Application.Features.Bookings.Create;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.EventTypeId)
            .NotEmpty();

        RuleFor(x => x.GuestName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.GuestEmail)
            .NotEmpty()
            .WithMessage("Guest Email cant be empty")
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.StartTime)
            .Must(BeInFuture)
            .WithMessage("Booking must be in the future");

        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }

    private bool BeInFuture(DateTime startTime)
    {
        return startTime > DateTime.UtcNow;
    }
}
