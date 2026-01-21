using MilkTea.Application.Commands.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories.Users;

namespace MilkTea.Application.UseCases.Users
{
    public class GetUserProfileUseCase(IUserRepository userRepository)
    {
        private readonly IUserRepository _vUserRepository = userRepository;

        public async Task<GetUserProfileResult> Execute(GetUserProfileCommand command)
        {
            GetUserProfileResult result = new();
            var user = await _vUserRepository.GetUserWithEmployeeAsync(command.UserId);
            if (user == null)
            {
                result.ResultData.Add(ErrorCode.E0001, nameof(command.UserId));
                return result;
            }

            if (user.Employee == null)
            {
                result.ResultData.Add(ErrorCode.E0001, nameof(command.UserId));
                return result;
            }
            result.User = user;
            return result;
        }
    }
}
