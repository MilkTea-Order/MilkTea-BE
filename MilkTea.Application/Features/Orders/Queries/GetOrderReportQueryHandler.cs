using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Shared.Extensions;

namespace MilkTea.Application.Features.Orders.Queries
{

    public sealed class GetOrderReportQuery : IRequest<GetOrderReportResult>
    {
        public string? PaymentMethod { get; init; }
        public string? Status { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
    }

    public sealed class GetOrderReportQueryValidator : AbstractValidator<GetOrderReportQuery>
    {
        public GetOrderReportQueryValidator()
        {
            RuleFor(x => x.Status)
                .Must(x => x.TryParseEnum<OrderStatus>(out _))
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.Status));

            RuleFor(x => x.Status)
                .Must(status =>
                {
                    status.TryParseEnum<OrderStatus>(out var parsedStatus);
                    return parsedStatus is OrderStatus.NotCollected or OrderStatus.Paid;
                })
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.Status))
                .When(x => x.Status.TryParseEnum<OrderStatus>(out _));
            
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

            

            var reports = await _vOrderQuery.GetOrderReportAsync(_vCurrentUser.UserId, 
                                                                      Enum.Parse<OrderStatus>(query.Status, ignoreCase: true),
                                                                                query.FromDate, query.ToDate, query.PaymentMethod, cancellationToken);

            var tableIds = reports.DateGroup.SelectMany(g => g.Orders).Select(o => o.DinnerTable!.Id).Distinct().ToList();
            var table = await _vTableService.GetTableAsync(tableIds, cancellationToken);

            var tableDict = table.ToDictionary(x => x.Id);

            foreach (var group in reports.DateGroup)
            {
                foreach (var o in group.Orders)
                {
                    if (o.DinnerTable != null && tableDict.TryGetValue(o.DinnerTable.Id, out var t))
                    {
                        o.DinnerTable = new TableDto
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Code = t.Code,
                            Position = t.Position,
                            NumberOfSeats = t.NumberOfSeats,
                            StatusId = t.Status.Id,
                            StatusName = t.Status.Name,
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