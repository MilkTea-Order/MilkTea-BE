namespace MilkTea.Application.Features.Orders.Abstractions.Services
{
    public interface IOrderServices
    {
        Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default);
    }
}
