using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Available.Get;

internal sealed class GetAvailabilitiesQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetAvailabilitiesQuery, List<AvailabilityResponse>>
{
    public async Task<Result<List<AvailabilityResponse>>> Handle(
        GetAvailabilitiesQuery query,
        CancellationToken cancellationToken)
    {
        Guid UserId = userContext.UserId ?? Guid.Empty;

        List<AvailabilityResponse> availabilities = await context.Availabilities
            .Where(a => a.UserId == UserId && a.IsActive)
            .Select(a => new AvailabilityResponse
            {
                Id = a.Id,
                UserId = a.UserId,
                DayOfWeek = a.DayOfWeek,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return availabilities;
    }
}
