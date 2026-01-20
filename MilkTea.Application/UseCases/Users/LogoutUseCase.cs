using MilkTea.Application.Commands.Users;
using MilkTea.Application.Results.Users;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Shared.Domain.Constants;

namespace MilkTea.Application.UseCases.Users
{
    public class LogoutUseCase(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
    {
        private readonly IRefreshTokenRepository _vRefreshTokenRepository = refreshTokenRepository;
        private readonly IUnitOfWork _vUnitOfWork = unitOfWork;

        public async Task<LogoutResult> Execute(LogoutCommand command)
        {
            LogoutResult result = new();
            var refreshToken = await _vRefreshTokenRepository.GetTokenAndRevokeAsync(command.RefreshToken);
            if (refreshToken == null || refreshToken.UserId != command.UserId)
            {
                result.ResultData.AddMeta(MetaKey.TOKEN_ERROR, true);
            }
            // Revoke the specific refresh token
            await _vUnitOfWork.CommitAsync();
            return result;
        }

        private LogoutResult SendMessageError(
            LogoutResult result,
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


//// Validate userId
//if (command.UserId <= 0)
//{
//    return SendMessageError(result, ErrorCode.E0036, "UserId");
//}

//// Validate refreshToken
//if (command.RefreshToken.IsNullOrWhiteSpace())
//{
//    return SendMessageError(result, ErrorCode.E0004, "RefreshToken");
//}

//var refreshToken = await _vRefreshTokenRepository.GetByTokenAsync(command.RefreshToken);

//// Check if token exists
//if (refreshToken == null)
//{
//    return SendMessageError(result, ErrorCode.E0044, "RefreshToken");
//}

//// Check if token belongs to the user
//if (refreshToken.UserId != command.UserId)
//{
//    return SendMessageError(result, ErrorCode.E0044, "RefreshToken");
//}

//// Check if token is revoked
//if (refreshToken.IsRevoked)
//{
//    return SendMessageError(result, ErrorCode.E0044, "RefreshToken");
//}

//if (refreshToken.ExpiryDate <= DateTime.UtcNow)
//{
//    return SendMessageError(result, ErrorCode.E0043, "RefreshToken");
//}