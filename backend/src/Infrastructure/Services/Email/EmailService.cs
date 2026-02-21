using Application.Abstractions.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Infrastructure.Services.Email;

internal sealed class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IConfiguration configuration,
        ILogger<EmailService> logger)
    {
        _logger = logger;

        _settings = new EmailSettings
        {
            SmtpServer = configuration["EmailSettings:SmtpServer"]!,
            SmtpPort = configuration.GetValue<int>("EmailSettings:SmtpPort"),
            Username = configuration["EmailSettings:Username"]!,
            Password = configuration["EmailSettings:Password"]!,
            FromEmail = configuration["EmailSettings:FromEmail"]!,
            FromName = configuration["EmailSettings:FromName"]!
        };
    }

    public async Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        using MimeMessage mimeMessage = CreateMimeMessage(message);
        await SendMimeMessageAsync(mimeMessage, cancellationToken);
    }

    public async Task SendBookingConfirmationAsync(
        BookingEmailData bookingData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Guest email
            await SendAsync(new EmailMessage
            {
                To = bookingData.GuestEmail,
                Subject = $"✓ Booking Confirmed: {bookingData.EventTypeName}",
                Body = BookingConfirmationTemplate.GenerateGuestEmail(bookingData)
            }, cancellationToken);

            // Host email
            await SendAsync(new EmailMessage
            {
                To = bookingData.HostEmail,
                Subject = $"📅 New Booking: {bookingData.EventTypeName}",
                Body = BookingConfirmationTemplate.GenerateHostEmail(bookingData)
            }, cancellationToken);

            //_logger.LogInformation("Sent booking confirmation for {BookingId}", bookingData.BookingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking confirmation for {BookingId}", bookingData.BookingId);
        }
    }

    public async Task SendBookingCancellationAsync(
        BookingEmailData bookingData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await SendAsync(new EmailMessage
            {
                To = bookingData.GuestEmail,
                Subject = $"✕ Booking Cancelled: {bookingData.EventTypeName}",
                Body = BookingCancellationTemplate.GenerateGuestEmail(bookingData)
            }, cancellationToken);

            await SendAsync(new EmailMessage
            {
                To = bookingData.HostEmail,
                Subject = $"Booking Cancelled: {bookingData.EventTypeName}",
                Body = BookingCancellationTemplate.GenerateHostEmail(bookingData)
            }, cancellationToken);

            //_logger.LogInformation("Sent booking cancellation for {BookingId}", bookingData.BookingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking cancellation for {BookingId}", bookingData.BookingId);
        }
    }

    public async Task SendBookingReminderAsync(
        BookingEmailData bookingData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await SendAsync(new EmailMessage
            {
                To = bookingData.GuestEmail,
                Subject = $"⏰ Reminder: {bookingData.EventTypeName} Tomorrow",
                Body = BookingReminderTemplate.Generate(bookingData)
            }, cancellationToken);

            //_logger.LogInformation("Sent booking reminder for {BookingId}", bookingData.BookingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking reminder for {BookingId}", bookingData.BookingId);
        }
    }

    public async Task SendBookingRescheduleAsync(
        BookingRescheduleEmailData rescheduleData,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await SendAsync(new EmailMessage
            {
                To = rescheduleData.GuestEmail,
                Subject = $"🔄 Booking Rescheduled: {rescheduleData.EventTypeName}",
                Body = BookingRescheduleTemplate.GenerateGuestEmail(rescheduleData)
            }, cancellationToken);

            await SendAsync(new EmailMessage
            {
                To = rescheduleData.HostEmail,
                Subject = $"Booking Rescheduled: {rescheduleData.EventTypeName}",
                Body = BookingRescheduleTemplate.GenerateHostEmail(rescheduleData)
            }, cancellationToken);

            //_logger.LogInformation("Sent booking reschedule for {BookingId}", bookingData.BookingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking reschedule for {BookingId}", rescheduleData.BookingId);
        }
    }

    private MimeMessage CreateMimeMessage(EmailMessage message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        mimeMessage.To.Add(MailboxAddress.Parse(message.To));
        mimeMessage.Subject = message.Subject;

        mimeMessage.Body = new BodyBuilder
        {
            HtmlBody = message.IsHtml ? message.Body : null,
            TextBody = message.IsHtml ? null : message.Body
        }.ToMessageBody();

        return mimeMessage;
    }

    private async Task SendMimeMessageAsync(MimeMessage mimeMessage, CancellationToken cancellationToken)
    {
        using var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await smtp.SendAsync(mimeMessage, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);

            //_logger.LogInformation("Email sent to {To}: {Subject}", mimeMessage.To.First(), mimeMessage.Subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", mimeMessage);
            throw new InvalidOperationException($"Failed to send email to {mimeMessage}", ex);
        }
    }
}
