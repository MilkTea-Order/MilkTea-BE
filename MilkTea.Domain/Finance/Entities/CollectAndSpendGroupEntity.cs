using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Finance.Entities
{
    public class CollectAndSpendGroupEntity : EntityId<int>
    {
        public string Name { get; private set; } = string.Empty;
        private CollectAndSpendGroupEntity() { }
    }
}
