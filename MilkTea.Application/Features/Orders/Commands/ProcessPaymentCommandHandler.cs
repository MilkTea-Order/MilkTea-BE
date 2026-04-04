using FluentValidation;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Features.Orders.Models.Results;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Common.Constants;
using MilkTea.Domain.Orders.Exceptions;
using MilkTea.Domain.Orders.Repositories;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Orders.Commands
{

    public class ProcessPaymentCommand : ICommand<ProcessPaymentResult>
    {
        public int OrderID { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public sealed class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
    {
        public ProcessPaymentCommandValidator()
        {
            RuleFor(x => x.OrderID)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName("orderID");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(ProcessPaymentCommand.PaymentMethod));
        }
    }
    public class ProcessPaymentCommandHandler(IOrderUnitOfWork orderUnitOfWork,
                                                IConfigurationService configurationService,
                                                IIdentifyServicePorts currentUser) : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
    {
        private readonly IOrderUnitOfWork _vOrderUnitOfWork = orderUnitOfWork;
        private readonly IConfigurationService _vConfigurationService = configurationService;
        private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
        public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            ProcessPaymentResult result = new();
            var order = await _vOrderUnitOfWork.Orders.GetOrderByIdWithItemsAsync(request.OrderID);
            if (order is null) return SendError(result, ErrorCode.E0001, request.OrderID.ToString());
            var billPrefix = await _vConfigurationService.GetBillPrefix();
            await _vOrderUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                order.Payment(billPrefix!, _vCurrentUser.UserId, request.PaymentMethod);
                await _vOrderUnitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (NotExistPaymentType)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0036, nameof(request.PaymentMethod));
            }
            catch (OrderNotEditableException)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E0042, nameof(request.OrderID));
            }
            catch (Exception)
            {
                await _vOrderUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "PaymentOrder");

            }
            return result;
        }

        private static ProcessPaymentResult SendError(ProcessPaymentResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
