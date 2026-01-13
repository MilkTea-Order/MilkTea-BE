using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Config
{
    [Table("definition")]
    public class Definition : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Required, Column("Code")]
        public string Code { get; set; } = null!;

        [Column("Value")]
        public string? Value { get; set; }

        [Column("ValueImage")]
        public byte[]? ValueImage { get; set; }

        [Required, Column("IsEdit")]
        public int IsEdit { get; set; }

        [Required, Column("IsEncrypt")]
        public int IsEncrypt { get; set; }

        [Required, Column("DefinitionGroupID")]
        public int DefinitionGroupID { get; set; }

        // Navigation
        public DefinitionGroup? DefinitionGroup { get; set; }
    }

}
