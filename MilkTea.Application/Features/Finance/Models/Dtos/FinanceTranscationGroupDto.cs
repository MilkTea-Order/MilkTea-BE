using System.Text.Json.Serialization;

namespace MilkTea.Application.Features.Finance.Models.Dtos
{
    public class FinanceTranscationGroupDto : CollectionAndSpendGroupDto
    {
        //public int Id { get; set; }

        //public string Name { get; set; } = default!;

        [JsonPropertyOrder(3)]
        public decimal TotalAmount { get; set; }

        [JsonPropertyOrder(4)]
        public List<FinanceTransactionDateDto> Dates { get; set; } = new();
    }
}
