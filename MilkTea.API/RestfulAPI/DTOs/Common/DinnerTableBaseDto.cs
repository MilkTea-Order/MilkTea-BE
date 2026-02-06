namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    /// <summary>
    /// Base DTO for Dinner Table information
    /// </summary>
    public class DinnerTableBaseDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Position { get; set; }
        public int NumberOfSeats { get; set; }
        public StatusBaseDto? Status { get; set; }
        public string? Note { get; set; }
    }
}
