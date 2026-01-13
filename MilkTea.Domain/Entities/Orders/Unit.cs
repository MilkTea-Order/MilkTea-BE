using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("Unit")]
    public class Unit : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;
    }


}
