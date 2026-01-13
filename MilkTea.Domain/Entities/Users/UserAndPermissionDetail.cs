using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("UserAndPermissionDetail")]
    public class UserAndPermissionDetail : BaseModel
    {
        [Key, Column("UserID", Order = 0)]
        public int UserID { get; set; }

        [Key, Column("PermissionDetailID", Order = 1)]
        public int PermissionDetailID { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}
