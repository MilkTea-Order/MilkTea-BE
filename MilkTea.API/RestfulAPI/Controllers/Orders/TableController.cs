using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.TableManagement.Queries;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/tables")]
    public class TableController(ISender sender, IMapper mapper) : BaseController
    {
        private readonly ISender _sender = sender;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ResponseDto> GetTableByStatus([FromQuery] int? statusID)
        {
            var query = new GetTableByStatusQuery { StatusId = statusID };
            var result = await _sender.Send(query);

            if (result.ResultData.HasData)
            {
                return SendError(result.ResultData);
            }
            return SendSuccess(result.Tables);
        }

        [HttpGet("empty")]
        public async Task<ResponseDto> GetTableEmpty()
        {
            var query = new GetTableEmptyQuery();
            var result = await _sender.Send(query);

            var response = _mapper.Map<List<GetTableEmptyResponseDto>>(result.Tables);
            return SendSuccess(response);
        }
    }
}
