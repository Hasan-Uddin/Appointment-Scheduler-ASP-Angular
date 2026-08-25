using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.EventTypes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.EventTypes.Create;

public sealed class CreateEventTypeCommandHandler(
    IUserContext userContext,
    IApplicationDbContext context
) : ICommandHandler<CreateEventTypeCommand, Guid>
{
    async Task<Result<Guid>> ICommandHandler<CreateEventTypeCommand, Guid>.Handle(CreateEventTypeCommand command, CancellationToken cancellationToken)
    {
        User? user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userContext.UserId, cancellationToken);

        if (user is null || userContext.UserId is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound());
        }

        // ensure slug is unique per user
        EventType? existingEventType = await context.EventTypes.AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userContext.UserId && e.Slug == command.Slug, cancellationToken);

        if (existingEventType != null)
        {
            throw new InvalidOperationException($"Slug '{command.Slug}' is already in use for this user.");
        }

        var eventType = EventType.Create(
            (Guid)userContext.UserId,
            command.Name,
            command.Slug,
            command.DurationMinutes,
            command.BufferMinutes,
            command.Description,
            command.Color
        );

        await context.EventTypes.AddAsync(eventType, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return eventType.Id;
    }
}
