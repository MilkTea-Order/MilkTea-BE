using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Models.Errors;
using MilkTea.Application.Models.Orders;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;

namespace MilkTea.Application.Services.Orders
{
    public class OrderItemValidator(
                            IMenuRepository menuRepository,
                            IPriceListRepository priceRepository,
                            IWarehouseRepository warehouseRepository,
                            IStatusRepository statusRepository,
                            ISizeRepository sizeRepository)
    {
        private readonly IMenuRepository _vMenuRepository = menuRepository;
        private readonly IPriceListRepository _vPriceRepository = priceRepository;
        private readonly IWarehouseRepository _vWarehouseRepository = warehouseRepository;
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly ISizeRepository _vSizeRepository = sizeRepository;
        public async Task<OrderItemValidation> Validate(OrderItemCommand item, int priceListId)
        {
            var result = new OrderItemValidation(item);
            var statusAvailable = await _vStatusRepository.GetActive();
            if (statusAvailable == null)
            {
                return result.SetError(ValidationError.SendError(ErrorCode.E0001, "Avaliable Status"));
            }
            // Check MenuID
            if (item.MenuID <= 0)
            {
                return result.SetError(ValidationError.InvalidData(nameof(item.MenuID)));

            }
            var menu = await _vMenuRepository.GetMenuByIDAsync(item.MenuID);
            if (menu == null)
            {
                return result.SetError(ValidationError.NotExist(nameof(item.MenuID)));

            }
            if (menu.StatusID != statusAvailable.ID)
            {
                return result.SetError(ValidationError.SendError(ErrorCode.E0036, nameof(item.MenuID)));

            }
            // Check SizeID
            var price = await _vPriceRepository.GetPriceAsync(priceListId, item.MenuID, item.SizeID);
            if (price == null)
            {
                return result.SetError(ValidationError.SendError(ErrorCode.E0036, nameof(item.SizeID)));
            }

            //Check Quantity
            if (item.Quantity <= 0)
            {
                return result.SetError(ValidationError.InvalidData(nameof(item.Quantity)));
            }

            // Check ToppingIDs && KindOfHotpotIDs

            // Check Recipe
            var recipe = await _vWarehouseRepository.GetRecipeAsync(item.MenuID, item.SizeID);

            if (recipe == null || recipe.Count == 0)
                return result.SetError(ValidationError.SendError(ErrorCode.E0001, nameof(item.SizeID), nameof(item.MenuID)));

            // Get info size
            var size = await _vSizeRepository.GetSizeByIdAsync(item.SizeID);

            return result.SetSuccess(menu, size!, price.Value, recipe);

        }
    }
}


