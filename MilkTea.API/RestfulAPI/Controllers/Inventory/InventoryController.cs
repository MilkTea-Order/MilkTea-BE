using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Inventory.Queries;
namespace MilkTea.API.RestfulAPI.Controllers.Inventory
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController(ISender sender,
                                        IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [Authorize]
        [HttpGet("report")]
        public async Task<ResponseDto> GetInventorySummary([FromQuery] string? materialName)
        {
            var query = new GetInventorySummaryQuery { MaterialName = materialName };
            var result = await _vSender.Send(query);
            if (result.ResultData.HasData) return SendError(result.ResultData);
            return SendSuccess(result.Materials);
        }
    }
}
