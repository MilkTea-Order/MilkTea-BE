namespace MilkTea.API.RestfulAPI.DTOs.Responses
{
    public class GetTableByStatusResponseDto
    {
        public int TableID { get; set; }
        public string TableCode { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public int NumberOfSeat { get; set; }
        public string TableNote { get; set; } = string.Empty;
        public int StatusID { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
