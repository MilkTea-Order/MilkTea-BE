using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Results;
using MilkTea.Application.Features.Orders.Abstractions.Services;
using MilkTea.Domain.Catalog;
using MilkTea.Domain.Catalog.Table.Enums;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetTableQuery : IRequest<GetTableResult>
{
    public bool IsEmpty { get; set; } = true;
}

public sealed class GetTableQueryHandler(ICatalogUnitOfWork catalogUnitOfWork,
                                            IOrderServices orderServices,
                                            ITableQuery tableQuery) : IRequestHandler<GetTableQuery, GetTableResult>
{
    private readonly ICatalogUnitOfWork _vCatalogUnitOfWork = catalogUnitOfWork;
    private readonly IOrderServices _vOrderServices = orderServices;
    private readonly ITableQuery _vTableQuery = tableQuery;
    public async Task<GetTableResult> Handle(GetTableQuery query, CancellationToken cancellationToken)
    {
        var result = new GetTableResult();
        var table = await _vTableQuery.GetTableAsync((int)TableStatus.InUsing, cancellationToken);
        if (table is null) return result;
        var tableIds = table.Select(t => t.Id).ToList();
        var tableAvailableIds = await _vOrderServices.GetTablesByAvailability(tableIds, query.IsEmpty, cancellationToken);
        result.Tables = table.Where(t => tableAvailableIds.Contains(t.Id)).ToList();
        return result;
    }
}
