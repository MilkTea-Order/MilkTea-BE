namespace MilkTea.Domain.Respositories.Orders
{
    public interface ITableRepository
    {
        Task<List<Dictionary<string, object?>>> GetTablesByStatusAsync(int? statusID);
    }
}
