using Domain.Bookings;
using Domain.EventTypes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configs.Bookings;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.GuestName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.GuestEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.Property(b => b.Status)
            .HasConversion<string>();

        builder.HasOne<EventType>(e => e.EventType)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.EventTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>(u => u.User)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.StartTime);
        builder.HasIndex(b => b.GuestEmail);
        builder.HasIndex(b => b.EventTypeId);

        builder.HasIndex(b => new { b.UserId, b.StartTime })
               .IsUnique()
               .HasFilter("\"Status\" = 'Confirmed'");
    }
}
