namespace MilkTea.Application.Features.Orders.Abstractions
{
    public interface IOrderQueries
    {
        Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default);
    }
}
