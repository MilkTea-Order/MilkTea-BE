using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Catalog.Dtos;
using MilkTea.Domain.Catalog.Table.Enums;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Catalog.Services
{
    public class TableService(AppDbContext context) : ITableService
    {
        private readonly AppDbContext _vContext = context;

        public async Task<List<TableDto>> GetTableAsync(IReadOnlyCollection<int> tableIds, CancellationToken cancellationToken = default)
        {
            var rows = await _vContext.Tables.AsNoTracking()
                                                .Where(x => tableIds.Contains(x.Id))
                                                .Select(x => new
                                                {
                                                    x.Id,
                                                    x.Code,
                                                    x.Name,
                                                    x.Position,
                                                    x.NumberOfSeats,
                                                    StatusId = (int)x.Status,
                                                    x.Note,
                                                    x.EmptyPicture,
                                                    x.UsingPicture
                                                })
                                                .ToListAsync(cancellationToken);

            return rows.Select(x => new TableDto
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Position = x.Position,
                NumberOfSeats = x.NumberOfSeats,
                StatusId = x.StatusId,
                StatusName = ((TableStatus)x.StatusId).GetDescription(),
                Note = x.Note,
                EmptyImg = x.EmptyPicture != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(x.EmptyPicture)}"
                    : null,
                UsingImg = x.UsingPicture != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(x.UsingPicture)}"
                    : null
            }).ToList();
        }

        public async Task<bool> IsTableInUsing(int tableId, CancellationToken cancellationToken = default)
        {
            return await _vContext.Tables.AsNoTracking()
                                    .Where(x => x.Id == tableId &&
                                           x.Status == TableStatus.InUsing)
                                    .AnyAsync(cancellationToken);
        }



    }
}
