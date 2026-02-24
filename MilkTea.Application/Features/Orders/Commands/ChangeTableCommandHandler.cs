using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class ChangeTableCommand : ICommand<ChangeTableResult>
    {
        public int OrderID { get; set; }
        public int NewDinnerTableID { get; set; }
    }

    public sealed class ChangeTableCommandHandler(
                                            IOrderingUnitOfWork orderingUnitOfWork,
                                            IOrderQueries orderQueries,
                                            ICurrentUser currentUser,
                                            ITableServices tableServices) : ICommandHandler<ChangeTableCommand, ChangeTableResult>
    {
        private readonly IOrderingUnitOfWork _vOrderingUnitOfWork = orderingUnitOfWork;
        private readonly IOrderQueries _vOrderQueries = orderQueries;
        private readonly ICurrentUser _vCurrentUser = currentUser;
        private readonly ITableServices _vTableServices = tableServices;

        public async Task<ChangeTableResult> Handle(ChangeTableCommand command, CancellationToken cancellationToken)
        {
            var result = new ChangeTableResult();
            var createdBy = _vCurrentUser.UserId;

            // Check order exist
            var order = await _vOrderingUnitOfWork.Orders.GetOrderByIdAsync(command.OrderID);
            if (order is null)
            {
                return SendError(result, ErrorCode.E0001, "OrderID");
            }
            // Check new table 
            var isInUsingTable = await _vTableServices.IsTableInUsing(command.NewDinnerTableID, cancellationToken);
            // Not exist or not in using table
            if (!isInUsingTable)
            {
                return SendError(result, ErrorCode.E0042, "NewDinnerTableID");
            }
            var isAvailableTable = await _vOrderQueries.IsTableAvailable(command.NewDinnerTableID, cancellationToken);
            //Table is used by other order
            if (!isAvailableTable)
            {
                return SendError(result, ErrorCode.E0042, "NewDinnerTableID");
            }
            await _vOrderingUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                order.ChangTable(command.NewDinnerTableID, createdBy);
                await _vOrderingUnitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (OrderNotEditableException)
            {
                await _vOrderingUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "OrderID");
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
