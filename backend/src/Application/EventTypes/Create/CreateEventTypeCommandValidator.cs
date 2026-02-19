using FluentValidation;

namespace Application.EventTypes.Create;

public sealed class CreateEventTypeCommandValidator : AbstractValidator<CreateEventTypeCommand>
{
    public CreateEventTypeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-z0-9\\-]+$") // lowercase, numbers, hyphens
            .WithMessage("Slug can contain only lowercase letters, numbers and hyphens");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0)
            .WithMessage("Duration must be greater than 0");

        RuleFor(x => x.BufferMinutes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Color)
            .MaximumLength(20);
    }
}
