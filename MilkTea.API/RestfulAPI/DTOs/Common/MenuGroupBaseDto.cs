namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    public class MenuGroupBaseDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public StatusBaseDto? Status { get; set; }
    }
}
