using MilkTea.Application.Results.Orders;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Orders
{
    public class GetGroupMenuAvaliableUseCase(
                                      IStatusRepository statusRepository,
                                      IMenuRepository menuRepository)
    {
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IMenuRepository _vMenuRepository = menuRepository;
        public async Task<GetGroupMenuResult> Execute()
        {
            GetGroupMenuResult result = new();
            // Set time
            result.ResultData.AddMeta(MetaKey.DATE_REQUEST, DateTime.UtcNow);
            result.GroupMenu = await _vMenuRepository.GetMenuGroupAvaliableAsync();
            return result;
        }
        private GetGroupMenuResult SendMessageError(
            GetGroupMenuResult result,
            string errorCode,
            params string[] values)
        {
            if (values != null && values.Length > 0)
            {
                result.ResultData.Add(errorCode, values.ToList());
            }
            return result;
        }
    }
}
