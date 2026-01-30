using MediatR;
using MilkTea.Application.Features.Catalog.Queries;
using MilkTea.Application.Features.Catalog.Results;
using MilkTea.Application.Models.Catalog;
using MilkTea.Domain.SharedKernel.Repositories;

namespace MilkTea.Application.Features.Catalog.Handlers;

public sealed class GetTableEmptyQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetTableEmptyQuery, GetTableEmptyResult>
{
    private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
    public async Task<GetTableEmptyResult> Handle(GetTableEmptyQuery query, CancellationToken cancellationToken)
    {
        var result = new GetTableEmptyResult();
        var tables = await _vUnitOfWork.Tables.GetTableAsync(query.IsEmpty, cancellationToken);
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
