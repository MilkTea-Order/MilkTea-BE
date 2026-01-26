using MediatR;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Features.TableManagement.Results;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Catalog.Enums;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.Features.TableManagement.Queries;

public sealed class GetTableByStatusQueryHandler(
    IDinnerTableRepository dinnerTableRepository) : IRequestHandler<GetTableByStatusQuery, GetTableByStatusResult>
{
    public async Task<GetTableByStatusResult> Handle(GetTableByStatusQuery query, CancellationToken cancellationToken)
    {
        var result = new GetTableByStatusResult();
        result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);

        List<Domain.Catalog.Entities.DinnerTable> tables;

        if (query.StatusId.HasValue)
        {
            if (query.StatusId <= 0)
                return SendError(result, ErrorCode.E0029, "StatusID");

            // Convert StatusId to enum
            if (!Enum.IsDefined(typeof(DinnerTableStatus), query.StatusId.Value))
                return SendError(result, ErrorCode.E0001, "StatusID");

            var status = (DinnerTableStatus)query.StatusId.Value;
            tables = await dinnerTableRepository.GetByStatusAsync(status);
        }
        else
        {
            tables = await dinnerTableRepository.GetAllAsync();
        }
        
        result.Tables = tables.Select(t => new DinnerTableDto
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
