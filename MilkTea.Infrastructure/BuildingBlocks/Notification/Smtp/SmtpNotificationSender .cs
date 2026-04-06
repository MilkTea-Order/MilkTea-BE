using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MilkTea.Application.Ports.Notification;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace MilkTea.Infrastructure.BuildingBlocks.Notification.SMTP;


public class SmtpOptions
{
    public const string SectionName = "Smtp";

    public string Host { get; init; } = string.Empty;

    public int Port { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string FromEmail { get; init; } = string.Empty;

    public string FromName { get; init; } = string.Empty;

    public bool EnableSsl { get; init; } = true;
}


public static class SmtpNotificationRegistration
{
    public static IServiceCollection AddNotificationSMTP(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));

        var smtpOptions = configuration.GetSection("Smtp").Get<SmtpOptions>()
            ?? throw new InvalidOperationException("Smtp settings are missing or invalid.");

        if (string.IsNullOrWhiteSpace(smtpOptions.Host))
            throw new InvalidOperationException("Smtp:Host is required.");

        if (smtpOptions.Port <= 0)
            throw new InvalidOperationException("Smtp:Port is invalid.");

        services.AddScoped<INotificationSender, SmtpNotificationSender>();

        return services;
    }
}

internal sealed class SmtpNotificationSender(IOptions<SmtpOptions> options) : INotificationSender
{
    private readonly SmtpOptions _vOptions = options.Value;
    public async Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Channel != NotificationChannel.Email)
            throw new NotSupportedException("SMTP only supports Email.");

        if (string.IsNullOrWhiteSpace(request.Recipient))
            throw new ArgumentException("Recipient is required.");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_vOptions.FromName, _vOptions.FromEmail));
        email.To.Add(MailboxAddress.Parse(request.Recipient));
        email.MessageId = MimeKit.Utils.MimeUtils.GenerateMessageId();
        email.Headers.Add("X-Entity-Ref-ID", Guid.NewGuid().ToString());
        email.Subject = request.Subject ?? "";
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = request.Content
        };

        using var smtp = new SmtpClient();

        var secure = _vOptions.EnableSsl ? SecureSocketOptions.StartTls
                                            : SecureSocketOptions.None;

        await smtp.ConnectAsync(_vOptions.Host, _vOptions.Port, secure, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_vOptions.Username))
        {
            await smtp.AuthenticateAsync(_vOptions.Username, _vOptions.Password, cancellationToken);
        }

        await smtp.SendAsync(email, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}
