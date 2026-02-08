using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;

namespace MilkTea.Domain.Catalog.Entities.Menu;
public sealed class MenuEntity : Entity<int>
{
    private readonly List<MenuSizeEntity> _vMenuSizes = new();
    public IReadOnlyList<MenuSizeEntity> MenuSizes => _vMenuSizes.AsReadOnly();

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    public int MenuGroupID { get; private set; }
    public MenuStatus Status { get; private set; }
    public int UnitID { get; private set; }
    public string? Formula { get; private set; }
    public byte[]? AvatarPicture { get; private set; }
    public string? Note { get; private set; }
    public int? TasteQTy { get; private set; }
    public bool? PrintSticker { get; private set; }

    private MenuEntity() { }

    internal static MenuEntity Create(
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
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        return new MenuEntity
        {
            Code = code,
            Name = name,
            Formula = formula,
            Note = note,
            Status = MenuStatus.Active,
            MenuGroupID = menuGroupId,
            UnitID = unitId,
            TasteQTy = tasteQty,
            PrintSticker = printSticker,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };
    }

    internal void SetSizePrice(int sizeId, decimal? cost, decimal? sale, int updatedBy)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(updatedBy);

        var existing = _vMenuSizes.FirstOrDefault(x => x.SizeID == sizeId);
        if (existing is null) _vMenuSizes.Add(MenuSizeEntity.Create(sizeId, cost, sale));
        else existing.UpdatePrice(cost, sale);

        Touch(updatedBy);
    }

    internal void Activate(int by)
    {
        if (Status == MenuStatus.Active)
            throw new InvalidOperationException("Menu is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(by);

        Status = MenuStatus.Active;
        Touch(by);
    }

    internal void Deactivate(int by)
    {
        if (Status == MenuStatus.Inactive)
            throw new InvalidOperationException("Menu is already inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(by);

        Status = MenuStatus.Inactive;
        Touch(by);
    }


    internal void UpdateInfo(string name, string? formula, string? note, int? tasteQty, bool? printSticker, int updatedBy)
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

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
