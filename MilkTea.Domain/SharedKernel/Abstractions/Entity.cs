using System.Reflection;

namespace MilkTea.Domain.SharedKernel.Abstractions;

public abstract class Entity<TId> : IEntityId<TId>, IAuditable
{
    public TId Id { get; set; } = default!;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    /// Converts the object properties to a dictionary.
    /// </summary>
    public Dictionary<string, object?> ToDictionary()
    {
        return GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.GetValue(this) != null)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(this, null)
            );
    }
}
