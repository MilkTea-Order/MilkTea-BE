using FluentValidation;
using MilkTea.Application.Features.Finance.Abstractions.Queries;
using MilkTea.Application.Features.Finance.Models.Results;
using MilkTea.Application.Features.Users.Abstractions.Services;
using MilkTea.Application.Ports.Users;
using MilkTea.Domain.Finance.Entities;
using MilkTea.Domain.Finance.Repositoties;
using MilkTea.Domain.SharedKernel.Constants;
using MilkTea.Shared.Domain.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Finance.Commands
{
    public class CreateFinanceTransactionCommand : ICommand<CreateFinanceTransactionResult>
    {
        public int TransactionGroupId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public int TransactionBy { get; set; }
        public DateTime TransactionDate { get; set; }

        public string? Note { get; set; }
    }

    public sealed class CreateFinanceTransactionCommandValidator : AbstractValidator<CreateFinanceTransactionCommand>
    {
        public CreateFinanceTransactionCommandValidator()
        {
            RuleFor(x => x.TransactionGroupId)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(CreateFinanceTransactionCommand.TransactionGroupId));

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(CreateFinanceTransactionCommand.Name));

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(CreateFinanceTransactionCommand.Amount));

            RuleFor(x => x.TransactionBy)
                .GreaterThan(0)
                .WithErrorCode(ErrorCode.E0001)
                .OverridePropertyName(nameof(CreateFinanceTransactionCommand.TransactionBy));

            RuleFor(x => x.TransactionDate)
                .NotEmpty()
                .Must(date => date.Date <= DateTime.UtcNow.Date)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(CreateFinanceTransactionCommand.TransactionDate));
            RuleFor(x => x.Note)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .When(x => x.Note != null)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(CreateFinanceTransactionCommand.Note));
        }
    }


    public class CreateFinanceTransactionCommandHandler(IIdentifyServicePorts currentUser,
                                                            IFinanceQuery financeQuery,
                                                            IUserServices userServices,
                                                            IFinanceUnitOfWork financeUnitOfWork) : ICommandHandler<CreateFinanceTransactionCommand, CreateFinanceTransactionResult>
    {
        private readonly IIdentifyServicePorts _vCurrentUser = currentUser;
        private readonly IFinanceQuery _vFinanceQuery = financeQuery;
        private readonly IUserServices _vUserServices = userServices;
        private readonly IFinanceUnitOfWork _vFinanceUnitOfWork = financeUnitOfWork;
        public async Task<CreateFinanceTransactionResult> Handle(CreateFinanceTransactionCommand commnad, CancellationToken cancellationToken)
        {
            var result = new CreateFinanceTransactionResult();
            var groupTransaction = (await _vFinanceQuery.GetCollectionAndSpendGroupsAsync(cancellationToken)).FirstOrDefault(x => x.Id == commnad.TransactionGroupId);
            if (groupTransaction is null)
            {
                return SendError(result, ErrorCode.E0001, nameof(CreateFinanceTransactionCommand.TransactionGroupId));
            }
            var isExistTransactionBy = await _vUserServices.isExist(commnad.TransactionBy, cancellationToken);
            if (!isExistTransactionBy)
            {
                return SendError(result, ErrorCode.E0001, nameof(CreateFinanceTransactionCommand.TransactionBy));
            }

            if (!groupTransaction.Name.Equals(Denifitions.COLLECT_TYPE, StringComparison.OrdinalIgnoreCase))
            {
                commnad.Amount = -commnad.Amount;
            }

            await _vFinanceUnitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var transaction = CollectAndSpendEntity.Create(commnad.TransactionGroupId, commnad.Name, commnad.TransactionBy, commnad.TransactionDate,
                                                                                    _vCurrentUser.UserId, commnad.Amount, commnad.Note);
                await _vFinanceUnitOfWork.Finance.AddAsync(transaction, cancellationToken);
                await _vFinanceUnitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _vFinanceUnitOfWork.RollbackTransactionAsync(cancellationToken);
                return SendError(result, ErrorCode.E9999, "CreateFinanceTransaction");
            }
            return result;
        }
        private static CreateFinanceTransactionResult SendError(CreateFinanceTransactionResult result, string errorCode, params string[] values)
        {
            if (values is { Length: > 0 })
                result.ResultData.Add(errorCode, values.ToList());
            return result;
        }
    }
}
