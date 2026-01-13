using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("menuandmaterials")]
    public class MenuAndMaterial : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("MenuID")]
        public int MenuID { get; set; }

        [Required, Column("SizeID")]
        public int SizeID { get; set; }

        [Required, Column("MaterialsID")]
        public int MaterialsID { get; set; }

        [Required, Column("Quantity")]
        public decimal Quantity { get; set; }
    }



}
