namespace MilkTea.Domain.Entities.Users
{
    public class PermissionGroupType : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
    }
}
