using System.ComponentModel.DataAnnotations;

namespace MilkTea.API.RestfulAPI.DTOs.Requests
{
    public class GetOrdersOfMeByStatusIDRequestDto
    {
        [Required(ErrorMessage = "StatusID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "StatusID must be greater than 0")]
        public int StatusID { get; set; }
    }
}
