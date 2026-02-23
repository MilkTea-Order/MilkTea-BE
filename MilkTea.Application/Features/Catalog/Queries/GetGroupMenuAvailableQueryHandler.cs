using MediatR;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Shared.Domain.Constants;
using Shared.Extensions;
namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuAvailableQuery : IRequest<GetGroupMenuResult>;

public sealed class GetGroupMenuAvailableQueryHandler(
    ICatalogUnitOfWork catalogUnitOfWork) : IRequestHandler<GetGroupMenuAvailableQuery, GetGroupMenuResult>
{
    public async Task<GetGroupMenuResult> Handle(GetGroupMenuAvailableQuery query, CancellationToken cancellationToken)
    {
        var result = new GetGroupMenuResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        var groups = await catalogUnitOfWork.Menus.GetByStatusWithMenuAsync((int)CommonStatus.Active, (int)MenuStatus.Active, cancellationToken);

        result.GroupMenu = groups.Select(g => new MenuGroupDto
        {
            MenuGroupId = g.Id,
            MenuGroupName = g.Name,
            StatusId = (int)g.Status,
            StatusName = g.Status.GetDescription(),
            Quantity = g.Menus.Count()
        }).ToList();
        return result;
    }
}
