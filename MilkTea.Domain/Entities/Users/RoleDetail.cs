namespace MilkTea.Domain.Entities.Users
{
    public class RoleDetail : BaseModel
    {
        public int PermissionDetailID { get; set; }
        public int RoleID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigations
        public Role? Role { get; set; }
        public PermissionDetail? PermissionDetail { get; set; }
    }
}
