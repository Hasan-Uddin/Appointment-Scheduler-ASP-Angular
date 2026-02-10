using Application.Abstractions.Messaging;

namespace Application.Features.Todos.Complete;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;
