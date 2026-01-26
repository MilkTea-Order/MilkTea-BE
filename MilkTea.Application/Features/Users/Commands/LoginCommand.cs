using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class LoginCommand : IRequest<LoginWithUserNameResult>
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
