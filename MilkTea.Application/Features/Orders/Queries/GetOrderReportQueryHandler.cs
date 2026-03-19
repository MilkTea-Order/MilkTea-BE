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
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public sealed class GetOrderReportQueryValidator : AbstractValidator<GetOrderReportQuery>
    {
        public GetOrderReportQueryValidator()
        {
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


    public class GetOrderReportQueryHandler(IOrderQuery orderQuerys, ICurrentUser currentUser, ITableService tableService) : IRequestHandler<GetOrderReportQuery, GetOrderReportResult>
    {
        private readonly IOrderQuery _vOrderQuery = orderQuerys;
        private readonly ICurrentUser _vCurrentUser = currentUser;
        private readonly ITableService _vTableService = tableService;
        public async Task<GetOrderReportResult> Handle(GetOrderReportQuery query, CancellationToken cancellationToken)
        {
            GetOrderReportResult result = new();

            var reports = await _vOrderQuery.GetOrderReportAsync(_vCurrentUser.UserId,
                                                                query.FromDate, query.ToDate, query.PaymentMethod, cancellationToken);

            var tableIds = reports.Orders.Select(o => o.DinnerTableId).Distinct().ToList();
            var table = await _vTableService.GetTableAsync(tableIds, cancellationToken);
            var tableDict = table.ToDictionary(x => x.Id);

            foreach (var o in reports.Orders)
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

            result.Static = reports;
            return result;
        }
    }
}
