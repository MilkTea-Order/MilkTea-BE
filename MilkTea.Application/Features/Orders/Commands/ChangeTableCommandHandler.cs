using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class ChangeTableCommand : ICommand<ChangeTableResult>
    {
        public int OrderId { get; set; }
        public int NewDinnerTableId { get; set; }
    }

    public sealed class ChangeTableCommandHandler(
                                            IOrderUnitOfWork orderingUnitOfWork,
                                            IOrderQuery orderQueries,
                                            IIdentifyServicePorts currentUser,
                                            ITableService tableServices) : ICommandHandler<ChangeTableCommand, ChangeTableResult>
    {
        private readonly IOrderUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        private readonly IOrderQuery _vOrderQueries = orderQueries;
        private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
        private readonly ITableService _vTableServices = tableServices;

        public async Task<ChangeTableResult> Handle(ChangeTableCommand command, CancellationToken cancellationToken)
        {
            var result = new ChangeTableResult();
            var createdBy = _vCurrentUser.UserId;

            // Check order exist
            var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdAsync(command.OrderId);
            if (order is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.OrderId));
            }
            // Check new table 
            var isInUsingTable = await _vTableServices.IsTableInUsing(command.NewDinnerTableId, cancellationToken);
            // Not exist or not in using table
            if (!isInUsingTable)
            {
                return SendError(result, ErrorCode.E0042, nameof(command.NewDinnerTableId));
            }
            var isAvailableTable = await _vOrderQueries.IsTableAvailable(command.NewDinnerTableId, cancellationToken);
            //Table is used by other order
            if (!isAvailableTable)
            {
                return SendError(result, ErrorCode.E0042, nameof(command.NewDinnerTableId));
            }
            await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                order.ChangTable(command.NewDinnerTableId, createdBy);
                await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (OrderNotEditableException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, nameof(command.OrderId));
            }
            catch (Exception)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "ChangeTable");
            }
        }
        private static ChangeTableResult SendError(ChangeTableResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
