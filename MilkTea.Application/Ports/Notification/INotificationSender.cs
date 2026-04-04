namespace MilkTea.Application.Ports.Notification
{
    public interface INotificationSender
    {
        Task SendAsync(NotificationRequest request, CancellationToken cancellationToken = default);
    }

    public enum NotificationChannel
    {
        Email = 1,
        Sms = 2
    }

    public sealed record NotificationRequest(NotificationChannel Channel, string Recipient, string? Subject, string Content);
}
