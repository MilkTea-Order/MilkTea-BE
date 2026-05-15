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

        [HttpGet("menus/groups/{groupId:int}/items")]
        public async Task<ResponseDto> GetMenuItemOfGroup([FromRoute] int groupId, [FromQuery] bool isMenuActive = true)
        {
            var query = new GetMenuItemsOfGroupQuery { GroupId = groupId, IsMenuActive = isMenuActive };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var response = _vMapper.Map<List<GetMenuItemOfGroupResponseDto>>(result.Menus);

            return SendSuccess(response);
        }


        [HttpGet("menus/groups/{groupId:int}/items/available")]
        public async Task<ResponseDto> GetMenuItemAvailableOfGroup([FromRoute] int groupId)
        {
            var query = new GetMenuItemsAvailableOfGroupQuery { GroupId = groupId };
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
