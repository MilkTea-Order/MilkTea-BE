using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class GetOrderDetailByIDAndStatusRequestDto
    {
        [Required(ErrorMessage = "OrderID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be greater than 0")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "IsCancelled is required")]
        public bool IsCancelled { get; set; }
    }
}
