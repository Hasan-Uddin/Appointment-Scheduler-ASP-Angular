using System.Globalization;
using Application.Abstractions.Email;

namespace Infrastructure.Services.Email;

internal static class BookingConfirmationTemplate
{
    public static string GenerateGuestEmail(BookingEmailData data)
    {
        string startTime = data.StartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);
        string cancelLink = $"https://yourdomain.com/bookings/{data.BookingId}/cancel";

        return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; background-color: #f4f4f5; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 40px 20px; text-align: center; }}
        .header h1 {{ margin: 0; font-size: 24px; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #f9fafb; border-left: 4px solid #667eea; padding: 20px; margin: 20px 0; border-radius: 4px; }}
        .detail {{ margin: 10px 0; }}
        .label {{ font-weight: 600; color: #555; }}
        .button {{ display: inline-block; padding: 12px 24px; margin: 8px 4px; background: #667eea; color: white; text-decoration: none; border-radius: 6px; }}
        .button-cancel {{ background: #ef4444; }}
        .footer {{ background: #f9fafb; padding: 20px; text-align: center; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>✅ Booking Confirmed</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.GuestName},</p>
            <p>Your booking has been confirmed!</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div class='detail'><span class='label'>📅 Date & Time:</span> {startTime} ({data.TimeZone})</div>
                <div class='detail'><span class='label'>⏱️ Duration:</span> {data.DurationMinutes} minutes</div>
                <div class='detail'><span class='label'>👤 Host:</span> {data.HostName}</div>
                {(!string.IsNullOrEmpty(data.MeetingLink) ? $"<div class='detail'><span class='label'>🎥 Meeting Link:</span> <a href='{data.MeetingLink}'>{data.MeetingLink}</a></div>" : "")}
                {(!string.IsNullOrEmpty(data.Notes) ? $"<div class='detail'><span class='label'>📝 Notes:</span> {data.Notes}</div>" : "")}
            </div>
            
            <div style='text-align: center;'>
                <a href='{cancelLink}' class='button button-cancel'>Cancel Booking</a>
            </div>
        </div>
        
        <div class='footer'>
            <p>This is an automated message. Please do not reply.</p>
            <p>&copy; 2024 Booking System. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
    }

    public static string GenerateHostEmail(BookingEmailData data)
    {
        string startTime = data.StartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; overflow: hidden; }}
        .header {{ background: linear-gradient(135deg, #10b981 0%, #059669 100%); color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #f0fdf4; border-left: 4px solid #10b981; padding: 20px; margin: 20px 0; }}
        .detail {{ margin: 8px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📅 New Booking</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.HostName},</p>
            <p>You have a new booking:</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div class='detail'><strong>Guest:</strong> {data.GuestName} ({data.GuestEmail})</div>
                <div class='detail'><strong>Date & Time:</strong> {startTime}</div>
                <div class='detail'><strong>Duration:</strong> {data.DurationMinutes} minutes</div>
                {(!string.IsNullOrEmpty(data.Notes) ? $"<div class='detail'><strong>Notes:</strong> {data.Notes}</div>" : "")}
            </div>
        </div>
    </div>
</body>
</html>";
    }
}
