namespace MilkTea.Application.Features.Finance.Models.Dtos
{
    public class CollectAndSpendItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public decimal Amount { get; set; }

        public DateTime ActionDate { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
