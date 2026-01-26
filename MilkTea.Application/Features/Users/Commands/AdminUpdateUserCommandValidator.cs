using FluentValidation;
using MilkTea.Application.Features.Users.Commands;

namespace MilkTea.Application.Features.Users.Commands;

public sealed class AdminUpdateUserCommandValidator : AbstractValidator<AdminUpdateUserCommand>
{
    public AdminUpdateUserCommandValidator()
    {
        RuleFor(x => x.UserID)
            .GreaterThan(0)
            .WithMessage("UserID phải lớn hơn 0");
    }
}
