using MilkTea.API.RestfulAPI.DTOs.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Common;

public class OrderDetailDto
{
    public int Id { get; set; }
    public MenuBaseDto? Menu { get; set; }
    public SizeBaseDto? Size { get; set; }
    public int? KindOfHotpot1ID { get; set; }
    public int? KindOfHotpot2ID { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? CancelledBy { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? Note { get; set; }
}
