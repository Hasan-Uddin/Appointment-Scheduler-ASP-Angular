using Application.Abstractions.Messaging;

namespace Application.Features.Todos.Get;

public sealed record GetTodosQuery(Guid UserId) : IQuery<List<TodoResponse>>;
