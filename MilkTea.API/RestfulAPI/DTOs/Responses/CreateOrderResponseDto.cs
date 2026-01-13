namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class CreateOrderResponseDto
    {
        public int OrderID { get; set; }
        public string BillNo { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }

    public class OrderItemResponse
    {
        //public int OrderDetailID { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public int SizeID { get; set; }
        public string? SizeName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}