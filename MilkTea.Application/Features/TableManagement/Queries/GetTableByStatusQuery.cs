using MediatR;
using MilkTea.Application.Features.TableManagement.Results;

namespace MilkTea.Application.Features.TableManagement.Queries;

public sealed class GetTableByStatusQuery : IRequest<GetTableByStatusResult>
{
    public int? StatusId { get; set; }
}
