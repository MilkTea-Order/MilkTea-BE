namespace MilkTea.API.RestfulAPI.DTOs.Finance.Requests
{
    public class CreateTransactionRequestDto
    {
        public int TransactionGroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public int TransactionBy { get; set; }
        public int Amount { get; set; }
        public string? Note { get; set; }
    }
}
