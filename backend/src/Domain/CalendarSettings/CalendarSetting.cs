using SharedKernel;

namespace Domain.CalendarSettings;

public sealed class CalendarSetting : Entity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    // General Settings
    public string TimeZone { get; private set; }
    public int MinimumNoticeMinutes { get; private set; } // How far in advance bookings must be made
    public int MaximumDaysInAdvance { get; private set; } // How far in the future bookings can be made
    public int SlotIntervalMinutes { get; private set; } // Time between available slots (15, 30, 60)

    // Buffer Times
    public int BufferBeforeMinutes { get; private set; } // Time before each meeting
    public int BufferAfterMinutes { get; private set; } // Time after each meeting

    // Daily Limits
    public int? MaxBookingsPerDay { get; private set; }
    public int? MaxBookingsPerWeek { get; private set; }

    // Working Hours (Default)
    public TimeOnly DefaultStartTime { get; private set; }
    public TimeOnly DefaultEndTime { get; private set; }

    // Date Range
    public int RollingDaysAvailable { get; private set; } // e.g., 30 days into future
    public DateTime? CustomAvailabilityStart { get; private set; }
    public DateTime? CustomAvailabilityEnd { get; private set; }

    // Google Calendar Integration
    public bool SyncToGoogleCalendar { get; private set; }
    public bool CheckGoogleCalendarConflicts { get; private set; }
    public string? GoogleCalendarId { get; private set; }

    // Notification Settings
    public bool SendConfirmationEmail { get; private set; }
    public bool SendReminderEmail { get; private set; }
    public int ReminderMinutesBefore { get; private set; }

    // Booking Page Settings
    public string? WelcomeMessage { get; private set; }
    public bool RequireGuestPhone { get; private set; }
    public bool AllowGuestNotes { get; private set; }

    // Advanced
    public bool AllowOverlapBooking { get; set; } // Allow overlapping bookings
    public int? MinimumSchedulingGap { get; private set; } // Minimum time between consecutive bookings

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    //private CalendarSettings() { } // EF Core

    public static CalendarSetting CreateDefault()
    {
        return new CalendarSetting
        {
            Id = Guid.NewGuid(),
            TimeZone = "UTC",
            MinimumNoticeMinutes = 120, // 2 hours
            MaximumDaysInAdvance = 60,
            SlotIntervalMinutes = 30,
            BufferBeforeMinutes = 0,
            BufferAfterMinutes = 0,
            DefaultStartTime = new TimeOnly(9, 0), // 9 AM
            DefaultEndTime = new TimeOnly(17, 0), // 5 PM
            RollingDaysAvailable = 30,
            SyncToGoogleCalendar = true,
            CheckGoogleCalendarConflicts = true,
            SendConfirmationEmail = true,
            SendReminderEmail = true,
            ReminderMinutesBefore = 1440, // 24 hours
            AllowGuestNotes = true,
            RequireGuestPhone = false,
            AllowOverlapBooking = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CalendarSetting Create(
        Guid userId,
        string timeZone,
        int minimumNoticeMinutes = 120,
        int maximumDaysInAdvance = 60,
        int slotIntervalMinutes = 30)
    {
        CalendarSetting settings = CreateDefault();
        settings.UserId = userId;
        settings.TimeZone = timeZone;
        settings.MinimumNoticeMinutes = minimumNoticeMinutes;
        settings.MaximumDaysInAdvance = maximumDaysInAdvance;
        settings.SlotIntervalMinutes = slotIntervalMinutes;

        return settings;
    }

    public void UpdateGeneralSettings(
        string timeZone,
        int minimumNoticeMinutes,
        int maximumDaysInAdvance,
        int slotIntervalMinutes)
    {
        TimeZone = timeZone;
        MinimumNoticeMinutes = minimumNoticeMinutes;
        MaximumDaysInAdvance = maximumDaysInAdvance;
        SlotIntervalMinutes = slotIntervalMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBufferTimes(int bufferBeforeMinutes, int bufferAfterMinutes)
    {
        BufferBeforeMinutes = bufferBeforeMinutes;
        BufferAfterMinutes = bufferAfterMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateWorkingHours(TimeOnly startTime, TimeOnly endTime)
    {
        DefaultStartTime = startTime;
        DefaultEndTime = endTime;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDailyLimits(int? maxBookingsPerDay, int? maxBookingsPerWeek)
    {
        MaxBookingsPerDay = maxBookingsPerDay;
        MaxBookingsPerWeek = maxBookingsPerWeek;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateGoogleCalendarSettings(
        bool syncToGoogleCalendar,
        bool checkConflicts,
        string? calendarId = null)
    {
        SyncToGoogleCalendar = syncToGoogleCalendar;
        CheckGoogleCalendarConflicts = checkConflicts;
        GoogleCalendarId = calendarId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNotificationSettings(
        bool sendConfirmation,
        bool sendReminder,
        int reminderMinutesBefore)
    {
        SendConfirmationEmail = sendConfirmation;
        SendReminderEmail = sendReminder;
        ReminderMinutesBefore = reminderMinutesBefore;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBookingPageSettings(
        string? welcomeMessage,
        bool requirePhone,
        bool allowNotes)
    {
        WelcomeMessage = welcomeMessage;
        RequireGuestPhone = requirePhone;
        AllowGuestNotes = allowNotes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCustomAvailabilityRange(DateTime? start, DateTime? end)
    {
        CustomAvailabilityStart = start;
        CustomAvailabilityEnd = end;
        UpdatedAt = DateTime.UtcNow;
    }

    public void EnableOverlapBooking()
    {
        AllowOverlapBooking = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DisableOverlapBooking()
    {
        AllowOverlapBooking = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetMinimumSchedulingGap(int? gapMinutes)
    {
        MinimumSchedulingGap = gapMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    // Helper methods
    public bool IsWithinBookingWindow(DateTime proposedDate)
    {
        DateTime now = DateTime.UtcNow;
        DateTime minimumDate = now.AddMinutes(MinimumNoticeMinutes);
        DateTime maximumDate = now.AddDays(MaximumDaysInAdvance);

        return proposedDate >= minimumDate && proposedDate <= maximumDate;
    }

    public bool IsWithinCustomAvailability(DateTime date)
    {
        if (!CustomAvailabilityStart.HasValue && !CustomAvailabilityEnd.HasValue)
        {
            return true;
        }

        if (CustomAvailabilityStart.HasValue && date < CustomAvailabilityStart.Value)
        {
            return false;
        }

        if (CustomAvailabilityEnd.HasValue && date > CustomAvailabilityEnd.Value)
        {
            return false;
        }

        return true;
    }

    //private bool IsValidSlotInterval(int minutes)
    //{
    //    int[] validIntervals = new[] { 5, 10, 15, 20, 30, 60 };
    //    return validIntervals.Contains(minutes);
    //}
}
