using MilkTea.Application.Features.Catalog.Models.Dtos.Material;

namespace MilkTea.Application.Features.Catalog.Abstractions.Queries
{
    public interface IMaterialQuery
    {
        Task<List<MaterialDto>> GetByIdsAsync(List<int> ids);
    }
}
