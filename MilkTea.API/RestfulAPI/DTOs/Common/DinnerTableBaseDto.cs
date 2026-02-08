using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    /// <summary>
    /// Base DTO for Dinner Table information
    /// </summary>
    public class DinnerTableBaseDto
    {
        [JsonPropertyOrder(1)]
        public int ID { get; set; }
        [JsonPropertyOrder(2)]
        public string? Code { get; set; }
        [JsonPropertyOrder(3)]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyOrder(4)]
        public string? Position { get; set; }
        [JsonPropertyOrder(5)]
        public int NumberOfSeats { get; set; }
        [JsonPropertyOrder(6)]
        public StatusBaseDto? Status { get; set; }
        [JsonPropertyOrder(7)]
        public string? Note { get; set; }
    }
}
