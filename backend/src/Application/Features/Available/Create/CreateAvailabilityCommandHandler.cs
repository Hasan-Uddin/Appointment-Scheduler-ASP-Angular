using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Availabilities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Available.Create;

internal sealed class CreateAvailabilityCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateAvailabilityCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateAvailabilityCommand request,
        CancellationToken cancellationToken)
    {
        // Check for overlapping availability
        bool hasOverlap = await context.Availabilities
            .AnyAsync(a =>
                a.UserId == request.UserId &&
                a.DayOfWeek == request.DayOfWeek &&
                a.IsActive &&
                request.StartTime < a.EndTime &&
                request.EndTime > a.StartTime,
                cancellationToken);

        if (hasOverlap)
        {
            return Result.Failure<Guid>(AvailabilityErrors.Overlap);
        }

        var availability = Availability.Create(
            request.UserId,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime
        );

        context.Availabilities.Add(availability);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(availability.Id);
    }
}
