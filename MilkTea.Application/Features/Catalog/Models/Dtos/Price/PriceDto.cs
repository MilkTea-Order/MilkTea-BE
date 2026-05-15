using MilkTea.Application.Features.Catalog.Models.Dtos.Currency;

namespace MilkTea.Application.Features.Catalog.Models.Dtos.Price;

public class PriceDto
{
    public int PriceListId { get; set; }
    public decimal Price { get; set; }
    public required CurrencyDto Currency { get; set; }
}