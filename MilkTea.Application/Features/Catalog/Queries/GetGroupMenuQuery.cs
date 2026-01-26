using MediatR;
using MilkTea.Application.Features.Catalog.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuQuery : IRequest<GetGroupMenuResult>
{
    public int? StatusId { get; set; }
    public int? ItemStatusId { get; set; }
}
