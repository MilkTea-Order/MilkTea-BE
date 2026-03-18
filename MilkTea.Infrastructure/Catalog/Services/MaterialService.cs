using Microsoft.EntityFrameworkCore;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Catalog.Models.Dtos;
using MilkTea.Application.Features.Catalog.Models.Dtos.Material;
using MilkTea.Application.Features.Catalog.Models.Dtos.Unit;
using MilkTea.Infrastructure.Persistence;
using Shared.Extensions;

namespace MilkTea.Infrastructure.Catalog.Services
{
    public class MaterialService(AppDbContext context) : IMaterialService
    {
        private readonly AppDbContext _vContext = context;
        public async Task<List<MaterialDto>> GetByIdsAsync(List<int> ids)
        {
            if (ids == null || !ids.Any()) return new List<MaterialDto>();
            ids = ids.Distinct().ToList();
            var query = from m in _vContext.Materials.AsNoTracking()
                        join g in _vContext.MaterialsGroups.AsNoTracking()
                            on m.MaterialsGroupID equals g.Id
                        join uMin in _vContext.Units on m.UnitID equals uMin.Id
                        join uMax in _vContext.Units on m.UnitID_Max equals uMax.Id
                        where ids.Contains(m.Id)
                        select new
                        {
                            GroupId = g.Id,
                            GroupName = g.Name,
                            Item = new MaterialItemDto
                            {
                                Id = m.Id,
                                Name = m.Name,
                                Code = m.Code,
                                UnitMin = new UnitDto
                                {
                                    Id = uMin.Id,
                                    Name = uMin.Name
                                },
                                UnitMax = new UnitDto
                                {
                                    Id = uMax.Id,
                                    Name = uMax.Name
                                },
                                StyleQuantity = m.StyleQuantity ?? 1,
                                Status = new StatusDto
                                {
                                    Id = (int)m.Status,
                                    Name = m.Status.GetDescription()
                                }
                            }
                        };
            var result = (await query.ToListAsync());
            return result.GroupBy(x => new { x.GroupId, x.GroupName })
                        .Select(g => new MaterialDto
                        {
                            Id = g.Key.GroupId,
                            Name = g.Key.GroupName,
                            MaterialItems = g.Select(x => x.Item).ToList()
                        }).ToList();
        }
    }
}

