namespace MilkTea.Application.Commands.Orders
{
    public class GetOrdersByOrderByAndStatusCommand
    {
        public int OrderBy { get; set; }
        public int StatusId { get; set; }
    }
}
