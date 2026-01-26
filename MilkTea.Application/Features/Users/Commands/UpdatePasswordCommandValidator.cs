using FluentValidation;
using MilkTea.Application.Features.Users.Commands;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password không được để trống");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("NewPassword không được để trống")
            .MinimumLength(6)
            .WithMessage("NewPassword phải có ít nhất 6 ký tự")
            .NotEqual(x => x.Password)
            .WithMessage("NewPassword phải khác Password");
    }
}
