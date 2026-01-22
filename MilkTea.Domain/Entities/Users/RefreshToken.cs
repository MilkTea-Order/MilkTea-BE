namespace MilkTea.Domain.Entities.Users
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiryDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool IsRevoked { get; set; } = false;

        // Navigation
        public User? User { get; set; }
    }
}
