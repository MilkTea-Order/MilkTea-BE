using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Features.Finance.Queries;

namespace MilkTea.API.RestfulAPI.Controllers.Finance
{
    [ApiController]
    [Route("api/finance")]
    public class FinanceController(ISender sender,
                                        IMapper mapper) : BaseController
    {
        private readonly ISender _vSender = sender;
        private readonly IMapper _vMapper = mapper;

        [Authorize]
        [HttpGet("report")]
        public async Task<ResponseDto> GetFinanceSummary([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var query = new GetSummaryCollectAndSpend
            {
                FromDate = fromDate,
                ToDate = toDate
            };
            var result = await _vSender.Send(query);
            if (result.ResultData.HasData) return SendError(result.ResultData);
            return SendSuccess(result.Summary);
        }
    }
}


