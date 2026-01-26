using FluentValidation;
using MilkTea.Application.Features.Users.Commands;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName không được để trống");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password không được để trống");
    }
}
