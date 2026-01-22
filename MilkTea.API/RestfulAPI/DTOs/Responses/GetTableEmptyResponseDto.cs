namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class GetTableEmptyResponseDto
    {
        public int TableID { get; set; }
        public string TableCode { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public int NumberOfSeat { get; set; }
        public string? TableNote { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string? UsingImg { get; set; }
        public string? EmptyImg { get; set; }
    }
}
