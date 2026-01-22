namespace MilkTea.Domain.Entities.Users
{
    public class PermissionGroup : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int PermissionGroupTypeID { get; set; }
        public string? Note { get; set; }

        // Navigation
        public PermissionGroupType? PermissionGroupType { get; set; }
    }
}
