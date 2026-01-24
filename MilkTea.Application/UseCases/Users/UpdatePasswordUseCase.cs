using MilkTea.Application.Commands.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Application.Ports.Identity;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Utils;

namespace MilkTea.Application.UseCases.Users
{
    public class UpdatePasswordUseCase(IUnitOfWork unitOfWork,
                                        IUserRepository userRepository,
                                        ICurrentUser currentUser)
    {
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
        private readonly IUserRepository _vUserRepository = userRepository;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<UpdatePasswordResult> Execute(UpdatePasswordCommand command)
        {
            UpdatePasswordResult result = new();
            // Set login time
            result.ResultData.AddMeta(MetaKey.DATE_LOGIN, DateTime.UtcNow);

            // Check exist user
            var userId = _currentUser.UserId;
            var vUser = await _vUserRepository.GetUserByUserID(userId);
            if (vUser == null) return sendMessageError(result, ErrorCode.E0001, "userName");

            // Verify password
            if (!RC2Helper.VerifyPasswordRC2(vUser.Password, command.Password))
                return sendMessageError(result, ErrorCode.E0001, "password");

            // Validate reused password
            if (RC2Helper.VerifyPasswordRC2(vUser.Password, command.NewPassword))
                return sendMessageError(result, ErrorCode.E0012, "newPassword");

            // Update password
            var newEncryptedPassword = RC2Helper.EncryptByRC2(command.NewPassword);
            await _vUserRepository.UpdatePasswordAsync(userId, newEncryptedPassword, userId);
            await _vUnitOfWork.CommitAsync();
            return result;
        }

        private UpdatePasswordResult sendMessageError(
            UpdatePasswordResult result,
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
