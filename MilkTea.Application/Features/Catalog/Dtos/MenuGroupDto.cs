namespace MilkTea.Application.Features.Catalog.Dtos
{
    internal class MenuGroupDto
    {
        public int MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
