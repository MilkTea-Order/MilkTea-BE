using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("Materials")]
    public class Material : BaseModel
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Required]
        [Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Code")]
        public string? Code { get; set; }

        [Column("UnitID")]
        public int? UnitID { get; set; }

        [Column("UnitID_Max")]
        public int? UnitID_Max { get; set; }

        [Column("StyleQuantity")]
        public int? StyleQuantity { get; set; }

        [Required]
        [Column("MaterialsGroupID")]
        public int MaterialsGroupID { get; set; }

        [Required]
        [Column("StatusID")]
        public int StatusID { get; set; }
    }
}
