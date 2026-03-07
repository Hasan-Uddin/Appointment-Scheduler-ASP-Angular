using Application.Abstractions.Messaging;

namespace Application.Features.Available.Get;

public sealed record GetAvailabilitiesQuery()
    : IQuery<List<AvailabilityResponse>>;
