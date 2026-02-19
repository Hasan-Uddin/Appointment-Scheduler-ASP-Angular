using Domain.Availabilities;
using Domain.Bookings;
using Domain.EventTypes;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Availability> Availabilities { get; }
    DbSet<EventType> EventTypes { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
