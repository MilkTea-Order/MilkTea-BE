using MilkTea.API.RestfulAPI.DTOs.Common;
using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Common
{
    public class DinnerTableUsingDto : DinnerTableBaseDto
    {
        [JsonPropertyOrder(8)]
        public string? UsingImg { get; set; }
    }
}
