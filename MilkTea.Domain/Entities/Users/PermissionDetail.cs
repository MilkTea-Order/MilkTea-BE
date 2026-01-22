namespace MilkTea.Domain.Entities.Users
{
    public class PermissionDetail : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int PermissionID { get; set; }
        public string? Note { get; set; }

        // Navigation
        public Permission? Permission { get; set; }
    }
}
