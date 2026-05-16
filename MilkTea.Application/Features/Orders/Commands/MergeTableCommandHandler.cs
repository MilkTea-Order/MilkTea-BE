using FluentValidation;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{
    public class MergeTableCommand : ICommand<MergeTableResult>
    {
        public int OrderId { get; set; }
        public int SourceTableId { get; set; }
    }

    public sealed class MergeTableCommandValidator : AbstractValidator<MergeTableCommand>
    {
        public MergeTableCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(MergeTableCommand.OrderId));
            RuleFor(x => x.SourceTableId)
               .GreaterThan(0)
               .WithErrorCode(ErrorCode.E0001)
               .OverridePropertyName(nameof(MergeTableCommand.SourceTableId));
        }
    }

    public class MergeTableCommandHandler(IOrderUnitOfWork orderingUnitOfWork,
                                          IIdentifyServicePorts currentUser,
                                          ITableService tableServices) : ICommandHandler<MergeTableCommand, MergeTableResult>
    {
        private readonly IOrderUnitOfWork _vOrderUnitOfWork = orderingUnitOfWork;
        private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
        private readonly ITableService _vTableServices = tableServices;
        public async Task<MergeTableResult> Handle(MergeTableCommand command, CancellationToken cancellationToken)
        {
            MergeTableResult result = new();
            var mergeBy = _vCurrentUser.UserId;
            var rootOrder = await _vOrderUnitOfWork.Orders.GetOrderByIdWithItemsAsync(command.OrderId);
            if (rootOrder is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.OrderId));
            }
            if (rootOrder.DinnerTableId == command.SourceTableId)
            {
                return SendError(result, ErrorCode.E0002, nameof(command.SourceTableId));
            }

            // Check source table is inUsing and exist
            var isSourceTableValid = await _vTableServices.IsTableInUsing(command.SourceTableId, cancellationToken);
            // Not exist or not in using or empty
            if (!isSourceTableValid)
            {
                return SendError(result, ErrorCode.E0042, nameof(command.SourceTableId));
            }
            var sourceOrder = await _vOrderUnitOfWork.Orders.GetOrderByTableAndStatusWithItemsAsync(command.SourceTableId, null, cancellationToken);
            // Source order is not exist
            if (sourceOrder is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(command.SourceTableId));
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
                return SendError(result, ErrorCode.E0042, nameof(command.OrderId));
            }
            catch (OrderSourceCannotMergeException)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, nameof(command.SourceTableId));
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
