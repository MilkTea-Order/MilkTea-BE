using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.Finance.Models.Results
{
    public class CreateFinanceTransactionResult
    {
        public StringListEntry ResultData { get; set; } = new();
    }
}
