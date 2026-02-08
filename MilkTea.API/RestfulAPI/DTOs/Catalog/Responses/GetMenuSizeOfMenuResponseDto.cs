using MilkTea.API.RestfulAPI.DTOs.Common;
using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Catalog.Responses
{
    public class GetMenuSizeOfMenuResponseDto : SizeBaseDto
    {
        [JsonPropertyOrder(4)]
        public PriceBaseDto? Price { get; set; }
    }
}
