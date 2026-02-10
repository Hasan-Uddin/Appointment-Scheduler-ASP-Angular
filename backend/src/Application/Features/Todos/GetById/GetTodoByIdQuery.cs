using Application.Abstractions.Messaging;

namespace Application.Features.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
