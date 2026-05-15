namespace MilkTea.Application.Features.Catalog.Models.Dtos.Menu
{
    public class MenuGroupDto
    {
        public int MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }
        public required StatusDto Status { get; set; }
        public int Quantity { get; set; } = 0;
    }
}
