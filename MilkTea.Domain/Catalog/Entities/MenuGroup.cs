using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.SharedKernel.Enums;

namespace MilkTea.Domain.Catalog.Entities;

/// <summary>
/// Menu group entity for categorizing menu items.
/// </summary>
public sealed class MenuGroup : Entity<int>
{

    private readonly List<Menu> _vMenus = new();
    public IReadOnlyList<Menu> Menus => _vMenus.AsReadOnly();

    public string Name { get; private set; } = null!;

    public CommonStatus Status { get; private set; }


    // For EF Core
    private MenuGroup() { }

    public static MenuGroup Create(string name, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new MenuGroup
        {
            Name = name,
            Status = CommonStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive => Status == CommonStatus.Active;

    public void Activate(int activatedBy)
    {
        if (Status == CommonStatus.Active)
            throw new InvalidOperationException("MenuGroup is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = CommonStatus.Active;
        Touch(activatedBy);
    }

    public void Deactivate(int deactivatedBy)
    {
        if (Status == CommonStatus.Inactive)
            throw new InvalidOperationException("MenuGroup is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deactivatedBy);

        Status = CommonStatus.Inactive;
        Touch(deactivatedBy);
    }

    public void UpdateName(string name, int updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
