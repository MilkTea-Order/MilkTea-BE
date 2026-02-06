using MilkTea.API.RestfulAPI.DTOs.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Responses
{
    public class GetOrderDetailByIDAndStatusResponseDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }
        public DinnerTableBaseDto? DinnerTable { get; set; }
        public StatusBaseDto? Status { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = default!;
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int MenuID { get; set; }
        public int SizeID { get; set; }

        public int? CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? Note { get; set; }
        public int? KindOfHotpot1ID { get; set; }
        public int? KindOfHotpot2ID { get; set; }
        public MenuBaseDto? Menu { get; set; }
        public SizeBaseDto? Size { get; set; }
    }
}
