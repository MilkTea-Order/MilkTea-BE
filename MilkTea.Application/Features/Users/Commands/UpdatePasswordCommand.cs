using FluentValidation;
using MilkTea.Application.Features.Users.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Users.Commands;

public class UpdatePasswordCommand : ICommand<UpdatePasswordResult>
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
                  .NotEmpty().WithErrorCode(ErrorCode.E0001)
                  .MinimumLength(6).WithErrorCode(ErrorCode.E0001)
                  .OverridePropertyName("password");
        RuleFor(x => x.NewPassword)
                  .NotEmpty().WithErrorCode(ErrorCode.E0001)
                  .MinimumLength(6).WithErrorCode(ErrorCode.E0001)
                  .NotEqual(x => x.Password).WithErrorCode(ErrorCode.E0012)
                  .OverridePropertyName("newPassword");
        RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.NewPassword).WithErrorCode(ErrorCode.E0012)
                    .OverridePropertyName("confirmPassword");
    }
}
