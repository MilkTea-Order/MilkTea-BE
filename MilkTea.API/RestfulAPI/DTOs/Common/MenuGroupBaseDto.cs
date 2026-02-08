using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    public class MenuGroupBaseDto
    {
        [JsonPropertyOrder(1)]
        public int ID { get; set; }
        [JsonPropertyOrder(2)]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyOrder(3)]
        public StatusBaseDto? Status { get; set; }
    }
}
