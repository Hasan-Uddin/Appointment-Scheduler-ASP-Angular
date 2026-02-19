using System.Globalization;
using Application.Abstractions.Authentication;
using Application.Abstractions.Interfaces;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace Infrastructure.Services.GoogleCalendar;

public sealed class GoogleCalendarService(IGoogleAuthService authService) : IGoogleCalendarService
{

    private async Task<CalendarService> CreateServiceAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        Google.Apis.Auth.OAuth2.UserCredential credential =
            await authService.CreateUserCredentialAsync(
                refreshToken,
                cancellationToken);

        var service =
            new CalendarService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "YourAppName"
                });

        return service;
    }

    // CREATE EVENT
    public async Task<string> CreateEventAsync(
        string refreshToken,
        GoogleCalendarEventRequest request,
        CancellationToken cancellationToken = default)
    {
        using CalendarService service = await CreateServiceAsync(refreshToken, cancellationToken);

        var newEvent = new Event
        {
            Summary = request.Summary,
            Description = request.Description,
            Start = new EventDateTime
            {
                DateTimeDateTimeOffset = request.StartTime.ToUniversalTime(),
                TimeZone = "UTC"
            },
            End = new EventDateTime
            {
                DateTimeDateTimeOffset = request.EndTime.ToUniversalTime(),
                TimeZone = "UTC"
            },
            Attendees = request.Attendees?
                .Select(email => new EventAttendee { Email = email })
                .ToList()
        };

        EventsResource.InsertRequest insertRequest =
            service.Events.Insert(newEvent, "primary");

        Event createdEvent =
            await insertRequest.ExecuteAsync(cancellationToken);

        return createdEvent.Id;
    }

    // UPDATE EVENT
    public async Task<bool> UpdateEventAsync(
        string refreshToken,
        string eventId,
        GoogleCalendarEventRequest request,
        CancellationToken cancellationToken = default)
    {
        using CalendarService service = await CreateServiceAsync(refreshToken, cancellationToken);

        Event existingEvent =
            await service.Events.Get("primary", eventId)
                .ExecuteAsync(cancellationToken);

        existingEvent.Summary = request.Summary;
        existingEvent.Description = request.Description;
        existingEvent.Start = new EventDateTime
        {
            DateTimeDateTimeOffset = request.StartTime.ToUniversalTime(),
            TimeZone = "UTC"
        };
        existingEvent.End = new EventDateTime
        {
            DateTimeDateTimeOffset = request.EndTime.ToUniversalTime(),
            TimeZone = "UTC"
        };

        EventsResource.UpdateRequest updateRequest =
            service.Events.Update(existingEvent, "primary", eventId);

        await updateRequest.ExecuteAsync(cancellationToken);

        return true;
    }

    // DELETE EVENT
    public async Task<bool> DeleteEventAsync(
        string refreshToken,
        string eventId,
        CancellationToken cancellationToken = default)
    {
        using CalendarService service = await CreateServiceAsync(refreshToken, cancellationToken);

        EventsResource.DeleteRequest deleteRequest =
            service.Events.Delete("primary", eventId);

        await deleteRequest.ExecuteAsync(cancellationToken);

        return true;
    }

    // GET SINGLE EVENT
    public async Task<GoogleCalendarEvent?> GetEventAsync(
        string refreshToken,
        string eventId,
        CancellationToken cancellationToken = default)
    {
        using CalendarService service = await CreateServiceAsync(refreshToken, cancellationToken);

        Event googleEvent =
            await service.Events.Get("primary", eventId)
                .ExecuteAsync(cancellationToken);

        if (googleEvent == null)
        {
            return null;
        }

        return new GoogleCalendarEvent
        {
            Id = googleEvent.Id,
            Summary = googleEvent.Summary,
            StartTime = ResolveGoogleDate(googleEvent.Start),
            EndTime = ResolveGoogleDate(googleEvent.End)
        };
    }

    // GET EVENTS RANGE
    public async Task<List<GoogleCalendarEvent>> GetEventsAsync(
        string refreshToken,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        using CalendarService service = await CreateServiceAsync(refreshToken, cancellationToken);

        EventsResource.ListRequest listRequest =
            service.Events.List("primary");

        listRequest.TimeMinDateTimeOffset = new DateTimeOffset(startDate.ToUniversalTime());

        listRequest.TimeMinDateTimeOffset = new DateTimeOffset(endDate.ToUniversalTime());

        listRequest.SingleEvents = true;
        listRequest.OrderBy =
            EventsResource.ListRequest.OrderByEnum.StartTime;

        Events events =
            await listRequest.ExecuteAsync(cancellationToken);

        if (events.Items == null)
        {
            return new List<GoogleCalendarEvent>();
        }

        var result = new List<GoogleCalendarEvent>();

        foreach (Event item in events.Items)
        {
            result.Add(new GoogleCalendarEvent
            {
                Id = item.Id,
                Summary = item.Summary,
                StartTime = ResolveGoogleDate(item.Start),
                EndTime = ResolveGoogleDate(item.End)
            });
        }

        return result;
    }

    // CONFLICT CHECK
    public async Task<bool> HasConflictAsync(
        string refreshToken,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken = default)
    {
        using CalendarService service = await CreateServiceAsync(refreshToken, cancellationToken);

        var freeBusyRequest =
            new FreeBusyRequest
            {
                TimeMinDateTimeOffset = startTime.ToUniversalTime(),
                TimeMaxDateTimeOffset = endTime.ToUniversalTime(),
                Items = new List<FreeBusyRequestItem>
                {
                    new FreeBusyRequestItem { Id = "primary" }
                }
            };

        FreeBusyResponse response =
            await service.Freebusy.Query(freeBusyRequest)
                .ExecuteAsync(cancellationToken);

        return response.Calendars["primary"].Busy.Any();
    }


    // PRIVATE DATE RESOLVER
    private static DateTime ResolveGoogleDate(EventDateTime dateTime)
    {
        if (dateTime.DateTimeDateTimeOffset.HasValue)
        {
            return dateTime.DateTimeDateTimeOffset.Value.UtcDateTime;
        }

        return DateTime.ParseExact(
            dateTime.Date,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }
}
