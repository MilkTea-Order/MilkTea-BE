namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class GetOrderDetailByIDAndStatusResponseDto
    {
        public int OrderID { get; set; }
        public int DinnerTableID { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int StatusID { get; set; }
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }

        public DinnerTableDto? DinnerTable { get; set; }
        public OrderStatusDto? Status { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; } = default!;
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int MenuID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int PriceListID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? Note { get; set; }
        public int? KindOfHotpot1ID { get; set; }
        public int? KindOfHotpot2ID { get; set; }
        public int SizeID { get; set; }

        public MenuDto? Menu { get; set; }
        public SizeDto? Size { get; set; }
    }

    public class MenuDto
    {
        public int ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? MenuGroupName { get; set; }
        public string? StatusName { get; set; }
        public string? UnitName { get; set; }
        public string? Note { get; set; }
    }

    public class SizeDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RankIndex { get; set; }
    }
}
