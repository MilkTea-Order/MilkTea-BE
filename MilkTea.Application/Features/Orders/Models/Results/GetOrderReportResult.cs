using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Orders.Models.Results
{
    public class GetOrderReportResult
    {
        public StringListEntry ResultData { get; set; } = new();
        public ReportOrderDto Static { get; set; } = new();
    }
}
