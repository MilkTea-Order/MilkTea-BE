using System.Text.Json.Serialization;
using MilkTea.API.RestfulAPI.DTOs.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses;

public class GetKitchenOrdersResponseDto
{
    [JsonPropertyOrder(1)]
    public List<KitchenOrderDto> Orders { get; set; } = new();
}

public class KitchenOrderDto
{
    [JsonPropertyOrder(1)]
    public int OrderID { get; set; }
    [JsonPropertyOrder(2)]
    public DinnerTableBaseDto? DinnerTable { get; set; }
    [JsonPropertyOrder(3)]
    public StatusBaseDto? Status { get; set; } 
    [JsonPropertyOrder(4)]
    public List<KitchenOrderItemDto> Items { get; set; } = new();
    [JsonPropertyOrder(5)]
    public string? Note { get; set; }
    [JsonPropertyOrder(6)]
    public DateTime CreatedDate { get; set; }  
    [JsonPropertyOrder(7)]
    public int CreatedBy { get; set; }
}

public class KitchenOrderItemDto
{
    [JsonPropertyOrder(1)]
    public int Id { get; set; }
    //[JsonPropertyOrder(2)]
    //public int OrderId { get; set; }
    [JsonPropertyOrder(2)]
    public MenuBaseDto? Menu { get; set; }
    [JsonPropertyOrder(3)]
    public SizeBaseDto? Size { get; set; }
    [JsonPropertyOrder(4)]
    public int Quantity { get; set; }
    [JsonPropertyOrder(5)]
    public StatusBaseDto? Status { get; set; }
    [JsonPropertyOrder(6)]
    public string? Note { get; set; }
    [JsonPropertyOrder(7)]
    public int? KindOfHotpot1Id { get; set; }
    [JsonPropertyOrder(8)]
    public int? KindOfHotpot2Id { get; set; }
}
