using MediatR;
using MilkTea.Application.Features.Catalog.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetTableByStatusQuery : IRequest<GetTableByStatusResult>
{
    public int? StatusId { get; set; }
}
