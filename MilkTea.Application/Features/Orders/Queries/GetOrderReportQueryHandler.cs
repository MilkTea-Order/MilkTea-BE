using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Features.Orders.Queries
{

    public sealed class GetOrderReportQuery : IRequest<GetOrderReportResult>
    {
        public string? PaymentMethod { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public sealed class GetOrderReportQueryValidator : AbstractValidator<GetOrderReportQuery>
    {
        public GetOrderReportQueryValidator()
        {
            RuleFor(x => x.OrderStatusId)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetOrderReportQuery.OrderStatusId));

            RuleFor(x => x.PaymentMethod)
                .Must(x => PaymentMethod.All.Contains(x!))
                .When(x => !string.IsNullOrWhiteSpace(x.PaymentMethod))
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetOrderReportQuery.PaymentMethod));

            RuleFor(x => x.FromDate)
                .NotNull()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetOrderReportQuery.FromDate));

            RuleFor(x => x.ToDate)
                .NotNull()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetOrderReportQuery.ToDate));

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetOrderReportQuery.FromDate) + nameof(GetOrderReportQuery.ToDate));
        }
    }


    public class GetOrderReportQueryHandler(IOrderQuery orderQuerys, IIdentifyServicePorts currentUser, ITableService tableService) : IRequestHandler<GetOrderReportQuery, GetOrderReportResult>
    {
        private readonly IOrderQuery _vOrderQuery = orderQuerys;
        private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
        private readonly ITableService _vTableService = tableService;
        public async Task<GetOrderReportResult> Handle(GetOrderReportQuery query, CancellationToken cancellationToken)
        {
            GetOrderReportResult result = new();

            if (query.OrderStatusId != (int)OrderStatus.NotCollected && query.OrderStatusId != (int)OrderStatus.Paid)
            {
                return SendError(result, ErrorCode.E0036, nameof(GetOrderReportQuery.OrderStatusId));
            }

            var reports = await _vOrderQuery.GetOrderReportAsync(_vCurrentUser.UserId,
                                                                (OrderStatus)query.OrderStatusId,
                                                                query.FromDate, query.ToDate, query.PaymentMethod, cancellationToken);

            //var tableIds = reports.Orders.Select(o => o.DinnerTableId).Distinct().ToList();
            var tableIds = reports.DateGroup.SelectMany(g => g.Orders).Select(o => o.DinnerTableId).Distinct().ToList();
            var table = await _vTableService.GetTableAsync(tableIds, cancellationToken);

            var tableDict = table.ToDictionary(x => x.Id);

            foreach (var group in reports.DateGroup)
            {
                foreach (var o in group.Orders)
                {
                    if (tableDict.TryGetValue(o.DinnerTableId, out var t))
                    {
                        o.DinnerTable = new TableDto
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Code = t.Code,
                            Position = t.Position,
                            NumberOfSeats = t.NumberOfSeats,
                            StatusId = t.StatusId,
                            StatusName = t.StatusName,
                            Note = t.Note,
                            UsingImg = t.UsingImg,
                            EmptyImg = t.EmptyImg,
                        };
                    }
                }
            }

            result.Static = reports;
            return result;
        }
        private static GetOrderReportResult SendError(GetOrderReportResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}


//foreach (var o in reports.DateGroup)
//{
//    if (tableDict.TryGetValue(o.DinnerTableId, out var t))
//    {
//        o.DinnerTable = new TableDto
//        {
//            Id = t.Id,
//            Name = t.Name,
//            Code = t.Code,
//            Position = t.Position,
//            NumberOfSeats = t.NumberOfSeats,
//            StatusId = t.StatusId,
//            StatusName = t.StatusName,
//            Note = t.Note,
//            UsingImg = t.UsingImg,
//            EmptyImg = t.EmptyImg,
//        };
//    }
//}