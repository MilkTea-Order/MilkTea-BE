using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class LogoutCommand : IRequest<LogoutResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken không được để trống");
    }
}
