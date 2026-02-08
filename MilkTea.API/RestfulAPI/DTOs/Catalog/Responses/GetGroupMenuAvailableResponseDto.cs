using MilkTea.API.RestfulAPI.DTOs.Common;
using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Catalog.Responses
{
    public class GetGroupMenuAvailableResponseDto : MenuGroupBaseDto
    {
        [JsonPropertyOrder(4)]
        public int? Quantity { get; set; } = 0;
    }
}
