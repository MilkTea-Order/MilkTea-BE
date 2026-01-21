using Microsoft.AspNetCore.Mvc;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Application.Commands.Orders;
using MilkTea.Application.UseCases.Orders;

namespace MilkTea.API.RestfulAPI.Controllers.Orders
{
    [ApiController]
    [Route("api/tables")]
    public class TableController(GetTableByStatusUseCase getTableByStatusUseCase) : BaseController
    {
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
    }
}
