using MilkTea.Application.Commands.Orders;
using MilkTea.Application.Models.Errors;
using MilkTea.Domain.Respositories.Orders;

namespace MilkTea.Application.Services.Orders
{
    public class OrderRequestValidator(IDinnerTableRepository tableRepository)
    {
        private readonly IDinnerTableRepository _vTableRepository = tableRepository;
        public async Task<ValidationError?> Validate(CreateOrderCommand command)
        {
            //Dinner Table ID
            if (command.DinnerTableID <= 0)
                return ValidationError.InvalidData(nameof(command.DinnerTableID));

            if (await _vTableRepository.GetTableByIdAsync(command.DinnerTableID) == null)
                return ValidationError.NotExist(nameof(command.DinnerTableID));

            // Check ordered by
            if (command.OrderedBy.HasValue && command.OrderedBy.Value <= 0)
                return ValidationError.InvalidData(nameof(command.OrderedBy));

            // Check Items
            if (command.Items == null || command.Items.Count == 0)
                return ValidationError.InvalidData(nameof(command.Items));

            return null;
        }
    }
}
