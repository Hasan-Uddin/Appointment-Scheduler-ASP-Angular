using SharedKernel;

namespace Domain.Bookings;

public static class BookingErrors
{
    public static Error NotFound(Guid bookingId) => Error.NotFound(
        "Bookings.NotFound",
        $"The booking with ID '{bookingId}' was not found");

    public static readonly Error SlotNotAvailable = Error.Failure(
        "Bookings.SlotNotAvailable",
        "The selected time slot is not available");

    public static readonly Error Conflict = Error.Conflict(
        "Bookings.Conflict",
        "There is a scheduling conflict with an existing booking");

    public static readonly Error AlreadyCancelled = Error.Failure(
        "Bookings.AlreadyCancelled",
        "This booking has already been cancelled");

    public static readonly Error CannotCancelPastBooking = Error.Failure(
        "Bookings.CannotCancelPastBooking",
        "Cannot cancel a booking in the past");

    public static readonly Error Unauthorized = Error.Failure(
        "Bookings.Unauthorized",
        "You are not authorized to perform this action");

    public static readonly Error UnexpectedError = Error.Failure(
        "Bookings.UnexpectedError",
        "UnexpectedError");

    public static readonly Error DatabaseError = Error.Failure(
        "Bookings.DatabaseError",
        "Error Occured while saving in DB");

    public static readonly Error SlotCalculatorFailed = Error.Failure(
        "Bookings.SlotCalculatorFailed",
        "Error Occured while Calculating Slote");
}
