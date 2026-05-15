namespace MilkTea.Application.Features.Catalog.Models.Dtos.Table
{
    public class TableDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public int? NumberOfSeats { get; set; }
        public required StatusDto Status {get; set;}
        public string? Note { get; set; }
        public string? EmptyImg { get; set; }
        public string? UsingImg { get; set; }
    }
}
