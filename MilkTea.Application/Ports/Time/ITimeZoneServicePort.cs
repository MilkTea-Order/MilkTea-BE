namespace MilkTea.Application.Ports.Time
{
    public interface ITimeZoneServicePort
    {
        TimeZoneInfo GetTimeZone();
    }
}
