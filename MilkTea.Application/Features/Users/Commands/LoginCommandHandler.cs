using MediatR;
using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Utils;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class LoginCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IPermissionRepository permissionRepository,
    IJWTServicePort jwtServicePort) : IRequestHandler<LoginCommand, LoginWithUserNameResult>
{
    public async Task<LoginWithUserNameResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = new LoginWithUserNameResult();
        result.ResultData.AddMeta(MetaKey.DATE_LOGIN, DateTime.UtcNow);

        // Validate username
        if (string.IsNullOrWhiteSpace(command.UserName))
            return SendError(result, ErrorCode.E0001, "Username");

        // Get user
        var user = await userRepository.GetByUserNameAsync(command.UserName);
        if (user is null)
            return SendError(result, ErrorCode.E0001, "Username");

        // Verify password
        if (!RC2Helper.VerifyPasswordRC2(user.Password, command.Password))
            return SendError(result, ErrorCode.E0001, "Password");

        // Check user is active (using domain property)
        if (!user.IsActive)
            return SendError(result, ErrorCode.E0005, "StatusID");

        await unitOfWork.BeginTransactionAsync();
        try
        {
            result.UserId = user.Id;

            var permissions = await permissionRepository.GetPermissionsByUserIdAsync(user.Id);

            result.Permissions = permissions.Select(p => new Dictionary<string, object?>
            {
                ["Id"] = p.PermissionDetail.Id,
                ["PermissionId"] = p.PermissionDetail.PermissionID,
                ["Note"] = p.PermissionDetail.Note,
                ["Permission"] = new Dictionary<string, object?>
                {
                    ["Id"] = p.Permission.Id,
                    ["Name"] = p.Permission.Name,
                    ["Code"] = p.Permission.Code
                }
            }).ToList();

            // Create tokens
            var (accessToken, expiresAt) = jwtServicePort.CreateJwtAccessToken(user.Id);
            result.AccessToken = accessToken;
            result.AccessTokenExpiresAt = expiresAt;

            var (refreshToken, refreshTokenExpiresAt) = jwtServicePort.CreateJwtRefreshToken();
            result.RefreshToken = refreshToken;

            // Save refresh token using domain method
            user.AddRefreshToken(refreshToken, refreshTokenExpiresAt);
            await userRepository.UpdateAsync(user);

            await unitOfWork.CommitTransactionAsync();

            return result;
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            return SendError(result, ErrorCode.E9999, "Login");
        }
    }

    private static LoginWithUserNameResult SendError(LoginWithUserNameResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
