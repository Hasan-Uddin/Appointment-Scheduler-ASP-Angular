
using Application.Abstractions.Messaging;

namespace Application.Features.Bookings.Create;

public sealed record class CreateBookingCommand(
    Guid EventTypeId,
    string GuestName,
    string GuestEmail,
    DateTime StartTime,
    string? GuestPhone,
    string? Notes
) : ICommand<Guid>;
