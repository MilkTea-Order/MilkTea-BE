using MilkTea.Application.Features.Catalog.Models.Dtos.Material;

namespace MilkTea.Application.Features.Catalog.Abstractions.Services
{
    public interface IMaterialService
    {
        Task<List<MaterialDto>> GetByIdsAsync(List<int> ids);
    }
}
