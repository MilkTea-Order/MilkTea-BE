namespace MilkTea.Application.Features.Finance.Models.Dtos
{
    public class FinanceTransactionDateDto
    {
        public DateOnly Date { get; set; }

        public decimal TotalAmount { get; set; }

        public List<CollectAndSpendItemDto> Items { get; set; } = new();
    }
}
