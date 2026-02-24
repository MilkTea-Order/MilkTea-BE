using FluentValidation;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class MergeTableCommand : ICommand<MergeTableResult>
    {
        public int OrderID { get; set; }
        public int SourceTableId { get; set; }
    }

    public sealed class MergeTableCommandValidator : AbstractValidator<MergeTableCommand>
    {
        public MergeTableCommandValidator()
        {
            RuleFor(x => x.OrderID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName("OrderID");
            RuleFor(x => x.SourceTableId)
               .GreaterThan(0)
               .WithErrorCode(ErrorCode.E0001)
               .OverridePropertyName("SourceTableId");
        }
    }

    public class MergeTableCommandHandler(IOrderingUnitOfWork orderingUnitOfWork,
                                          ICurrentUser currentUser,
                                          ITableServices tableServices) : ICommandHandler<MergeTableCommand, MergeTableResult>
    {
        private readonly IOrderingUnitOfWork _vOrderUnitOfWork = orderingUnitOfWork;
        private readonly ICurrentUser _vCurrentUser = currentUser;
        private readonly ITableServices _vTableServices = tableServices;
        public async Task<MergeTableResult> Handle(MergeTableCommand command, CancellationToken cancellationToken)
        {
            MergeTableResult result = new();
            var mergeBy = _vCurrentUser.UserId;
            var rootOrder = await _vOrderUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderID);
            if (rootOrder is null)
            {
                return SendError(result, ErrorCode.E0001, "OrderID");
            }
            if (rootOrder.DinnerTableId == command.SourceTableId)
            {
                return SendError(result, ErrorCode.E0002, "OrderID");
            }

            // Check source table is inUsing and exist
            var isSourceTableValid = await _vTableServices.IsTableInUsing(command.SourceTableId, cancellationToken);
            // Not exist or not in using or empty
            if (!isSourceTableValid)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "SourceTableID");
            }
            var sourceOrder = await _vOrderUnitOfWork.Orders.GetOrderByTableAndStatusWithItemsAsync(command.SourceTableId, null, cancellationToken);
            // Source order is not exist
            if (sourceOrder is null)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0001, "SourceTableID");
            }
            await _vOrderUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {

                rootOrder.MergeFrom(sourceOrder, mergeBy);
                _vOrderUnitOfWork.Orders.Remove(sourceOrder);
                await _vOrderUnitOfWork.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (OrderNotEditableException)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "OrderID");
            }
            catch (OrderSourceCannotMergeException)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, "SourceTableID");
            }
            catch (Exception)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "MergeTable");
            }
        }
        private static MergeTableResult SendError(MergeTableResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
