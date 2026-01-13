using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("menugroup")]
    public class MenuGroup : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Required, Column("StatusID")]
        public int StatusID { get; set; }

        public Status? Status { get; set; }
    }


}
