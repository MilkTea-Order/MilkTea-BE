namespace MilkTea.Domain.Orders.ValueObjects;

/// <summary>
/// Value Object capturing menu item info at the time of order.
/// Snapshot để giữ thông tin menu lúc đặt hàng, không phụ thuộc vào entity Menu.
/// Immutable value object.
/// </summary>
public sealed class MenuItem : IEquatable<MenuItem>
{
    public int MenuId { get; }
    public int SizeId { get; }
    public int PriceListId { get; }
    public decimal Price { get; }
    public int? KindOfHotpot1Id { get; }
    public int? KindOfHotpot2Id { get; }

    // For EF Core
    private MenuItem() { }

    private MenuItem(
        int menuId,
        int sizeId,
        int priceListId,
        decimal price,
        int? kindOfHotpot1Id,
        int? kindOfHotpot2Id)
    {
        MenuId = menuId;
        SizeId = sizeId;
        PriceListId = priceListId;
        Price = price;
        KindOfHotpot1Id = kindOfHotpot1Id;
        KindOfHotpot2Id = kindOfHotpot2Id;
    }

    public static MenuItem Of(
        int menuId,
        int sizeId,
        decimal price,
        int priceListId,
        int? kindOfHotpot1Id = null,
        int? kindOfHotpot2Id = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(menuId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sizeId);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(priceListId);

        if (kindOfHotpot1Id.HasValue && kindOfHotpot1Id.Value <= 0)
            throw new ArgumentOutOfRangeException(nameof(kindOfHotpot1Id));

        if (kindOfHotpot2Id.HasValue && kindOfHotpot2Id.Value <= 0)
            throw new ArgumentOutOfRangeException(nameof(kindOfHotpot2Id));

        return new MenuItem(menuId, sizeId, priceListId, price, kindOfHotpot1Id, kindOfHotpot2Id);
    }

    // Value Object equality
    public bool Equals(MenuItem? other)
    {
        if (other is null) return false;
        return MenuId == other.MenuId &&
               SizeId == other.SizeId &&
               PriceListId == other.PriceListId &&
               KindOfHotpot1Id == other.KindOfHotpot1Id &&
               KindOfHotpot2Id == other.KindOfHotpot2Id;
    }

    public override bool Equals(object? obj) => Equals(obj as MenuItem);

    public override int GetHashCode() => HashCode.Combine(MenuId, SizeId, PriceListId, KindOfHotpot1Id, KindOfHotpot2Id);

    public static bool operator ==(MenuItem? left, MenuItem? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(MenuItem? left, MenuItem? right) => !(left == right);
}