using MediatR;
using MilkTea.Application.Features.Catalog.Queries;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.Catalog.Repositories;

namespace MilkTea.Application.Features.Catalog.Handlers;

public sealed class GetTableEmptyQueryHandler(
    ICatalogUnitOfWork catalogUnitOfWork) : IRequestHandler<GetTableEmptyQuery, GetTableEmptyResult>
{
    private readonly ICatalogUnitOfWork _vCatalogUnitOfWork = catalogUnitOfWork;
    public async Task<GetTableEmptyResult> Handle(GetTableEmptyQuery query, CancellationToken cancellationToken)
    {
        var result = new GetTableEmptyResult();
        var tables = await _vCatalogUnitOfWork.Tables.GetTableEmptyAsync(query.IsEmpty, cancellationToken);
        result.Tables = tables.Select(t => new TableDto
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
