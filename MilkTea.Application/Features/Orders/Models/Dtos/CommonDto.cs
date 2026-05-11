namespace MilkTea.Application.Features.Orders.Models.Dtos
{
    public class TableDto
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


    public sealed class MenuDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? Note { get; set; }
    }

    public sealed class SizeDto

    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? RankIndex { get; set; }
    }

    public sealed class OrderStatusDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
