namespace MilkTea.Application.Commands.Users
{
    public class UpdatePasswordCommand
    {
        public int UserId { get; set; } = default;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
