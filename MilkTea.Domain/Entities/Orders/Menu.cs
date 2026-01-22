using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Entities.Orders
{
    public class Menu : BaseModel
    {
        public int ID { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Formula { get; set; }
        public byte[]? AvatarPicture { get; set; }
        public string? Note { get; set; }
        public int MenuGroupID { get; set; }
        public int StatusID { get; set; }
        public int UnitID { get; set; }
        public int? TasteQTy { get; set; }
        public bool? PrintSticker { get; set; }

        // Navigations
        public MenuGroup? MenuGroup { get; set; }
        public Status? Status { get; set; }
        public Unit? Unit { get; set; }
    }
}
