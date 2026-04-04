using FluentValidation;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Dtos;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Enums;
using Shared.Abstractions.CQRS;
namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQuery : IQuery<GetOrdersByOrderByAndStatusResult>
{
    public int StatusId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
public sealed class GetOrdersByOrderByAndStatusQueryValidator : AbstractValidator<GetOrdersByOrderByAndStatusQuery>
{
    public GetOrdersByOrderByAndStatusQueryValidator()
    {
        // Status must be a valid enum value
        RuleFor(x => x.StatusId)
            .Must(x => Enum.IsDefined(typeof(OrderStatus), x))
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.StatusId));

        // If one of the dates is provided, the other must also be provided
        RuleFor(x => x.FromDate)
            .NotNull()
            .When(x => x.ToDate.HasValue)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.FromDate));

        RuleFor(x => x.ToDate)
            .NotNull()
            .When(x => x.FromDate.HasValue)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.ToDate));

        // Dates cannot be in the future
        RuleFor(x => x.ToDate)
            .Must(toDate => !toDate.HasValue || toDate.Value.Date <= DateTime.Today)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.ToDate));

        // FromDate cannot be after ToDate
        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.FromDate) + nameof(GetOrdersByOrderByAndStatusQuery.ToDate));

    }
}

public sealed class GetOrdersByOrderByAndStatusQueryHandler(IIdentifyServicePorts currentUser,
                                                                IOrderQuery orderQuery,
                                                                ITableService tableService) : IQueryHandler<GetOrdersByOrderByAndStatusQuery, GetOrdersByOrderByAndStatusResult>
{
    private readonly IOrderQuery _vOrderQuery = orderQuery;
    private readonly ITableService _vTableService = tableService;
    public async Task<GetOrdersByOrderByAndStatusResult> Handle(GetOrdersByOrderByAndStatusQuery query, CancellationToken cancellationToken)
    {
        GetOrdersByOrderByAndStatusResult result = new();
        var orders = await _vOrderQuery.GetOrdersAsync(currentUser.UserId, (OrderStatus)query.StatusId, query.FromDate, query.ToDate, cancellationToken);
        var tableIds = orders.Select(o => o.DinnerTableId).Distinct().ToList();

        var table = await _vTableService.GetTableAsync(tableIds, cancellationToken);
        var tableDict = table.ToDictionary(x => x.Id);

        foreach (var o in orders)
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
        result.Orders = orders;
        return result;
    }
    private static GetOrdersByOrderByAndStatusResult SendError(GetOrdersByOrderByAndStatusResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
