using MilkTea.Application.Features.Catalog.Models.Dtos.Price;
using MilkTea.Application.Features.Catalog.Models.Dtos.Size;

namespace MilkTea.Application.Features.Catalog.Models.Dtos;

public class SizeAndPriceDto 
{
        public required SizeDto Size { get; set; }
        public required PriceDto Price { get; set; }
}