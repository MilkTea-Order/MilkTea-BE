using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class CancelOrderDetailsRequestDto
    {
        [Required(ErrorMessage = "OrderDetailIDs is required")]
        [MinLength(1, ErrorMessage = "At least one OrderDetailID is required")]
        public List<int> OrderDetailIDs { get; set; } = new();
    }
}