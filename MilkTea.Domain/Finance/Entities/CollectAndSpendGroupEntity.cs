using MilkTea.Domain.Common.Abstractions;

namespace MilkTea.Domain.Finance.Entities
{
    public class CollectAndSpendGroupEntity : EntityId<int>
    {
        public string Name { get; private set; } = string.Empty;
        private CollectAndSpendGroupEntity() { }
    }
}
