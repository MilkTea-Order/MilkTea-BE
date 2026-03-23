using System.Text.Json.Serialization;

namespace MilkTea.Application.Features.Finance.Models.Dtos
{
    public class CollectionAndSpendGroupDto
    {
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyOrder(2)]
        public string Name { get; set; } = default!;
    }
}
