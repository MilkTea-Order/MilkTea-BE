using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class CancelOrderRequestDto
    {
        [Required(ErrorMessage = "OrderID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderID must be greater than 0")]
        public int OrderID { get; set; }

        [MaxLength(500, ErrorMessage = "Cancel note cannot exceed 500 characters")]
        public string? CancelNote { get; set; }
    }
}