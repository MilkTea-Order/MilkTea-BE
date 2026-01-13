using MilkTea.Application.Commands.Users;
using MilkTea.Application.Ports.Authentication.JWTPort;
using MilkTea.Application.Results.Users;
using MilkTea.Domain.Constants.Errors;
using MilkTea.Domain.Entities.Users;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Extensions;
using MilkTea.Shared.Utils;

namespace MilkTea.Application.UseCases.Users
{
    public class LoginWithUserNameUseCase(IUserRepository userRepository,
                                            IStatusRepository statusRepository,
                                            IPermissionRepository permissionRepository,
                                            IJWTServicePort jwtServicePort,
                                            IRefreshTokenRepository refreshTokenRepository,
                                            IUnitOfWork unitOfWork)
    {
        private readonly IUserRepository _vUserRepository = userRepository;
        private readonly IStatusRepository _vStatusRepository = statusRepository;
        private readonly IPermissionRepository _vPermission = permissionRepository;
        private readonly IJWTServicePort _vJWTServicePort = jwtServicePort;
        private readonly IRefreshTokenRepository _vRefreshTokenRepository = refreshTokenRepository;
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
        //private readonly IUnitOfWork _vUnitOfWork = unitOfWork;
        public async Task<LoginWithUserNameResult> Execute(LoginCommand command)
        {
            LoginWithUserNameResult result = new();
            // Set login time
            result.ResultData.AddMeta(MetaKey.DATE_LOGIN, DateTime.UtcNow);

            // Validate username
            if (command.UserName.IsNullOrWhiteSpace()) return SendMessageError(result, ErrorCode.E0004, "Username");

            // Check exist user
            var vUser = await _vUserRepository.GetUserByUserName(command.UserName);
            if (vUser == null) return SendMessageError(result, ErrorCode.E0001, "Username");

            // Verify password
            if (!RC2Helper.VerifyPasswordRC2(vUser.Password, command.Password)) return SendMessageError(result, ErrorCode.E0001, "Password");

            // If valid account, will ..

            // Set user info
            result.UserId = vUser.ID;

            // Status
            var status = await _vStatusRepository.GetActive();
            if (!(status != null && status.ID == vUser.StatusID))
            {
                return SendMessageError(result, ErrorCode.E0005, "StatusID");
            }

            // Permissions
            result.Permissions = await _vPermission.GetPermissionsByUserId(vUser.ID);

            // Tokens
            (var accessToken, var expiresAt) = _vJWTServicePort.CreateJwtAccessToken(vUser.ID);
            result.AccessToken = accessToken;
            result.AccessTokenExpiresAt = expiresAt;
            (result.RefreshToken, var refreshTokenExpiresAt) = _vJWTServicePort.CreateJwtRefreshToken();


            // Save refresh token
            var refreshToken = new RefreshToken
            {
                UserId = vUser.ID,
                Token = result.RefreshToken,
                ExpiryDate = refreshTokenExpiresAt,
                IsRevoked = false,
                CreatedDate = DateTime.UtcNow
            };
            await _vRefreshTokenRepository.StoreRefreshTokenAsync(refreshToken);
            await _vUnitOfWork.CommitAsync();
            return result;
        }
        private LoginWithUserNameResult SendMessageError(
            LoginWithUserNameResult result,
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


