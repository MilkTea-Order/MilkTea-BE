using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("Gender")]
    public class Gender : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;
    }


}
