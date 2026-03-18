using MediatR;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Inventory.Abstractions;
using MilkTea.Application.Features.Inventory.Models.Dtos;
using MilkTea.Application.Features.Inventory.Models.Results;

namespace MilkTea.Application.Features.Inventory.Queries
{
    public class GetInventorySummaryQuery : IRequest<GetInventorySummaryResult>
    {
    }

    public class GetInventorySummaryQueryHandler(IInventoryQuery inventoryQuery,
                                                    IMaterialService materialService) : IRequestHandler<GetInventorySummaryQuery, GetInventorySummaryResult>
    {
        private readonly IInventoryQuery _vInventoryQuery = inventoryQuery;
        private readonly IMaterialService _vMaterialService = materialService;


        public async Task<GetInventorySummaryResult> Handle(GetInventorySummaryQuery request, CancellationToken cancellationToken)
        {
            var result = new GetInventorySummaryResult();

            var inventory = await _vInventoryQuery.GetInventoryReportAsync();
            if (!inventory.Any()) return result;

            var materialIds = inventory.Select(x => x.MaterialId).Distinct().ToList();
            var materials = await _vMaterialService.GetByIdsAsync(materialIds);

            var inventoryDict = inventory.ToDictionary(x => x.MaterialId);

            result.Materials = materials.Select(group => new MaterialInventoryDto
            {
                Id = group.Id,
                Name = group.Name,
                MaterialItems = group.MaterialItems.Select(item =>
                {
                    inventoryDict.TryGetValue(item.Id, out var inv);

                    return new MaterialItemInventoryDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Code = item.Code,

                        UnitMin = new UnitDto
                        {
                            Id = item.UnitMin.Id,
                            Name = item.UnitMin.Name,
                            Quantity = inv?.TotalQuantity ?? 1
                        },
                        UnitMax = new UnitDto
                        {
                            Id = item.UnitMax.Id,
                            Name = item.UnitMax.Name,
                            Quantity = (inv?.TotalQuantity ?? 1) / item.StyleQuantity
                        },

                        StyleQuantity = item.StyleQuantity,
                        Status = new StatusDto
                        {
                            Id = item.Status.Id,
                            Name = item.Status.Name
                        },
                        LatestPriceImport = inv?.LatestPriceImport
                    };
                }).ToList()
            }).ToList();

            return result;
        }
    }
}
