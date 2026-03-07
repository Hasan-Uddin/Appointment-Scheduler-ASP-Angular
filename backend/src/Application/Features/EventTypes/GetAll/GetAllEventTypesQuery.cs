using Application.Abstractions.Messaging;

namespace Application.Features.EventTypes.GetAll;

public sealed record GetAllEventTypesQuery() : IQuery<List<EventTypeResponse>>;
