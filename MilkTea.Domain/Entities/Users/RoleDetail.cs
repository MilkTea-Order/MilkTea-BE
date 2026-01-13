using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("roledetail")]
    public class RoleDetail : BaseModel
    {
        [Key, Column("PermissionDetailID", Order = 0)]
        public int PermissionDetailID { get; set; }

        [Key, Column("RoleID", Order = 1)]
        public int RoleID { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        // Navigations
        public Role? Role { get; set; }
        public PermissionDetail? PermissionDetail { get; set; }
    }


}
