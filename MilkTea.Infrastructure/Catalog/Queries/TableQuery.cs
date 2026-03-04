using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Dtos;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Catalog.Queries
{
    public class TableQuery(AppDbContext context) : ITableQuery
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<TableDto>> GetTableAsync(int? statusID, CancellationToken cancellationToken)
        {
            var query = _vContext.Tables.AsNoTracking();

            if (statusID is int status)
                query = query.Where(x => (int)x.Status == status);

            return await query
                .Select(x => new TableDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Position = x.Position,
                    NumberOfSeats = x.NumberOfSeats,
                    StatusId = (int)x.Status,
                    StatusName = x.Status.GetDescription(),
                    Note = x.Note,
                    EmptyImg = x.EmptyPicture != null
                                                ? $"data:image/png;base64,{Convert.ToBase64String(x.EmptyPicture)}"
                                                : null,
                    UsingImg = x.UsingPicture != null
                                                ? $"data:image/png;base64,{Convert.ToBase64String(x.UsingPicture)}"
                                                : null
                })
                 .ToListAsync(cancellationToken);
        }
    }
}
