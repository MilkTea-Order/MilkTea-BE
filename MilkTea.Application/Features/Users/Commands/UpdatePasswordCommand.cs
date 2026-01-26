using MediatR;
using MilkTea.Application.Features.Users.Results;

namespace MilkTea.Application.Features.Users.Commands;

public class UpdatePasswordCommand : IRequest<UpdatePasswordResult>
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
