using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("role")]
    public class Role : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Note")]
        public string? Note { get; set; }

        [Required, Column("StatusID")]
        public int StatusID { get; set; }

        [Required, Column("CreatedBy")]
        public int CreatedBy { get; set; }

        [Required, Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Column("LastUpdatedBy")]
        public int? LastUpdatedBy { get; set; }

        [Column("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }
    }
}
