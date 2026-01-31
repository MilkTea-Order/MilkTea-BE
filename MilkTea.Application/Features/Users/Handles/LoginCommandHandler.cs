using MilkTea.Application.Features.Users.Commands;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Application.Models.Users;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Application.Ports.Hash.Permission;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Users.Handles;

public sealed class LoginCommandHandler(
    IUserUnitOfWork userUnitOfWork,
    IJWTServicePort jwtServicePort,
    IPasswordHasher passwordHasher,
    IPermissionHasher permissionHasher
    ) : ICommandHandler<LoginCommand, LoginWithUserNameResult>
{
    private readonly IUserUnitOfWork _vUserUnitOfWork = userUnitOfWork;
    private readonly IJWTServicePort _vJWTServicePort = jwtServicePort;
    private readonly IPasswordHasher _vPasswordHasher = passwordHasher;
    private readonly IPermissionHasher _vPermissionHasher = permissionHasher;
    public async Task<LoginWithUserNameResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = new LoginWithUserNameResult();
        result.ResultData.AddMeta(MetaKey.DATE_LOGIN, DateTime.UtcNow);

        // Get user
        var user = await _vUserUnitOfWork.Users.GetByUserNameForUpdateAsync(command.UserName, cancellationToken);
        if (user is null)
        {
            return SendError(result, ErrorCode.E0001, "Username");
        }

        // Verify password
        if (!_vPasswordHasher.VerifyHashedPassword(user.Password.value, command.Password))
        {
            return SendError(result, ErrorCode.E0001, "Password");
        }

        // Check user is active (using domain property)
        if (!user.IsActive)
        {
            return SendError(result, ErrorCode.E0005, "StatusID");
        }

        result.UserId = user.Id;
        var permissions = await _vUserUnitOfWork.Permissions.GetPermissionsByUserIdAsync(user.Id, cancellationToken);
        result.Permissions = permissions.Select(p =>
                                            new UserPermission(
                                                Id: p.PermissionDetail.Id,
                                                PermissionId: p.PermissionDetail.PermissionID,
                                                Note: p.PermissionDetail.Note,
                                                Permission: new PermissionInfo(
                                                    Id: p.Permission.Id,
                                                    Name: p.Permission.Name,
                                                    Code: _vPermissionHasher.DecodePermission(p.Permission.Code)
                                                )
                                            )
                                        ).ToList();

        // Create tokens
        var (accessToken, expiresAt) = _vJWTServicePort.CreateJwtAccessToken(user.Id);
        result.AccessToken = accessToken;
        result.AccessTokenExpiresAt = expiresAt;

        var (refreshToken, refreshTokenExpiresAt) = _vJWTServicePort.CreateJwtRefreshToken();
        result.RefreshToken = refreshToken;

        await _vUserUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            user.AddRefreshToken(refreshToken, refreshTokenExpiresAt);
            await _vUserUnitOfWork.CommitTransactionAsync(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            await _vUserUnitOfWork.RollbackTransactionAsync(cancellationToken);
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
