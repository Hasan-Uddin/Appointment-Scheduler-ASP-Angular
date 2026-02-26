
using Application.Abstractions.Messaging;

namespace Application.Features.EventTypes.GetByEmailAndSlug;

public sealed record GetEventTypeByEmailAndSlugQuery(
    string Email,
    string Slug
) : IQuery<EventTypeResponse>;
