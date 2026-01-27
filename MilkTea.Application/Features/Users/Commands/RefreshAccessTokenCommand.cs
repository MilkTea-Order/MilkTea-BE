using FluentValidation;
using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class RefreshAccessTokenCommand : IRequest<RefreshAccessTokenResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("RefreshToken không được để trống");
    }
}
