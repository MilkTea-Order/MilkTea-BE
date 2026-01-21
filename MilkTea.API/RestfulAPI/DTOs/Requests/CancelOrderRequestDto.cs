using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class CancelOrderRequestDto
    {
        [MaxLength(500, ErrorMessage = "Cancel note cannot exceed 500 characters")]
        public string? CancelNote { get; set; }
    }
}