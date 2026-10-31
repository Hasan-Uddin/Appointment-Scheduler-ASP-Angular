using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Available.Get;
using Domain.Availabilities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Features.Available.Create;

internal sealed class CreateAvailabilityCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateAvailabilityCommand, AvailabilityResponse>
{
    public async Task<Result<AvailabilityResponse>> Handle(
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
            return Result.Failure<AvailabilityResponse>(AvailabilityErrors.Overlap);
        }

        var availability = Availability.Create(
            request.UserId,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime
        );

        context.Availabilities.Add(availability);
        await context.SaveChangesAsync(cancellationToken);

        var response = new AvailabilityResponse
        {
            Id = availability.Id,
            UserId = availability.UserId,
            DayOfWeek = availability.DayOfWeek,
            StartTime = availability.StartTime,
            EndTime = availability.EndTime,
            IsActive = availability.IsActive,
            CreatedAt = availability.CreatedAt
        };

        return Result.Success(response);
    }
}
