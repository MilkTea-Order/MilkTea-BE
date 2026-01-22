namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class GetOrdersByOrderByAndStatusResponseDto
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

        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    public class DinnerTableDto
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Position { get; set; }
        public int NumberOfSeats { get; set; }
        public string? StatusName { get; set; }
        public string? Note { get; set; }
    }

    public class OrderStatusDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
