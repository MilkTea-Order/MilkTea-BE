using MilkTea.Domain.Entities.Config;

namespace MilkTea.Domain.Respositories.Configs
{
    public interface IDenifitionRepository
    {
        Task<Definition?> GetCodePrefixBill();
    }
}
