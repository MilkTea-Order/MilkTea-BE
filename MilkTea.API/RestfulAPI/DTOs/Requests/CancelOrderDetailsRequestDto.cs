using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class CancelOrderDetailsRequestDto
    {
        [Required(ErrorMessage = "OrderID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be greater than 0")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "OrderDetailIDs is required")]
        [MinLength(1, ErrorMessage = "At least one OrderDetailID is required")]
        public List<int> OrderDetailIDs { get; set; } = new();
    }
}