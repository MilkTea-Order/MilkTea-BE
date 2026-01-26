using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities;

/// <summary>
/// Menu item entity.
/// </summary>
public sealed class Menu : Entity<int>
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Formula { get; private set; }
    public byte[]? AvatarPicture { get; private set; }
    public string? Note { get; private set; }
    public int MenuGroupID { get; private set; }
    
    /// <summary>
    /// Menu status. Maps to StatusID column.
    /// </summary>
    public MenuStatus Status { get; private set; }
    
    public int UnitID { get; private set; }
    public int? TasteQTy { get; private set; }
    public bool? PrintSticker { get; private set; }

    // Navigations
    public MenuGroup? MenuGroup { get; private set; }

    // For EF Core
    private Menu() { }

    public static Menu Create(
        string code,
        string name,
        int menuGroupId,
        int unitId,
        int createdBy,
        string? formula = null,
        string? note = null,
        int? tasteQty = null,
        bool? printSticker = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(menuGroupId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        var now = DateTime.UtcNow;

        return new Menu
        {
            Code = code,
            Name = name,
            Formula = formula,
            Note = note,
            MenuGroupID = menuGroupId,
            Status = MenuStatus.Active,
            UnitID = unitId,
            TasteQTy = tasteQty,
            PrintSticker = printSticker,
            CreatedBy = createdBy,
            CreatedDate = now
        };
    }

    public bool IsActive => Status == MenuStatus.Active;

    public void Activate(int activatedBy)
    {
        if (Status == MenuStatus.Active)
            throw new InvalidOperationException("Menu is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activatedBy);

        Status = MenuStatus.Active;
        Touch(activatedBy);
    }

    public void Deactivate(int deactivatedBy)
    {
        if (Status == MenuStatus.Inactive)
            throw new InvalidOperationException("Menu is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(deactivatedBy);

        Status = MenuStatus.Inactive;
        Touch(deactivatedBy);
    }

    public void UpdateInfo(
        string name,
        string? formula = null,
        string? note = null,
        int? tasteQty = null,
        bool? printSticker = null,
        int updatedBy = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        Name = name;
        Formula = formula;
        Note = note;
        TasteQTy = tasteQty;
        PrintSticker = printSticker;
        Touch(updatedBy);
    }

    public void UpdateAvatar(byte[]? avatarPicture, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        AvatarPicture = avatarPicture;
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
