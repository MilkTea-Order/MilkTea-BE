using FluentValidation;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Enums;
using MilkTea.Shared.Extensions;
using Shared.Abstractions.CQRS;
namespace MilkTea.Application.Features.Orders.Queries;

public sealed class GetOrdersByOrderByAndStatusQuery : IQuery<GetOrdersByOrderByAndStatusResult>
{
    public required string Status { get; init; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}
public sealed class GetOrdersByOrderByAndStatusQueryValidator : AbstractValidator<GetOrdersByOrderByAndStatusQuery>
{
    public GetOrdersByOrderByAndStatusQueryValidator()
    {
        // Status must be a valid enum value
        RuleFor(x => x.Status)
            .Must(x => x.TryParseEnum<OrderStatus>(out _))
            .WithErrorCode(ErrorCode.E0036)
            .OverridePropertyName(nameof(GetOrdersByOrderByAndStatusQuery.Status));
        
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
        var orders = await _vOrderQuery.GetOrdersAsync(currentUser.UserId, 
                                                                Enum.Parse<OrderStatus>(query.Status, ignoreCase: true), 
                                                                      query.FromDate, query.ToDate, cancellationToken);
        var tableIds = orders.Select(o => o.DinnerTable!.Id).Distinct().ToList();

        var table = await _vTableService.GetTableAsync(tableIds, cancellationToken);
        var tableDict = table.ToDictionary(x => x.Id);

        foreach (var o in orders)
        {
            if (o.DinnerTable != null && tableDict.TryGetValue(o.DinnerTable.Id, out var t))
            {
                o.DinnerTable.Code = t.Code;
                o.DinnerTable.Name = t.Name;
                o.DinnerTable.Position = t.Position;
                o.DinnerTable.NumberOfSeats = t.NumberOfSeats;
                o.DinnerTable.StatusId = t.Status.Id;
                o.DinnerTable.StatusName = t.Status.Name;
                o.DinnerTable.Note = t.Note;
                o.DinnerTable.EmptyImg = t.EmptyImg;
                o.DinnerTable.UsingImg = t.UsingImg;
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
