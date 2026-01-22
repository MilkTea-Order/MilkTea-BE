namespace MilkTea.Domain.Entities.Users
{
    public class UserAndRole : BaseModel
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigations
        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
