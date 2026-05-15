namespace MilkTea.Application.Features.Catalog.Models.Dtos.Menu
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public string? MenuCode { get; set; }
        public string? MenuName { get; set; }
        public string? MenuImage { get; set; } = null;

        public int MenuGroupId { get; set; }
        public string? MenuGroupName { get; set; }

        public required StatusDto Status { get; set; }

        public int UnitId { get; set; }
        public string? UnitName { get; set; }

        public string? Note { get; set; }
    }

}
