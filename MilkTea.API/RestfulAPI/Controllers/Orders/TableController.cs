using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Queries.Orders;
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
            var vData = await getTableByStatusUseCase.Execute(new GetTableByStatusQuery { StatusId = statusID });
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
            // Temporary mapping; will be replaced with Application DTO -> API Response mapping.
            var result = _mapper.Map<List<GetTableEmptyResponseDto>>(vData.Tables);
            return SendSuccess(result);
        }
    }
}
