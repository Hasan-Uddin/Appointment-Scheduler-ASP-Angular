using Application.Abstractions.Messaging;

namespace Application.Features.Todos.Delete;

public sealed record DeleteTodoCommand(Guid TodoItemId) : ICommand;
