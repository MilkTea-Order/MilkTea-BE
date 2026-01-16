namespace MilkTea.Application.Commands.Users
{
    public class LogoutCommand
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
