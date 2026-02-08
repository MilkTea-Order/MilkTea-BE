using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    public class MenuBaseDto
    {
        [JsonPropertyOrder(1)]
        public int ID { get; set; }
        [JsonPropertyOrder(2)]
        public int MenuGroupID { get; set; }
        [JsonPropertyOrder(3)]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyOrder(4)]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyOrder(5)]
        public StatusBaseDto? Status { get; set; }
        [JsonPropertyOrder(6)]
        public UnitBaseDto? Unit { get; set; }
        [JsonPropertyOrder(7)]
        public string? Note { get; set; }
    }
}
