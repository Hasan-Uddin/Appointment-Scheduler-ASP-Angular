
namespace Application.Abstractions.Interfaces;

public interface IGoogleCalendarService
{
    // CREATE EVENT
    Task<string> CreateEventAsync(
        string refreshToken,
        GoogleCalendarEventRequest request,
        CancellationToken cancellationToken = default);

    // UPDATE EVENT
    Task<bool> UpdateEventAsync(
        string refreshToken,
        string eventId,
        GoogleCalendarEventRequest request,
        CancellationToken cancellationToken = default);

    // DELETE EVENT
    Task<bool> DeleteEventAsync(
        string refreshToken,
        string eventId,
        CancellationToken cancellationToken = default);

    // GET SINGLE EVENT
    Task<GoogleCalendarEvent?> GetEventAsync(
        string refreshToken,
        string eventId,
        CancellationToken cancellationToken = default);

    // GET EVENTS RANGE
    Task<List<GoogleCalendarEvent>> GetEventsAsync(
        string refreshToken,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    // CONFLICT CHECK
    Task<bool> HasConflictAsync(
        string refreshToken,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken = default);
}

public sealed class GoogleCalendarEventRequest
{
    public string Summary { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DateTime StartTime { get; init; }

    public DateTime EndTime { get; init; }

    public List<string>? Attendees { get; init; }
}

public class GoogleCalendarEvent
{
    public string Id { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public string? MeetLink { get; set; }
    public List<string> Attendees { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}

//public class GoogleCalendarEventsResponse
//{
//    public string Kind { get; set; } = string.Empty;
//    public string Etag { get; set; } = string.Empty;
//    public string Summary { get; set; } = string.Empty;
//    public string? Description { get; set; }
//    public string TimeZone { get; set; } = string.Empty;
//    public string? NextPageToken { get; set; }
//    public List<GoogleCalendarEventResponse> Items { get; set; } = new();
//}

//public class GoogleCalendarEventResponse
//{
//    public string Id { get; set; } = string.Empty;
//    public string Status { get; set; } = string.Empty;
//    public string HtmlLink { get; set; } = string.Empty;
//    public string Summary { get; set; } = string.Empty;
//    public string? Description { get; set; }
//    public GoogleCalendarDateTime Start { get; set; } = new();
//    public GoogleCalendarDateTime End { get; set; } = new();
//    public List<GoogleCalendarAttendee>? Attendees { get; set; }
//    public string? Location { get; set; }
//    public GoogleCalendarConferenceData? ConferenceData { get; set; }
//    public GoogleCalendarReminders? Reminders { get; set; }
//}


//public class GoogleCalendarDateTime
//{
//    public string DateTime { get; set; } = string.Empty;
//    public string TimeZone { get; set; } = string.Empty;
//}

//public class GoogleCalendarAttendee
//{
//    public string Email { get; set; } = string.Empty;
//    public string? DisplayName { get; set; }
//    public string? ResponseStatus { get; set; }
//    public bool? Organizer { get; set; }
//    public bool? Optional { get; set; }
//}

//public class GoogleCalendarConferenceData
//{
//    public string? ConferenceId { get; set; }
//    public GoogleCalendarEntryPoint[]? EntryPoints { get; set; }
//}

//public class GoogleCalendarEntryPoint
//{
//    public string EntryPointType { get; set; } = string.Empty;
//    public string Uri { get; set; } = string.Empty;
//    public string? Label { get; set; }
//}

//public class GoogleCalendarReminders
//{
//    public bool UseDefault { get; set; }
//    public List<GoogleCalendarReminderOverride>? Overrides { get; set; }
//}

//public class GoogleCalendarReminderOverride
//{
//    public string Method { get; set; } = string.Empty; // "email" or "popup"
//    public int Minutes { get; set; }
//}




//public class GoogleCalendarEventRequest
//{
//    private const bool V = false;

//    public string Summary { get; set; } = string.Empty;
//    public string? Description { get; set; }
//    public DateTime StartTime { get; set; }
//    public DateTime EndTime { get; set; }
//    public string TimeZone { get; set; } = "UTC";
//    public string[] Attendees { get; set; } = Array.Empty<string>();
//    public string? Location { get; set; }
//    public bool SendNotifications { get; set; } = true;
//    public bool CreateMeetLink { get; set; } = V;
//    public ReminderSetting[]? Reminders { get; set; }
//}

//public class ReminderSetting
//{
//    public string Method { get; set; } = string.Empty; // "email" or "popup"
//    public int Minutes { get; set; }
//}
