using FluentValidation;
using MilkTea.Application.Features.Users.Commands;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken không được để trống");
    }
}
