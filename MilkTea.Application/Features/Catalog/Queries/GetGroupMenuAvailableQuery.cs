using MediatR;
using MilkTea.Application.Features.Catalog.Results;

namespace MilkTea.Application.Features.Catalog.Queries;

public sealed class GetGroupMenuAvailableQuery : IRequest<GetGroupMenuResult>
{
}
