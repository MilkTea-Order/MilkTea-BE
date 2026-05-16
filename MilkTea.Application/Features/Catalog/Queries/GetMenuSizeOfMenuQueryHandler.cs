using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Results;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetMenuSizeOfMenuQuery : IRequest<GetMenuSizeOfMenuResult>
{
    public int MenuId { get; init; }
}

public sealed class GetMenuSizeOfMenuQueryHandler(ICatalogQuery catalogQuery)
    : IRequestHandler<GetMenuSizeOfMenuQuery, GetMenuSizeOfMenuResult>
{
    private readonly ICatalogQuery _vCatalogQuery = catalogQuery;

    public async Task<GetMenuSizeOfMenuResult> Handle(GetMenuSizeOfMenuQuery query, CancellationToken cancellationToken)
    {
        var result = new GetMenuSizeOfMenuResult();

        if (query.MenuId <= 0)
            return SendError(result, ErrorCode.E0036, nameof(query.MenuId));

        var menuSizes = await _vCatalogQuery.GetMenuSizesAsync(query.MenuId, cancellationToken);
        result.MenuSize = menuSizes;

        return result;
    }

    private static GetMenuSizeOfMenuResult SendError(GetMenuSizeOfMenuResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
