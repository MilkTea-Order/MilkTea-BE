namespace MilkTea.Domain.Entities.Users
{
    public class Permission : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int PermissionGroupID { get; set; }
        public string? Note { get; set; }

        // Navigation
        public PermissionGroup? PermissionGroup { get; set; }
    }
}
