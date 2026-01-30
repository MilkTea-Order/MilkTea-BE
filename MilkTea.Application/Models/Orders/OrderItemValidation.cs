using MilkTea.Application.Features.Orders.Commands;
using MilkTea.Application.Models.Errors;
using MilkTea.Domain.Catalog.Entities;

namespace MilkTea.Application.Models.Orders
{
    public class OrderItemValidation(OrderItemCommand item)
    {
        public OrderItemCommand Item { get; } = item;

        public Domain.Catalog.Entities.Menu? Menu { get; private set; }

        public Domain.Catalog.Entities.Size? Size { get; private set; }
        public decimal? Price { get; private set; }
        public List<Domain.Inventory.Entities.MenuMaterialRecipe>? Recipe { get; private set; }

        public ValidationError? Error { get; private set; }

        public bool HasError => Error != null;


        public OrderItemValidation SetSuccess(
            Domain.Catalog.Entities.Menu menu,
            Domain.Catalog.Entities.Size size,
            decimal price,
            List<Domain.Inventory.Entities.MenuMaterialRecipe> recipe)
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
