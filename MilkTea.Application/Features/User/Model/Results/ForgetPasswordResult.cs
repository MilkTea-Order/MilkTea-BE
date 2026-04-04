using MilkTea.Shared.Domain.Services;

namespace MilkTea.Application.Features.User.Model.Results
{
    public class ForgetPasswordResult
    {
        public StringListEntry ResultData { get; set; } = new StringListEntry();
    }
}
