using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.EventTypes.GetAll;

internal sealed class GetAllEventTypesQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetAllEventTypesQuery, List<EventTypeResponse>>
{
    public async Task<Result<List<EventTypeResponse>>> Handle(
        GetAllEventTypesQuery query,
        CancellationToken cancellationToken)
    {
        List<EventTypeResponse> eventTypes = await context.EventTypes
            .AsNoTracking()
            .Where(e => e.UserId == userContext.UserId)
            .Select(e => new EventTypeResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                Name = e.Name,
                Slug = e.Slug,
                Description = e.Description,
                DurationMinutes = e.DurationMinutes,
                BufferMinutes = e.BufferMinutes,
                IsActive = e.IsActive,
                Color = e.Color,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return eventTypes;
    }
}
