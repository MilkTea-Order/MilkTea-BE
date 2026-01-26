using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class LogoutCommand : IRequest<LogoutResult>
{
    public string RefreshToken { get; set; } = string.Empty;
}
