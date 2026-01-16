using MilkTea.Application.Models.Errors;
using MilkTea.Application.Models.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.Services.Orders
{
    public class OrderStockService(IWarehouseRepository warehouseRepository)
    {
        private readonly IWarehouseRepository _vWarehouseRepository = warehouseRepository;

        public async Task<ValidationError?> CheckAvailability(List<OrderItemValidation> items)
        {
            var required = new Dictionary<int, decimal>();

            foreach (var item in items)
            {
                foreach (var recipe in item.Recipe!)
                {
                    var qty = recipe.Quantity * item.Item!.Quantity;
                    required[recipe.MaterialsID] = required.GetValueOrDefault(recipe.MaterialsID) + qty;
                }
            }
            var stock = await _vWarehouseRepository.GetMaterialStockAsync(required.Keys.ToList());

            var insufficientMaterials = new List<string>();
            foreach (var req in required)
            {
                if (!stock.ContainsKey(req.Key) || stock[req.Key] < req.Value)
                {
                    insufficientMaterials.Add($"MaterialID:{req.Key}");
                }
            }
            if (insufficientMaterials.Count > 0)
            {
                return new ValidationError(
                    ErrorCode.E0041,
                    insufficientMaterials.ToArray()
                );
            }
            return null;
        }
    }
}
