using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class UpdatePasswordCommand : IRequest<UpdatePasswordResult>
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

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
