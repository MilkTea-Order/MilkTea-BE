using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Finance.Entities
{
    public class CollectAndSpendEntity : Aggregate<int>
    {
        public int CollectAndSpendGroupID { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public DateTime ActionDate { get; private set; } = DateTime.UtcNow;
        public int ActionBy { get; private set; }

        public decimal Amount { get; private set; }

        public string? Note { get; private set; }

        private CollectAndSpendEntity() { }

        public static CollectAndSpendEntity Create(int collectAndSpendGroupID, string name, int actionBy, int createdBy, decimal amount, string? note)
        {
            var now = DateTime.UtcNow;
            return new CollectAndSpendEntity
            {
                CollectAndSpendGroupID = collectAndSpendGroupID,
                Name = name,
                ActionDate = now,
                ActionBy = actionBy,
                CreatedBy = createdBy,
                CreatedDate = now,
                Amount = amount,
                Note = note
            };
        }
    }
}
