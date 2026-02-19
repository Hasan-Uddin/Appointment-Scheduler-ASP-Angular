using SharedKernel;

namespace Domain.EventTypes;

public static class EventTypeErrors
{
    public static Error NotFound(Guid Id) => Error.NotFound(
        "EventTypeErrors.NotFound",
        $"The eventType with the Id = '{Id}' was not found");

    public static Error NotFound() => Error.NotFound(
        "EventTypeErrors.NotFound",
        $"The user not found");

    public static Error Unauthorized() => Error.Failure(
        "EventTypeErrors.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "EventTypeErrors.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error SlotNotAvailable = Error.Failure(
        "EventTypeErrors.SlotNotAvailable",
        "Time slot is not available");

    public static readonly Error Inactive = Error.Failure(
        "EventTypeErrors.Inactive",
        "Event type is not active");

    public static readonly Error BookingConflict = Error.Conflict(
    "Users.BookingConflict",
    "Booking conflicts with existing booking");
}
