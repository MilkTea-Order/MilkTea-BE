using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Orders
{
    [Table("dinnertable")]
    public class DinnerTable : BaseModel
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("Code")]
        public string? Code { get; set; }

        [Required, Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Position")]
        public string? Position { get; set; }

        [Required, Column("NumberOfSeats")]
        public int NumberOfSeats { get; set; }

        [Column("Longs")]
        public int? Longs { get; set; }

        [Column("Width")]
        public int? Width { get; set; }

        [Column("Height")]
        public int? Height { get; set; }

        [Required, Column("EmptyPicture")]
        public byte[] EmptyPicture { get; set; } = null!;

        [Required, Column("UsingPicture")]
        public byte[] UsingPicture { get; set; } = null!;

        [Required, Column("StatusOfDinnerTableID")]
        public int StatusOfDinnerTableID { get; set; }

        [Column("Note")]
        public string? Note { get; set; }

        public StatusOfDinnerTable? StatusOfDinnerTable { get; set; }
    }


}
