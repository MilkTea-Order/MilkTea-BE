using MilkTea.Domain.Catalog.Material.Enums;
using MilkTea.Domain.Catalog.Material.Exceptions;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Material.Entities;

public sealed class MaterialEntity : Aggregate<int>
{
    public string Name { get; private set; } = "";
    public int MaterialsGroupID { get; private set; }
    public MaterialStatus Status { get; private set; }
    public string? Code { get; private set; }
    public int UnitID { get; private set; }
    public int? UnitID_Max { get; private set; }
    public int? StyleQuantity { get; private set; }
    public bool IsActive => Status == MaterialStatus.Active;

    private MaterialEntity() { }

    /// <summary>
    /// Creates a new MaterialEntity with the specified properties and validates input parameters.
    /// </summary>
    /// <param name="name">The name of the material.</param>
    /// <param name="materialsGroupId">The identifier of the materials group.</param>
    /// <param name="unitId">The identifier of the unit.</param>
    /// <param name="code">The optional code for the material.</param>
    /// <param name="unitIdMax">The optional maximum unit identifier.</param>
    /// <param name="styleQuantity">The optional style quantity.</param>
    /// <returns>A new instance of MaterialEntity with the provided values.</returns>
    /// <exception cref="MaterialUnitMinException">Thrown when unitId is less than or equal to zero.</exception>
    /// <exception cref="MaterialNameException">Thrown when name is null or empty.</exception>
    /// <exception cref="MaterialGroupException">Thrown when materialsGroupId is less than or equal to zero.</exception>
    public static MaterialEntity Create(
        string name,
        int materialsGroupId,
        int unitId,
        string? code = null,
        int? unitIdMax = null,
        int? styleQuantity = null)
    {
        if (unitId <= 0) throw new MaterialUnitMinException();
        if (string.IsNullOrEmpty(name)) throw new MaterialNameException();
        if (materialsGroupId <= 0) throw new MaterialGroupException();
        return new MaterialEntity
        {
            Name = name,
            Code = code,
            UnitID = unitId,
            UnitID_Max = unitIdMax,
            StyleQuantity = styleQuantity,
            MaterialsGroupID = materialsGroupId,
            Status = MaterialStatus.Active,
        };
    }

    /// <summary>
    /// Activates the material by setting its status to Active.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the material is already active.</exception>
    public void Activate()
    {
        if (Status == MaterialStatus.Active)
        {
            throw new InvalidOperationException("Material is already active.");
        }
        Status = MaterialStatus.Active;
    }

    /// <summary>
    /// Sets the material status to inactive.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the material is already inactive.</exception>
    public void Deactivate()
    {
        if (Status == MaterialStatus.Inactive)
        {
            throw new InvalidOperationException("Material is already inactive.");
        }
        Status = MaterialStatus.Inactive;
    }

    /// <summary>
    /// Updates the material information with the provided values.
    /// </summary>
    /// <param name="name">The new name for the material.</param>
    /// <param name="code">The new code for the material.</param>
    /// <param name="unitId">The new unit ID for the material.</param>
    /// <param name="unitIdMax">The new maximum unit ID for the material.</param>
    /// <param name="styleQuantity">The new style quantity for the material.</param>
    /// <exception cref="MaterialUnitMinException">Thrown when unitId is less than or equal to zero.</exception>
    /// <exception cref="MaterialUnitMaxException">Thrown when unitIdMax is less than or equal to zero.</exception>
    /// <exception cref="MaterialStyleQuantityException">Thrown when styleQuantity is less than or equal to zero.</exception>
    public void UpdateInfo(
        string? name = null,
        string? code = null,
        int? unitId = null,
        int? unitIdMax = null,
        int? styleQuantity = null)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (!string.IsNullOrEmpty(code)) Code = code;
        if (unitId.HasValue)
        {
            if (unitId.Value <= 0) throw new MaterialUnitMinException();
            UnitID = unitId.Value;
        }
        if (unitIdMax.HasValue)
        {
            if (unitIdMax.Value <= 0) throw new MaterialUnitMaxException();
            UnitID_Max = unitIdMax.Value;
        }
        if (styleQuantity.HasValue)
        {
            if (styleQuantity.Value <= 0) throw new MaterialStyleQuantityException();
            StyleQuantity = styleQuantity.Value;
        }
    }
}
