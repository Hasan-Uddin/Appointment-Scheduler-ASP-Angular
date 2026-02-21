using SharedKernel;

namespace Domain.Availabilities;

public static class AvailabilityErrors
{
    public static Error NotFound(Guid availabilityId) => Error.NotFound(
        "Availability.NotFound",
        $"The availability rule with ID '{availabilityId}' was not found");

    public static readonly Error InvalidTimeRange = Error.Validation(
        "Availability.InvalidTimeRange",
        "Start time must be before end time");

    public static readonly Error Overlap = Error.Conflict(
        "Availability.Overlap",
        "This availability overlaps with an existing rule");

    public static readonly Error Unauthorized = Error.Forbidden(
        "Availability.Unauthorized",
        "You are not authorized to manage this availability rule");
}
