
using Domain.CalendarSettings;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configs.CalendarSettings;

public class CalendarSettingsConfiguration : IEntityTypeConfiguration<CalendarSetting>
{
    public void Configure(EntityTypeBuilder<CalendarSetting> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.TimeZone)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.MinimumNoticeMinutes)
            .IsRequired();

        builder.Property(c => c.MaximumDaysInAdvance)
            .IsRequired();

        builder.Property(c => c.SlotIntervalMinutes)
            .IsRequired();

        builder.Property(c => c.BufferBeforeMinutes)
            .HasDefaultValue(0);

        builder.Property(c => c.BufferAfterMinutes)
            .HasDefaultValue(0);

        builder.Property(c => c.DefaultStartTime)
            .IsRequired();

        builder.Property(c => c.DefaultEndTime)
            .IsRequired();

        builder.Property(c => c.RollingDaysAvailable)
            .HasDefaultValue(30);

        builder.Property(c => c.WelcomeMessage)
            .HasMaxLength(500);

        builder.Property(c => c.GoogleCalendarId)
            .HasMaxLength(255);

        builder.Property(c => c.SyncToGoogleCalendar)
            .HasDefaultValue(true);

        builder.Property(c => c.CheckGoogleCalendarConflicts)
            .HasDefaultValue(true);

        builder.Property(c => c.SendConfirmationEmail)
            .HasDefaultValue(true);

        builder.Property(c => c.SendReminderEmail)
            .HasDefaultValue(true);

        builder.Property(c => c.ReminderMinutesBefore)
            .HasDefaultValue(1440);

        builder.Property(c => c.RequireGuestPhone)
            .HasDefaultValue(false);

        builder.Property(c => c.AllowGuestNotes)
            .HasDefaultValue(true);

        builder.Property(c => c.AllowOverlapBooking)
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<CalendarSetting>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.UserId)
            .IsUnique();
    }
}
