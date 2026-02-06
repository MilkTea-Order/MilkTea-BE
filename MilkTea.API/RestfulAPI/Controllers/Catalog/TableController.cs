using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Catalog.Responses;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Catalog.Queries;

namespace MilkTea.API.RestfulAPI.Controllers.Catalog
{
    [ApiController]
    [Route("api/catalog/tables")]
    public class TableController(ISender sender, IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [HttpGet]
        public async Task<ResponseDto> GetTableByStatus([FromQuery] int? statusID)
        {
            var query = new GetTableByStatusQuery { StatusId = statusID };
            var result = await _vSender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            var response = _vMapper.Map<List<DinnerTableDto>>(result.Tables);
            return SendSuccess(response);
        }

        [HttpGet("empty")]
        public async Task<ResponseDto> GetTableEmpty([FromQuery] bool isEmpty = true)
        {
            var query = new GetTableEmptyQuery { IsEmpty = isEmpty };
            var result = await _vSender.Send(query);

            var response = _vMapper.Map<List<GetTableEmptyResponseDto>>(result.Tables);
            return SendSuccess(response);
        }
    }
}
