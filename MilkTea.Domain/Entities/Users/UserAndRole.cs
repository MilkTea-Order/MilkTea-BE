using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("UserAndRole")]
    public class UserAndRole : BaseModel
    {
        [Key, Column("UserID", Order = 0)]
        public int UserID { get; set; }

        [Key, Column("RoleID", Order = 1)]
        public int RoleID { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}
