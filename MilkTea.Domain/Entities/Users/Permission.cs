using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("Permission")]
    public class Permission : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Required, Column("Code")]
        public string Code { get; set; } = null!;

        [Required, Column("PermissionGroupID")]
        public int PermissionGroupID { get; set; }

        [Column("Note")]
        public string? Note { get; set; }
    }


}
