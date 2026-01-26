namespace MilkTea.Domain.Pricing.Enums;

/// <summary>
/// Status of a promotion.
/// Maps to StatusID column in promotionontotalbill table.
/// </summary>
public enum PromotionStatus
{
    Draft = 1,      // Nháp
    Active = 2,     // Đang hoạt động
    Expired = 3,    // Hết hạn
    Cancelled = 4   // Đã hủy
}
