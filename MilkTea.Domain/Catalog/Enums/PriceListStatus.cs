namespace MilkTea.Domain.Catalog.Enums;

/// <summary>
/// Status of a price list.
/// Maps to StatusOfPriceListID column in pricelist table.
/// </summary>
public enum PriceListStatus
{
    Draft = 1,      // Chưa phát hành
    Active = 2,     // Đang hiện hành
    Expired = 3     // Hết hạn
}
