using MilkTea.Shared.Domain.Services;
using MilkTea.Application.DTOs.Orders;

namespace MilkTea.Application.Results.Orders
{
    public class GetGroupMenuResult
    {
        public List<MenuGroupDto> GroupMenu { get; set; } = new();
        public StringListEntry ResultData { get; set; } = new();
    }
}
