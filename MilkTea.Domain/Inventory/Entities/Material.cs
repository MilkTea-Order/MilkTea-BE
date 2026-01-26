using MilkTea.Domain.Inventory.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Inventory.Entities;

/// <summary>
/// Material entity.
/// </summary>
public sealed class Material : Entity<int>
{
    public string Name { get; private set; } = null!;
    public string? Code { get; private set; }
    public int? UnitID { get; private set; }
    public int? UnitID_Max { get; private set; }
    public int? StyleQuantity { get; private set; }
    public int MaterialsGroupID { get; private set; }
    
    /// <summary>
    /// Material status. Maps to StatusID column.
    /// </summary>
    public MaterialStatus Status { get; private set; }

    // Navigations
    public MaterialsGroup? MaterialsGroup { get; private set; }
    public Unit? Unit { get; private set; }
    public Unit? UnitMax { get; private set; }

    // For EF Core
    private Material() { }

    public static Material Create(
        string name,
        int materialsGroupId,
        int createdBy,
        string? code = null,
        int? unitId = null,
        int? unitIdMax = null,
        int? styleQuantity = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(materialsGroupId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new Material
        {
            Name = name,
            Code = code,
            UnitID = unitId,
            UnitID_Max = unitIdMax,
            StyleQuantity = styleQuantity,
            MaterialsGroupID = materialsGroupId,
            Status = MaterialStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive => Status == MaterialStatus.Active;

    public void Activate(int activatedBy)
    {
        if (Status == MaterialStatus.Active)
            throw new InvalidOperationException("Material is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = MaterialStatus.Active;
        Touch(activatedBy);
    }

    public void Deactivate(int deactivatedBy)
    {
        if (Status == MaterialStatus.Inactive)
            throw new InvalidOperationException("Material is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deactivatedBy);

        Status = MaterialStatus.Inactive;
        Touch(deactivatedBy);
    }

    public void UpdateInfo(
        string name,
        string? code = null,
        int? unitId = null,
        int? unitIdMax = null,
        int? styleQuantity = null,
        int updatedBy = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Code = code;
        UnitID = unitId;
        UnitID_Max = unitIdMax;
        StyleQuantity = styleQuantity;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
