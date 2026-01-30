using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Catalog.Queries;

namespace MilkTea.API.RestfulAPI.Controllers.Catalog
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController(ISender sender) : BaseController
    {
        private readonly ISender _vSender = sender;

        [HttpGet("menus/groups")]
        public async Task<ResponseDto> GetGroupMenu([FromQuery] int? statusID, [FromQuery] int? itemStatus)
        {
            var query = new GetGroupMenuQuery { StatusId = statusID, ItemStatusId = itemStatus };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(result.GroupMenu);
        }

        [HttpGet("menus/groups/available")]
        public async Task<ResponseDto> GetGroupMenuAvailable()
        {
            var query = new GetGroupMenuAvailableQuery();
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(result.GroupMenu);
        }

        [HttpGet("menus/groups/{groupID:int}/items")]
        public async Task<ResponseDto> GetMenuItemOfGroup([FromRoute] int groupID, [FromQuery] int? menuItemStatus)
        {
            var query = new GetMenuItemsOfGroupQuery { GroupId = groupID, MenuStatusId = menuItemStatus };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(result.Menus);
        }

        [HttpGet("menus/groups/{groupID:int}/items/avaliable")]
        public async Task<ResponseDto> GetMenuItemAvaliableOfGroup([FromRoute] int groupID)
        {
            var query = new GetMenuItemsAvailableOfGroupQuery { GroupId = groupID };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(result.Menus);
        }

        [HttpGet("menus/{menuID:int}/sizes")]
        public async Task<ResponseDto> GetMenuSizeOfMenu([FromRoute] int menuID)
        {
            var query = new GetMenuSizeOfMenuQuery { MenuId = menuID };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(result.MenuSize);
        }
    }
}
