using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Dtos.Table;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Features.Catalog.Queries
{
    public class TableQuery(AppDbContext context) : ITableQuery
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<TableDto>> GetTableAsync(int? statusId, CancellationToken cancellationToken)
        {
            var query = _vContext.Tables.AsNoTracking();

            if (statusId.HasValue)
            {
                query = query.Where(x => (int)x.Status == statusId.Value);
            }

            return await query
                .Select(x => new TableDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Position = x.Position,
                    NumberOfSeats = x.NumberOfSeats,
                    Note = x.Note,
                    Status = new StatusDto()
                    {
                        Id = (int)x.Status,
                        Name = x.Status.GetDescription()
                    },
                    EmptyImg = $"data:image/png;base64,{Convert.ToBase64String(x.EmptyPicture)}",
                    UsingImg = $"data:image/png;base64,{Convert.ToBase64String(x.UsingPicture)}"
                })
                 .ToListAsync(cancellationToken);
        }
    }
}
