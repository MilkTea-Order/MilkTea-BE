using FluentValidation;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Ports.Authentication.JWTPorts;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class LoginCommand : ICommand<LoginWithUserNameResult>
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(LoginCommand.UserName));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(LoginCommand.Password));
    }
}

public sealed class LoginCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                            IJWTServicePort jwtServicePort,
                                            IPasswordHasher passwordHasher,
                                            IPermissionQuery permissionQuery
                                            ) : ICommandHandler<LoginCommand, LoginWithUserNameResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IJWTServicePort _vJwtServicePort = jwtServicePort;
    private readonly IPasswordHasher _vPasswordHasher = passwordHasher;
    private readonly IPermissionQuery _vPermissionQuery = permissionQuery;
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
        result.Permissions = (await _vPermissionQuery.GetPermissionsByUserIdAsync(user.Id, cancellationToken)).ToList();

        // Create tokens
        var (accessToken, expiresAt) = _vJwtServicePort.CreateJwtAccessToken(user.Id);
        result.AccessToken = accessToken;
        result.AccessTokenExpiresAt = expiresAt;

        var (refreshToken, refreshTokenExpiresAt) = _vJwtServicePort.CreateJwtRefreshToken();
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
