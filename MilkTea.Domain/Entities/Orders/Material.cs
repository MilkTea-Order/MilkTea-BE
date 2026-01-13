using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("materials")]
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

        // Navigations
        public MaterialsGroup? MaterialsGroup { get; set; }
        public MaterialsStatus? MaterialsStatus { get; set; }
        public Unit? Unit { get; set; }
        public Unit? UnitMax { get; set; }
    }
}
