namespace MilkTea.Application.Features.Orders.Abstractions.Services
{
    public interface IOrderServices
    {
        //Task<bool> IsTableAvailable(int tableId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of table IDs filtered by their availability status.
        /// </summary>
        /// <param name="tableIds">A list of table IDs to check for availability.</param>
        /// <param name="isAvailable">Specifies whether to filter for available or unavailable tables.</param>
        /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of table IDs matching the specified
        /// availability.</returns>
        Task<List<int>> GetTablesByAvailability(List<int> tableIds, bool isAvailable, CancellationToken cancellationToken = default);
    }
}
