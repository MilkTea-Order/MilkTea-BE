using FluentValidation;
using MilkTea.Application.Features.User.Model.Results;
using MilkTea.Application.Models.Users;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Application.Ports.Hash.Permission;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class LoginCommand : ICommand<LoginWithUserNameResult>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("Username");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName("Password");
    }
}

public sealed class LoginCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                            IJWTServicePort jwtServicePort,
                                            IPasswordHasher passwordHasher,
                                            IPermissionHasher permissionHasher
                                            ) : ICommandHandler<LoginCommand, LoginWithUserNameResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IJWTServicePort _vJWTServicePort = jwtServicePort;
    private readonly IPasswordHasher _vPasswordHasher = passwordHasher;
    private readonly IPermissionHasher _vPermissionHasher = permissionHasher;
    public async Task<LoginWithUserNameResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = new LoginWithUserNameResult();
        result.ResultData.AddMeta(MetaKey.DATE_LOGIN, DateTime.Now);

        // Get user
        var user = await _vAuthUnitOfWork.Users.GetByUserNameForUpdateAsync(command.UserName, cancellationToken);
        if (user is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(LoginCommand.UserName));
        }

        // Verify password
        if (!_vPasswordHasher.VerifyHashedPassword(user.Password.value, command.Password))
        {
            return SendError(result, ErrorCode.E0001, nameof(LoginCommand.Password));
        }

        // Check user is active
        if (!user.IsActive)
        {
            return SendError(result, ErrorCode.E0036, "StatusID");
        }

        result.UserId = user.Id;
        var permissions = await _vAuthUnitOfWork.Permissions.GetPermissionsByUserIdAsync(user.Id, cancellationToken);
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

        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            user.AddRefreshToken(refreshToken, refreshTokenExpiresAt);
            await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch (Exception)
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
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
