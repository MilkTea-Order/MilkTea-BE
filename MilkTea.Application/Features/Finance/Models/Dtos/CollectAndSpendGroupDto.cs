namespace MilkTea.Application.Features.Finance.Models.Dtos
{
    public class CollectAndSpendGroupDto
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; } = default!;

        public decimal TotalAmount { get; set; }

        public List<CollectAndSpendDateDto> Dates { get; set; } = new();
    }
}
