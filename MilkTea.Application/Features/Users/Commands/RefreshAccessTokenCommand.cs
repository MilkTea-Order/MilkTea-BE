using FluentValidation;
using MilkTea.Application.Features.Users.Results;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Users.Commands;

public class RefreshAccessTokenCommand : ICommand<RefreshAccessTokenResult>
{
    public string? RefreshToken { get; set; } = string.Empty;
}

public sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
    }
}
