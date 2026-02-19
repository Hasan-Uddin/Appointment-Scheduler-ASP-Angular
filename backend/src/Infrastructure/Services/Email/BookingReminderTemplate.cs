using System.Globalization;
using Application.Abstractions.Email;
using Domain.Bookings;
using Domain.EventTypes;

namespace Infrastructure.Services.Email;

internal static class BookingReminderTemplate
{
    public static string Generate(BookingEmailData data)
    {
        string startTime = data.StartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; }}
        .header {{ background: #f59e0b; color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #fef3c7; border-left: 4px solid #f59e0b; padding: 20px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>⏰ Meeting Reminder</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.GuestName},</p>
            <p>Reminder about your upcoming meeting:</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div><strong>Date & Time:</strong> {startTime}</div>
                <div><strong>Duration:</strong> {data.DurationMinutes} minutes</div>
                <div><strong>Host:</strong> {data.HostName}</div>
                {(!string.IsNullOrEmpty(data.MeetingLink) ? $"<div><strong>Meeting Link:</strong> <a href='{data.MeetingLink}'>{data.MeetingLink}</a></div>" : "")}
            </div>
        </div>
    </div>
</body>
</html>";
    }
}
