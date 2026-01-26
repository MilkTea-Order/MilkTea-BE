using MediatR;
using MilkTea.Application.DTOs.Orders;
using MilkTea.Application.Features.TableManagement.Results;
using MilkTea.Domain.Catalog.Repositories;

namespace MilkTea.Application.Features.TableManagement.Queries;

public sealed class GetTableEmptyQueryHandler(
    IDinnerTableRepository dinnerTableRepository) : IRequestHandler<GetTableEmptyQuery, GetTableEmptyResult>
{
    public async Task<GetTableEmptyResult> Handle(GetTableEmptyQuery query, CancellationToken cancellationToken)
    {
        var result = new GetTableEmptyResult();
        var tables = await dinnerTableRepository.GetEmptyTablesAsync();

        result.Tables = tables.Select(t => new DinnerTableDto
        {
            Id = t.Id,
            Code = t.Code,
            Name = t.Name,
            Position = t.Position,
            NumberOfSeats = t.NumberOfSeats,
            StatusId = (int)t.Status,
            StatusName = t.Status.ToString(),
            Note = t.Note,
            Img = t.EmptyPicture != null
                ? $"data:image/png;base64,{Convert.ToBase64String(t.EmptyPicture)}"
                : null
        }).ToList();

        return result;
    }
}
