using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Models.Errors;
using MilkTea.Domain.Entities.Orders;

namespace MilkTea.Application.Models.Orders
{
    public class OrderItemValidation(OrderItemCommand item)
    {
        public OrderItemCommand Item { get; } = item;

        public Menu? Menu { get; private set; }

        public Size? Size { get; private set; }
        public decimal? Price { get; private set; }
        public List<MenuAndMaterial>? Recipe { get; private set; }

        public ValidationError? Error { get; private set; }

        public bool HasError => Error != null;


        public OrderItemValidation SetSuccess(
            Menu menu,
            Size size,
            decimal price,
            List<MenuAndMaterial> recipe)
        {
            Menu = menu;
            Size = size;
            Price = price;
            Recipe = recipe;
            return this;
        }

        public OrderItemValidation SetError(ValidationError error)
        {
            Error = error;
            return this;
        }
    }
}
