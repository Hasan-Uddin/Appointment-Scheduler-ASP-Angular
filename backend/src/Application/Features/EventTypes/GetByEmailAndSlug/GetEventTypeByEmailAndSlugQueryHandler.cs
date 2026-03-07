using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.EventTypes.GetByEmailAndSlug;

internal sealed class GetEventTypeByEmailAndSlugQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetEventTypeByEmailAndSlugQuery, EventTypeResponse>
{
    public async Task<Result<EventTypeResponse>> Handle(
        GetEventTypeByEmailAndSlugQuery query,
        CancellationToken cancellationToken)
    {
        EventTypeResponse? eventType = await context.EventTypes
            .AsNoTracking()
            .Where(e =>
                e.User.Email == query.Email &&
                e.Slug == query.Slug &&
                e.IsActive)
            .Select(e => new EventTypeResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                Name = e.Name,
                Slug = e.Slug,
                HostName = e.User.Name,
                HostEmail = e.User.Email,
                Description = e.Description,
                DurationMinutes = e.DurationMinutes,
                BufferMinutes = e.BufferMinutes,
                Color = e.Color,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (eventType is null)
        {
            return Result.Failure<EventTypeResponse>(
                Error.NotFound("EventType.NotFound", "Event type not found."));
        }

        return eventType;
    }
}
