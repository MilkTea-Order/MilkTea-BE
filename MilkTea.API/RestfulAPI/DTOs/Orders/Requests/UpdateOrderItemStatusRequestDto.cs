namespace MilkTea.API.RestfulAPI.DTOs.Orders.Requests;

public class UpdateOrderItemStatusRequestDto
{
    public List<UpdateOrderItemStatusItemDto> Items { get; set; } = [];
}

public class UpdateOrderItemStatusItemDto
{
    public int OrderID { get; set; }
    public int OrderDetailID { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
}
