namespace MilkTea.Domain.Entities.Orders
{
    public class DinnerTable : BaseModel
    {
        public int ID { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Position { get; set; }
        public int NumberOfSeats { get; set; }
        public int? Longs { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public byte[] EmptyPicture { get; set; } = null!;
        public byte[] UsingPicture { get; set; } = null!;
        public int StatusOfDinnerTableID { get; set; }
        public string? Note { get; set; }

        public StatusOfDinnerTable? StatusOfDinnerTable { get; set; }
    }
}
