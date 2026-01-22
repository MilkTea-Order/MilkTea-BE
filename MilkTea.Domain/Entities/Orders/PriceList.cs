namespace MilkTea.Domain.Entities.Orders
{
    public class PriceList : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public int CurrencyID { get; set; }
        public int StatusOfPriceListID { get; set; }

        // Navigations
        public Currency? Currency { get; set; }
        public StatusOfPriceList? StatusOfPriceList { get; set; }
    }
}
