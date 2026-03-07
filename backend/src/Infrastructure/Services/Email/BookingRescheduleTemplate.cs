using System.Globalization;
using Application.Abstractions.Email;

namespace Infrastructure.Services.Email;

internal static class BookingRescheduleTemplate
{
    public static string GenerateGuestEmail(BookingRescheduleEmailData data)
    {
        string oldTime = data.OldStartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);
        string newTime = data.StartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; }}
        .header {{ background: #8b5cf6; color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #f5f3ff; border-left: 4px solid #8b5cf6; padding: 20px; margin: 20px 0; }}
        .old-time {{ text-decoration: line-through; color: #999; }}
        .new-time {{ color: #10b981; font-weight: bold; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🔄 Booking Rescheduled</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.GuestName},</p>
            <p>Your booking has been rescheduled:</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div class='old-time'><strong>Old Time:</strong> {oldTime}</div>
                <div class='new-time'><strong>New Time:</strong> {newTime}</div>
                <div><strong>Duration:</strong> {data.DurationMinutes} minutes</div>
                <div><strong>Host:</strong> {data.HostName}</div>
            </div>
        </div>
    </div>
</body>
</html>";
    }

    public static string GenerateHostEmail(BookingRescheduleEmailData data)
    {
        string oldTime = data.OldStartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);
        string newTime = data.StartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; }}
        .header {{ background: #8b5cf6; color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #f5f3ff; border-left: 4px solid #8b5cf6; padding: 20px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Booking Rescheduled</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.HostName},</p>
            <p>A booking has been rescheduled:</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div><strong>Guest:</strong> {data.GuestName} ({data.GuestEmail})</div>
                <div><strong>Old Time:</strong> {oldTime}</div>
                <div><strong>New Time:</strong> {newTime}</div>
            </div>
        </div>
    </div>
</body>
</html>";
    }
}
