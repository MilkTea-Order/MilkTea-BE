using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Dtos.Menu;
using MilkTea.Application.Features.Catalog.Models.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuAvailableQuery : IRequest<GetGroupMenuResult>;

public sealed class GetGroupMenuAvailableQueryHandler(ICatalogQuery catalogQuery)
                                        : IRequestHandler<GetGroupMenuAvailableQuery, GetGroupMenuResult>
{
    private readonly ICatalogQuery _vCatalogQuery = catalogQuery;
    public async Task<GetGroupMenuResult> Handle(GetGroupMenuAvailableQuery query, CancellationToken cancellationToken)
    {
        var groups = await _vCatalogQuery.GetGroupMenuAvailableAsync(cancellationToken);

        var result = new GetGroupMenuResult
        {
            GroupMenu = groups
        };
        return result;
    }
}
