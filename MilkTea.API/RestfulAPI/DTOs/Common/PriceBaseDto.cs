using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Common
{
    public class PriceBaseDto
    {
        [JsonPropertyOrder(1)]
        public int PriceListID { get; set; }
        [JsonPropertyOrder(2)]
        public decimal Price { get; set; }
        [JsonPropertyOrder(3)]
        public CurrencyBaseDto? Currency { get; set; }
    }
}
