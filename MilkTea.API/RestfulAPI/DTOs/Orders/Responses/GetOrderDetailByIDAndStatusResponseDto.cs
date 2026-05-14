using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses;

public class GetOrderDetailByIdAndStatusResponseDto : MilkTea.API.RestfulAPI.DTOs.Orders.Common.OrderDto
{
    [JsonPropertyOrder(3)]
    public List<MilkTea.API.RestfulAPI.DTOs.Orders.Common.OrderDetailDto> Items { get; set; } = default!;
}
