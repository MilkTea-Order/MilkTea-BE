namespace MilkTea.API.RestfulAPI.DTOs.Orders.Requests;

public class UpdateOrderItemStatusRequestDto
{
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
}
