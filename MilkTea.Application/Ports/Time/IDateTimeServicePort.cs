namespace MilkTea.Application.Ports.Time
{
    public interface IDateTimeServicePort
    {
        DateTime UtcNow { get; }
    }
}
