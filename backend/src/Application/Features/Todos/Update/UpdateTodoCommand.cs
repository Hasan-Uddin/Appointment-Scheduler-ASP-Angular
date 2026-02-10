using Application.Abstractions.Messaging;

namespace Application.Features.Todos.Update;

public sealed record UpdateTodoCommand(
    Guid TodoItemId,
    string Description) : ICommand;
