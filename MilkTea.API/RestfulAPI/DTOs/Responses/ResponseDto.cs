namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class ResponseDto
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public string? Description { get; set; }
        public object? Data { get; set; }
    }
}
