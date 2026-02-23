namespace MilkTea.Application.Features.Catalog.Dtos
{
    internal class TableDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public int? NumberOfSeats { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public string? Note { get; set; }
        public string? EmptyImg { get; set; }
        public string? UsingImg { get; set; }
    }
}
