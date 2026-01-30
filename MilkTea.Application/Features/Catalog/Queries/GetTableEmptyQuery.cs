using MediatR;
using MilkTea.Application.Features.Catalog.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetTableEmptyQuery : IRequest<GetTableEmptyResult>
{
    public bool IsEmpty { get; set; } = true;
}
