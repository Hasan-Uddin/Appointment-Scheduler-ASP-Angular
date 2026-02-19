using System.Globalization;
using Application.Abstractions.Email;

namespace Infrastructure.Services.Email;

internal static class BookingCancellationTemplate
{
    public static string GenerateGuestEmail(BookingEmailData data)
    {
        string startTime = data.StartTime.ToString("dddd, MMMM dd, yyyy 'at' h:mm tt", CultureInfo.InvariantCulture);

        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; }}
        .header {{ background: #ef4444; color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #fee2e2; border-left: 4px solid #ef4444; padding: 20px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>❌ Booking Cancelled</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.GuestName},</p>
            <p>Your booking has been cancelled:</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div><strong>Date & Time:</strong> {startTime}</div>
                <div><strong>Host:</strong> {data.HostName}</div>
            </div>
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
        .container {{ max-width: 600px; margin: 20px auto; background: white; border-radius: 8px; }}
        .header {{ background: #ef4444; color: white; padding: 30px 20px; text-align: center; }}
        .content {{ padding: 30px 20px; }}
        .card {{ background: #fee2e2; border-left: 4px solid #ef4444; padding: 20px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Booking Cancelled</h1>
        </div>
        
        <div class='content'>
            <p>Hi {data.HostName},</p>
            <p>A booking has been cancelled:</p>
            
            <div class='card'>
                <h2 style='margin-top:0;'>{data.EventTypeName}</h2>
                <div><strong>Guest:</strong> {data.GuestName} ({data.GuestEmail})</div>
                <div><strong>Date & Time:</strong> {startTime}</div>
            </div>
        </div>
    </div>
</body>
</html>";
    }
}
