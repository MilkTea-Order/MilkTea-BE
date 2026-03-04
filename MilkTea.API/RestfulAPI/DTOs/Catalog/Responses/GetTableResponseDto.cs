using MilkTea.API.RestfulAPI.DTOs.Common;
using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Catalog.Responses
{
    public class GetTableResponseDto : DinnerTableBaseDto
    {
        [JsonPropertyOrder(8)]
        public string? EmptyImg { get; set; } = null;

        [JsonPropertyOrder(9)]
        public string? UsingImg { get; set; } = null;
    }
}
