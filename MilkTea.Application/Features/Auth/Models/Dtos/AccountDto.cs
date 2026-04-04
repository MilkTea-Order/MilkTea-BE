namespace MilkTea.Application.Features.Auth.Models.Dtos
{
    public class AccountDto
    {
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
        public string Username { get; set; } = string.Empty;

        public int StatusID { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
