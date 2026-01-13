using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Domain.Respositories.Orders
{
    public interface ISizeRepository
    {
        Task<Size?> GetSizeByIdAsync(int sizeId);
    }
}