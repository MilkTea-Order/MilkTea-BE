using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("permissiondetail")]
    public class PermissionDetail : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Required, Column("Code")]
        public string Code { get; set; } = null!;

        [Required, Column("PermissionID")]
        public int PermissionID { get; set; }

        [Column("Note")]
        public string? Note { get; set; }
    }


}
