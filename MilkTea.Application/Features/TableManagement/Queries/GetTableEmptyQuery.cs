using MediatR;
using MilkTea.Application.Features.TableManagement.Results;

namespace MilkTea.Application.Features.TableManagement.Queries;

public sealed class GetTableEmptyQuery : IRequest<GetTableEmptyResult>
{
}
