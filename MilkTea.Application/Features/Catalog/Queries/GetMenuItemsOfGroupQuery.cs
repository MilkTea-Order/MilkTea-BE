using MediatR;
using MilkTea.Application.Features.Catalog.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetMenuItemsOfGroupQuery : IRequest<GetMenuItemsOfGroupResult>
{
    public int GroupId { get; set; }
    public int? MenuStatusId { get; set; }
}
