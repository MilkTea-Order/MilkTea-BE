namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    public class CurrencyBaseDto
    {
        public int ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
