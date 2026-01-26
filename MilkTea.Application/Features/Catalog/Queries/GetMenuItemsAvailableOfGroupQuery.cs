using MediatR;
using MilkTea.Application.Features.Catalog.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetMenuItemsAvailableOfGroupQuery : IRequest<GetMenuItemsOfGroupResult>
{
    public int GroupId { get; set; }
}
