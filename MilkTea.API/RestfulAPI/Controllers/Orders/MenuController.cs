using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Queries.Orders;
using MilkTea.Application.UseCases.Orders;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/menus")]
    public class MenuController(GetGroupMenuUseCase getGroupMenuUseCase,
                                GetMenuItemsOfGroupUseCase getItemsOfGroupMenuUseCase,
                                GetMenuSizeOfMenuUseCase getMenuSizeOfMenuUseCase,
                                GetGroupMenuAvaliableUseCase getGroupMenuAvaliableUseCase,
                                GetMenuItemsAvaliableOfGroupUseCase getMenuItemsAvaliableOfGroupUseCase) : BaseController
    {
        [HttpGet("groups")]
        public async Task<ResponseDto> GetGroupMenu([FromQuery] int? statusID, [FromQuery] int? itemStatus)
        {
            var vData = await getGroupMenuUseCase.Execute(new GetGroupMenuQuery { StatusId = statusID, ItemStatusId = itemStatus });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.GroupMenu);
        }

        [HttpGet("groups/avaliable")]
        public async Task<ResponseDto> GetGroupMenuAvaliable()
        {
            var vData = await getGroupMenuAvaliableUseCase.Execute();
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.GroupMenu);
        }


        [HttpGet("groups/{groupID:int}/items")]
        public async Task<ResponseDto> GetMenuItemOfGroup([FromRoute] int groupID, [FromQuery] int? menuItemStatus)
        {
            var vData = await getItemsOfGroupMenuUseCase.Execute(new GetMenuItemsOfGroupQuery { GroupId = groupID, MenuStatusId = menuItemStatus });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.Menus);
        }

        [HttpGet("groups/{groupID:int}/items/avaliable")]
        public async Task<ResponseDto> GetMenuItemAvaliableOfGroup([FromRoute] int groupID)
        {
            var vData = await getMenuItemsAvaliableOfGroupUseCase.Execute(new GetMenuItemsAvailableOfGroupQuery { GroupId = groupID });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.Menus);
        }


        [HttpGet("{menuID:int}/sizes")]
        public async Task<ResponseDto> GetMenuSizeOfMenu([FromRoute] int menuID)
        {
            var vData = await getMenuSizeOfMenuUseCase.Execute(new GetMenuSizeOfMenuQuery { MenuId = menuID });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.MenuSize);
        }
    }
}
