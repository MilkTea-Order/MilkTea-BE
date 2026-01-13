using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("menu")]
    public class Menu : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Code")]
        public string Code { get; set; } = null!;

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Formula")]
        public string? Formula { get; set; }

        [Column("AvatarPicture")]
        public byte[]? AvatarPicture { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        [Required, Column("MenuGroupID")]
        public int MenuGroupID { get; set; }

        [Required, Column("StatusID")]
        public int StatusID { get; set; }

        [Required, Column("UnitID")]
        public int UnitID { get; set; }

        [Column("TasteQTy")]
        public int? TasteQTy { get; set; }

        [Column("PrintSticker")]
        public bool? PrintSticker { get; set; }

        // Navigations
        public MenuGroup? MenuGroup { get; set; }
        public Status? Status { get; set; }
        public Unit? Unit { get; set; }
    }


}
