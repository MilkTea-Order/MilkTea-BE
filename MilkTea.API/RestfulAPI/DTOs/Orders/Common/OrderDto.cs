using MilkTea.API.RestfulAPI.DTOs.Common;
using System.Text.Json.Serialization;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Common;

public class OrderDto
{
    [JsonPropertyOrder(1)]
    public int OrderID { get; set; }

    [JsonPropertyOrder(2)]
    public DinnerTableDto? DinnerTable { get; set; }

    [JsonPropertyOrder(3)]
    public string? Note { get; set; }

    [JsonPropertyOrder(4)]
    public decimal TotalAmount { get; set; }

    [JsonPropertyOrder(5)]
    public StatusBaseDto? Status { get; set; }

    [JsonPropertyOrder(6)]
    public int OrderBy { get; set; }

    [JsonPropertyOrder(7)]
    public DateTime OrderDate { get; set; }

    [JsonPropertyOrder(8)]
    public int CreatedBy { get; set; }

    [JsonPropertyOrder(9)]
    public DateTime CreatedDate { get; set; }
}
