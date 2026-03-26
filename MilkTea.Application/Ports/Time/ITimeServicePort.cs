namespace MilkTea.Application.Ports.Time
{
    public interface ITimeServicePort
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        DateTime ToLocal(DateTime utc);
        DateTime ToUtc(DateTime local);
    }
}
