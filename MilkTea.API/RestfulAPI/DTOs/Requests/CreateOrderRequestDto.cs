using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class CreateOrderRequestDto
    {
        [Required(ErrorMessage = "DinnerTableID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "DinnerTableID must be greater than 0")]
        public int DinnerTableID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "OrderByID must be greater than 0")]
        public int? OrderByID { get; set; }

        [Required(ErrorMessage = "Items is required")]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<OrderItemRequest> Items { get; set; } = new();

        [MaxLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
        public string? Note { get; set; }
    }

    public class OrderItemRequest
    {
        [Required(ErrorMessage = "MenuID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MenuID must be greater than 0")]
        public int MenuID { get; set; }

        [Required(ErrorMessage = "SizeID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SizeID must be greater than 0")]
        public int SizeID { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        public List<int>? ToppingIDs { get; set; }
        public List<int>? KindOfHotpotIDs { get; set; }

        [MaxLength(200, ErrorMessage = "Note cannot exceed 200 characters")]
        public string? Note { get; set; }
    }
}