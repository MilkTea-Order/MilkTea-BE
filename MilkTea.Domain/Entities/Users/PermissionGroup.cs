using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("permissiongroup")]
    public class PermissionGroup : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Required, Column("PermissionGroupTypeID")]
        public int PermissionGroupTypeID { get; set; }

        [Column("Note")]
        public string? Note { get; set; }
    }

}
