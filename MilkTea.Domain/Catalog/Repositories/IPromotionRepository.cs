using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Domain.Catalog.Repositories;

/// <summary>
/// Repository interface for promotion operations.
/// </summary>
public interface IPromotionRepository
{
    /// <summary>
    /// Gets a promotion by ID.
    /// </summary>
    Task<PromotionOnTotalBill?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all active promotions.
    /// </summary>
    Task<List<PromotionOnTotalBill>> GetActivePromotionsAsync();
}
