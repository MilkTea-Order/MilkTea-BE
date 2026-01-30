using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Abstractions;
using MilkTea.Domain.SharedKernel.Enums;

namespace MilkTea.Domain.Catalog.Entities;

public sealed class MenuGroup : Aggregate<int>
{
    private readonly List<Menu> _vMenus = new();
    public IReadOnlyList<Menu> Menus => _vMenus.AsReadOnly();

    public string Name { get; private set; } = null!;
    public CommonStatus Status { get; private set; }
    public bool IsActive => Status == CommonStatus.Active;

    private MenuGroup() { }

    public static MenuGroup Create(string name, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        return new MenuGroup
        {
            Name = name,
            Status = CommonStatus.Active,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };
    }

    // Actions on MenuGroup
    public void Activate(int by)
    {
        if (Status == CommonStatus.Active)
            throw new InvalidOperationException("MenuGroup is already active.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(by);
        Status = CommonStatus.Active;
        Touch(by);
    }
    public void Deactivate(int by)
    {
        if (Status == CommonStatus.Inactive) throw new InvalidOperationException("MenuGroup is already inactive.");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(by);

        Status = CommonStatus.Inactive;
        foreach (var m in _vMenus)
        {
            if (m.Status == MenuStatus.Active)
                m.Deactivate(by);
        }

        Touch(by);
    }


    // Action on Menu within this MenuGroup
    public Menu AddMenu(
       string code,
       string name,
       int unitId,
       int createdBy,
       string? formula = null,
       string? note = null,
       int? tasteQty = null,
       bool? printSticker = null)
    {
        if (!IsActive) throw new InvalidOperationException("MenuGroup is inactive.");
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(unitId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(createdBy);

        if (_vMenus.Any(m => m.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Menu code already exists in this group.");

        var menu = Menu.Create(code, name, this.Id, unitId, createdBy, formula, note, tasteQty, printSticker);
        _vMenus.Add(menu);
        Touch(createdBy);
        return menu;
    }
    public void ActivateMenu(int menuId, int by)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot activate menu because MenuGroup is inactive.");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(by);

        var menu = FindMenu(menuId);
        menu.Activate(by);
    }

    public void DeactivateMenu(int menuId, int by)
    {
        var menu = FindMenu(menuId);
        menu.Deactivate(by);
    }

    private Menu FindMenu(int menuId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(menuId);
        var menu = _vMenus.FirstOrDefault(x => x.Id == menuId);
        if (menu is null) throw new KeyNotFoundException("Menu not found in this group.");
        return menu;
    }

    public void UpdateMenuInfo(int menuId, string name, string? formula, string? note, int? tasteQty, bool? printSticker, int updatedBy)
    {
        var menu = FindMenu(menuId);
        if (!IsActive) throw new InvalidOperationException("MenuGroup is inactive.");

        menu.UpdateInfo(name, formula, note, tasteQty, printSticker, updatedBy);
        Touch(updatedBy);
    }

    public void SetMenuSizePrice(int menuId, int sizeId, decimal? cost, decimal? sale, int updatedBy)
    {
        var menu = FindMenu(menuId);
        if (!IsActive) throw new InvalidOperationException("MenuGroup is inactive.");

        menu.SetSizePrice(sizeId, cost, sale, updatedBy);
        Touch(updatedBy);
    }

    private void Touch(int updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedDate = DateTime.UtcNow;
    }
}
