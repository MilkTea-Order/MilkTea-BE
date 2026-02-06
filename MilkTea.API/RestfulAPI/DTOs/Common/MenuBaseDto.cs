namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    public class MenuBaseDto
    {
        public int ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public StatusBaseDto? Status { get; set; }
        public UnitBaseDto? Unit { get; set; }
        public string? Note { get; set; }
    }
}
