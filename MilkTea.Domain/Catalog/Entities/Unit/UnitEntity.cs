using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities.Unit
{
    public class UnitEntity : Aggregate<int>
    {
        public string Name { get; private set; } = null!;

        private UnitEntity() { }

        internal static UnitEntity Create(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return new UnitEntity
            {
                Name = name.Trim()
            };
        }

        internal void Rename(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var newName = name.Trim();

            if (string.Equals(Name, newName, StringComparison.Ordinal)) return;

            Name = newName;
        }
    }
}
