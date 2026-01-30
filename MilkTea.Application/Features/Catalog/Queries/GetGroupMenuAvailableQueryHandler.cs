using MediatR;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Enums;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuAvailableQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetGroupMenuAvailableQuery, GetGroupMenuResult>
{
    public async Task<GetGroupMenuResult> Handle(GetGroupMenuAvailableQuery query, CancellationToken cancellationToken)
    {
        var result = new GetGroupMenuResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        var groups = await unitOfWork.Menus.GetByStatusWithMenuAsync((int)CommonStatus.Active, (int)MenuStatus.Active, cancellationToken);

        result.GroupMenu = groups.Select(g => new MenuGroupDto
        {
            MenuGroupId = g.Id,
            MenuGroupName = g.Name,
            StatusId = (int)g.Status,
            StatusName = g.Status.ToString(),
            Quantity = g.Menus.Count()
        }).ToList();
        return result;
    }
}
