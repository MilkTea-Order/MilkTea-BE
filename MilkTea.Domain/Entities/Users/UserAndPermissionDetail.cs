namespace MilkTea.Domain.Entities.Users
{
    public class UserAndPermissionDetail : BaseModel
    {
        public int UserID { get; set; }
        public int PermissionDetailID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigations
        public User? User { get; set; }
        public PermissionDetail? PermissionDetail { get; set; }
    }
}
