using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Order.Responses;

public class GetOrdersByOrderByAndStatusResponseDto : MilkTea.API.RestfulAPI.DTOs.Orders.Common.OrderDto
{
    [JsonPropertyOrder(10)]
    public DateTime? PaymentDate { get; set; }
    [JsonPropertyOrder(11)]
    public int? PaymentBy { get; set; }
    [JsonPropertyOrder(12)]
    public DateTime? ActionDate { get; set; }
    [JsonPropertyOrder(12)]
    public int? ActionBy { get; set; }
    [JsonPropertyOrder(13)]
    public DateTime? CancelledDate { get; set; }
    [JsonPropertyOrder(14)]
    public int CancelledBy { get; set; }
}
