using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Catalog.Responses;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Catalog.Queries;

namespace MilkTea.API.RestfulAPI.Controllers.Catalog
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController(ISender sender, IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [HttpGet("menus/groups/available")]
        public async Task<ResponseDto> GetGroupMenuAvailable()
        {
            var query = new GetGroupMenuAvailableQuery();
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var response = _vMapper.Map<List<GetGroupMenuAvailableResponseDto>>(result.GroupMenu);
            return SendSuccess(response);
        }

        [HttpGet("menus/groups/{groupID:int}/items")]
        public async Task<ResponseDto> GetMenuItemOfGroup([FromRoute] int groupID, [FromQuery] bool isMenuActive = true)
        {
            var query = new GetMenuItemsOfGroupQuery { GroupId = groupID, IsMenuActive = isMenuActive };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var response = _vMapper.Map<List<GetMenuItemOfGroupResponseDto>>(result.Menus);

            return SendSuccess(response);
        }


        [HttpGet("menus/groups/{groupID:int}/items/available")]
        public async Task<ResponseDto> GetMenuItemAvailableOfGroup([FromRoute] int groupID)
        {
            var query = new GetMenuItemsAvailableOfGroupQuery { GroupId = groupID };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var response = _vMapper.Map<List<GetMenuItemAvailableOfGroupResponseDto>>(result.Menus);
            return SendSuccess(response);
        }

        [HttpGet("menus/items/available")]
        public async Task<ResponseDto> GetMenuItemAvailableByGroupAndMenuName([FromQuery] int? groupID, [FromQuery] string? menuName)
        {
            var query = new GetMenuItemAvailableByGroupAndMenuNameQuery { GroupId = groupID, MenuName = menuName };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var response = _vMapper.Map<List<GetMenuItemAvailableOfGroupAndNameResponseDto>>(result.Menus);
            return SendSuccess(response);
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
            var response = _vMapper.Map<List<GetMenuSizeOfMenuResponseDto>>(result.MenuSize);
            return SendSuccess(response);
        }
    }
}

//[HttpGet("menus/groups")]
//public async Task<ResponseDto> GetGroupMenu([FromQuery] int? statusID, [FromQuery] int? itemStatus)
//{
//    var query = new GetGroupMenuQuery { StatusId = statusID, ItemStatusId = itemStatus };
//    var result = await _vSender.Send(query);

//    if (result.ResultData.HasData)
//    {
//        return SendError(result.ResultData);
//    }
//    return SendSuccess(result.GroupMenu);
//}