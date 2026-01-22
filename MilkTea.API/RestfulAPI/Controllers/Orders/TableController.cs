using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Orders;
using MilkTea.Application.UseCases.Orders;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/tables")]
    public class TableController(GetTableByStatusUseCase getTableByStatusUseCase, GetTableEmptyUseCase getTableEmptyUseCase, IMapper mapper) : BaseController
    {
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ResponseDto> GetTableByStatus([FromQuery] int? statusID)
        {
            var vData = await getTableByStatusUseCase.Execute(new GetTableByStatusCommand { statusID = statusID });
            if (vData.ResultData.HasData)
            {
                return SendError(vData.ResultData);
            }
            return SendSuccess(vData.Tables);
        }

        [HttpGet("empty")]
        public async Task<ResponseDto> GetTableEmpty()
        {
            var vData = await getTableEmptyUseCase.Execute();
            var result = _mapper.Map<List<GetTableEmptyResponseDto>>(vData);
            return SendSuccess(result);
        }
    }
}
