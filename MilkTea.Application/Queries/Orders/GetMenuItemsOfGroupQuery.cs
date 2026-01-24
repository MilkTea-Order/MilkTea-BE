namespace MilkTea.Application.Queries.Orders
{
    public sealed class GetMenuItemsOfGroupQuery
    {
        public int GroupId { get; set; }
        public int? MenuStatusId { get; set; }
    }
}

