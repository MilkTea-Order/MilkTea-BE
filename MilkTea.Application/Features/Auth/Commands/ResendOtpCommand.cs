using FluentValidation;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Application.Ports.Notification;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Templates;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class ResendOtpCommand : ICommand<ResendOtpResult>
{
    public string Email { get; set; } = string.Empty;
}

public sealed class ResendOtpCommandValidator : AbstractValidator<ResendOtpCommand>
{
    public ResendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(ResendOtpCommand.Email));
    }
}

public class ResendOtpCommandHandler(
    IAuthUnitOfWork authUnitOfWork,
    IConfigurationService configurationService,
    IUserService userService,
    INotificationSender notificationSender) : ICommandHandler<ResendOtpCommand, ResendOtpResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IConfigurationService _vConfigurationService = configurationService;
    private readonly IUserService _vUserService = userService;
    private readonly INotificationSender _vNotificationSender = notificationSender;

    public async Task<ResendOtpResult> Handle(ResendOtpCommand command, CancellationToken cancellationToken)
    {
        var result = new ResendOtpResult();

        // 1. Validate email → lấy EmployeeID
        var employeeId = await _vUserService.GetUserIdByEmailAsync(command.Email, cancellationToken);
        if (employeeId == null)
            return SendError(result, ErrorCode.E0001, nameof(command.Email));

        // 2. Check user tồn tại
        var user = await _vAuthUnitOfWork.Users.GetByIdAsync(employeeId.Value, cancellationToken);
        if (user == null)
            return SendError(result, ErrorCode.E0001, nameof(command.Email));

        // 3. Lấy OTP mới nhất với tracking
        var otp = await _vAuthUnitOfWork.Otps.GetLatestByEmailAndTypeForUpdateAsync(
            command.Email,
            Denifinitions.OTP_TYPE_FORGOT_PASSWORD,
            cancellationToken);

        if (otp == null)
            return SendError(result, ErrorCode.E0001, nameof(command.Email));

        // 4. Check max attempts
        var maxAttempts = await _vConfigurationService.GetOtpMaxAttemptsAsync(cancellationToken);
        if (otp.IsMaxAttemptsReached(maxAttempts))
            return SendError(result, ErrorCode.E0044, nameof(command.Email));

        // 5. Reset OtpCode + OtpDate
        var newOtpCode = otp.ResetOtpCode();

        // 6. Get expiration minutes for email template
        var expirationMinutes = await _vConfigurationService.GetOtpExpirationMinutesAsync(cancellationToken);

        // 7. Transaction: save OTP + send email
        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _vNotificationSender.SendAsync(
                new NotificationRequest(
                    NotificationChannel.Email,
                    command.Email,
                    "Mã OTP của bạn",
                    OtpEmail.BuildOtpTemplate(newOtpCode, expirationMinutes, "Mã OTP để xác thực", "MilkTea Shop")),
                cancellationToken);

            await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }

        return result;
    }

    private static ResendOtpResult SendError(ResendOtpResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
