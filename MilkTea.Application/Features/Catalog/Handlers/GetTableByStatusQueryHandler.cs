using MediatR;
using MilkTea.Application.Features.Catalog.Queries;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
namespace MilkTea.Application.Features.Catalog.Handlers;

public sealed class GetTableByStatusQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetTableByStatusQuery, GetTableByStatusResult>
{
    public async Task<GetTableByStatusResult> Handle(GetTableByStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetTableByStatusResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        List<Domain.Catalog.Entities.TableEntity> tables;

        if (query.StatusId.HasValue)
        {
            if (query.StatusId <= 0)
                return SendError(result, ErrorCode.E0029, "StatusID");

            // Convert StatusId to enum
            if (!Enum.IsDefined(typeof(TableStatus), query.StatusId.Value))
                return SendError(result, ErrorCode.E0001, "StatusID");

            var status = (TableStatus)query.StatusId.Value;
            tables = await unitOfWork.Tables.GetByStatusAsync(status);
        }
        else
        {
            tables = await unitOfWork.Tables.GetAllAsync();
        }

        result.Tables = tables.Select(t => new TableDto
        {
            Id = t.Id,
            Code = t.Code,
            Name = t.Name,
            Position = t.Position,
            NumberOfSeats = t.NumberOfSeats,
            StatusId = (int)t.Status,
            StatusName = t.Status.ToString(),
            Note = t.Note
        }).ToList();

        return result;
    }

    private static GetTableByStatusResult SendError(GetTableByStatusResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
