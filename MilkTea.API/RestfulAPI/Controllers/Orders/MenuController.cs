using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Orders;
using MilkTea.Application.UseCases.Orders;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController(GetGroupMenuUseCase getGroupMenuUseCase,
                                GetMenuItemsOfGroupUseCase getItemsOfGroupMenuUseCase,
                                GetMenuSizeOfMenuUseCase getMenuSizeOfMenuUseCase,
                                GetGroupMenuAvaliableUseCase getGroupMenuAvaliableUseCase,
                                GetMenuItemsAvaliableOfGroupUseCase getMenuItemsAvaliableOfGroupUseCase) : BaseController
    {
        [HttpGet("group")]
        public async Task<ResponseDto> GetGroupMenu([FromQuery] int? statusID, [FromQuery] int? itemStatus)
        {
            var vData = await getGroupMenuUseCase.Execute(new GetGroupMenuCommnad { StatusID = statusID, ItemStatusID = itemStatus });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.GroupMenu);
        }

        [HttpGet("group/avaliable")]
        public async Task<ResponseDto> GetGroupMenuAvaliable()
        {
            var vData = await getGroupMenuAvaliableUseCase.Execute();
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.GroupMenu);
        }


        [HttpGet("group/{groupID}/items")]
        public async Task<ResponseDto> GetMenuItemOfGroup([FromRoute] int groupID, [FromQuery] int? menuItemStatus)
        {
            var vData = await getItemsOfGroupMenuUseCase.Execute(new GetMenuItemsOfGroupCommand { GroupID = groupID, MenuStatusID = menuItemStatus });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.Menus);
        }

        [HttpGet("group/{groupID}/items/avaliable")]
        public async Task<ResponseDto> GetMenuItemAvaliableOfGroup([FromRoute] int groupID)
        {
            var vData = await getMenuItemsAvaliableOfGroupUseCase.Execute(new GetMenuItemsAvaliableOfGroupCommand { GroupID = groupID });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.Menus);
        }


        [HttpGet("{menuID}")]
        public async Task<ResponseDto> GetMenuSizeOfMenu([FromRoute] int menuID)
        {
            var vData = await getMenuSizeOfMenuUseCase.Execute(new GetMenuSizeOfMenuCommand { MenuID = menuID });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.MenuSize);
        }
    }
}
