namespace MilkTea.Domain.Orders.Enums
{
    public class PaymentMethod
    {
        public const string CASH = "CASH";
        public const string BANK = "BANK";
        public const string GRAB = "GRAB";
        public const string SHOPEE = "SHOPEE";
        public static readonly HashSet<string> All = new()
        {
            CASH, BANK, GRAB, SHOPEE
        };

    }
}
