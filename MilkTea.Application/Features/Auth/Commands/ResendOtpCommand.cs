using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Models.Results;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Ports.Notification;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Auth.Exceptions;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Auth.ValueObjects;
using MilkTea.Domain.Common.Constants;
using MilkTea.Shared.Templates;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Auth.Commands;

public class ResendOtpCommand : ICommand<ResendOtpResult>
{
    public int SessionId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
}

public sealed class ResendOtpCommandValidator : AbstractValidator<ResendOtpCommand>
{
    public ResendOtpCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(ResendOtpCommand.SessionId));

        RuleFor(x => x.Channel)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(ResendOtpCommand.Channel));

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .WithErrorCode(ErrorCode.E0001)
            .OverridePropertyName(nameof(ResendOtpCommand.IdempotencyKey));
    }
}

public class ResendOtpCommandHandler(IAuthUnitOfWork authUnitOfWork,
                                        IConfigurationService configurationService,
                                        INotificationSender notificationSender,
                                        IOtpQuery otpQuery,
                                        IMemoryCache memoryCache) : ICommandHandler<ResendOtpCommand, ResendOtpResult>
{
    private readonly IAuthUnitOfWork _vAuthUnitOfWork = authUnitOfWork;
    private readonly IConfigurationService _vConfigurationService = configurationService;
    private readonly INotificationSender _vNotificationSender = notificationSender;
    private readonly IOtpQuery _vOtpQuery = otpQuery;
    private readonly IMemoryCache _vMemoryCache = memoryCache;



    public async Task<ResendOtpResult> Handle(ResendOtpCommand command, CancellationToken cancellationToken)
    {
        var result = new ResendOtpResult();

        // 1. Validate channel
        Channel channel;
        try
        {
            channel = Channel.Create(command.Channel);
        }
        catch (InvalidChannelException)
        {
            return SendError(result, ErrorCode.E0036, nameof(command.Channel));
        }

        // 2. Get session by ID first to validate
        var session = await _vAuthUnitOfWork.Sessions.GetByIdAsync(command.SessionId, cancellationToken);
        if (session is null)
        {
            return SendError(result, ErrorCode.E0001, nameof(command.SessionId));
        }

        // 3. Check if session is expired
        if (session.IsExpired)
        {
            return SendError(result, ErrorCode.E0043, nameof(command.SessionId));
        }

        // 4. Check if session is already verified
        if (session.IsVerified)
        {
            return SendError(result, ErrorCode.E0042, nameof(command.SessionId));
        }

        // 5. Check idempotency cache
        var idempotencyCacheKey = $"idempotency:resend_otp:{command.SessionId}:{command.IdempotencyKey}";
        if (_vMemoryCache.TryGetValue(idempotencyCacheKey, out int? cachedOtpId) && cachedOtpId.HasValue)
        {
            // Query OTP from DB using cached ID (no tracking)
            var cachedOtp = await _vOtpQuery.GetOtpByIdAsync(cachedOtpId.Value, cancellationToken);

            if (cachedOtp != null && cachedOtp.SessionId == command.SessionId)
            {
                var now = DateTime.UtcNow;

                // Check if the OTP is still valid (not expired)
                if (cachedOtp.ExpiredDate > now)
                {
                    // Before returning cached result, check if session is still valid (not verified)
                    // Reuse session entity (already fetched with no tracking above)
                    if (session.IsVerified)
                    {
                        // Session has been verified - remove cache and process as new request
                        _vMemoryCache.Remove(idempotencyCacheKey);
                    }
                    else
                    {
                        // Session is still valid (not verified) - return result from DB (idempotent)
                        result.SessionId = command.SessionId;
                        result.ExpiresAt = cachedOtp.ExpiredDate;
                        return result;
                    }
                }
                else
                {
                    // OTP has expired - remove from cache and process as new request
                    _vMemoryCache.Remove(idempotencyCacheKey);
                }
            }
            else
            {
                // OTP not found in DB or ID mismatch - remove invalid cache entry
                _vMemoryCache.Remove(idempotencyCacheKey);
            }
        }

        // 6. Get OTP send limit from configuration for the specific channel
        var otpSendLimit = await _vConfigurationService.GetOtpMaxAttemptsAsync(cancellationToken);

        // 7. Count successful OTP sends in DB for this session and channel
        // If session is verified, only count OTPs created AFTER verification (reset counter)
        var totalOtpSends = await _vOtpQuery.CountSuccessfulOtpBySessionAndChannelAsync(command.SessionId,
                                                                                                channel.Value,
                                                                                                session.VerifiedDate,
                                                                                                cancellationToken);

        if (totalOtpSends >= otpSendLimit)
        {
            return SendError(result, ErrorCode.E0044, nameof(command.SessionId));
        }

        // 8. Get OTP expiration time for creating new OTP
        var expirationMinutes = await _vConfigurationService.GetOtpExpirationMinutesAsync(cancellationToken);
        var expiresDate = DateTime.Now.AddMinutes(expirationMinutes);

        // 9. Transaction: create new OTP and send notification
        await _vAuthUnitOfWork.BeginTransactionAsync(cancellationToken);
        OtpEntity? newOtp = null;
        try
        {
            // Create new OTP
            newOtp = OtpEntity.Create(
                sessionId: command.SessionId,
                channel: channel,
                expiredDate: expiresDate);
            await _vAuthUnitOfWork.Otps.AddAsync(newOtp, cancellationToken);
            await _vAuthUnitOfWork.SaveChangesAsync(cancellationToken);

            // Send notification based on channel
            string recipient = session.Email;

            await _vNotificationSender.SendAsync(
                new NotificationRequest(
                    channel == Channel.Email ? NotificationChannel.Email : NotificationChannel.Sms,
                    recipient,
                    "Mã OTP mới của bạn",
                    OtpEmail.BuildOtpTemplate(newOtp.OtpCode, expirationMinutes, "Mã OTP để xác thực", "MilkTea Shop")),
                cancellationToken);

            await _vAuthUnitOfWork.CommitTransactionAsync(cancellationToken);

            // 10. Cache the new OTP ID for idempotency
            var idempotencyOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Cache.IdempotencyCacheMinutes));
            _vMemoryCache.Set(idempotencyCacheKey, newOtp.Id, idempotencyOptions);
            result.SessionId = command.SessionId;
            result.ExpiresAt = newOtp.ExpiredDate;
            return result;
        }
        catch (InvalidChannelException)
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E0036, "Channel");
        }
        catch
        {
            await _vAuthUnitOfWork.RollbackTransactionAsync(cancellationToken);
            return SendError(result, ErrorCode.E9999, "ResendOtp");
        }
    }

    private static ResendOtpResult SendError(ResendOtpResult result, string errorCode, params string[] values)
    {
        if (values is { Length: > 0 })
            result.ResultData.Add(errorCode, values.ToList());
        return result;
    }
}
