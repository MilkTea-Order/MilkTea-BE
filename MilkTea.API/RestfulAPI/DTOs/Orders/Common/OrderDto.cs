using MilkTea.API.RestfulAPI.DTOs.Common;

namespace MilkTea.API.RestfulAPI.DTOs.Orders.Common
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderBy { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }


        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? PaymentBy { get; set; }


        public DateTime? ActionDate { get; set; }
        public int? ActionBy { get; set; }

        public DateTime? CancelledDate { get; set; }
        public int? CancelledBy { get; set; }

        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }

        public DinnerTableDto? DinnerTable { get; set; }
        public StatusBaseDto? Status { get; set; }
    }
}
