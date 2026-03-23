using FluentValidation;
using MilkTea.Application.Features.Finance.Abstractions.Queries;
using MilkTea.Application.Features.Finance.Models.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Finance.Queries
{
    public class GetSummaryTransaction : IQuery<GetSummaryTransationResult>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public sealed class GetSummaryTransationValidator : AbstractValidator<GetSummaryTransaction>
    {
        public GetSummaryTransationValidator()
        {
            RuleFor(x => x.FromDate)
                .NotNull()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetSummaryTransaction.FromDate));

            RuleFor(x => x.ToDate)
                .NotNull()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetSummaryTransaction.ToDate));

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetSummaryTransaction.FromDate) + nameof(GetSummaryTransaction.ToDate));
        }
    }

    public sealed class GetSummaryTransactionQueryHandler(IFinanceQuery financeQuery) : IQueryHandler<GetSummaryTransaction, GetSummaryTransationResult>
    {
        private readonly IFinanceQuery _vFinanceQuery = financeQuery;

        public async Task<GetSummaryTransationResult> Handle(GetSummaryTransaction query, CancellationToken cancellationToken)
        {
            var result = new GetSummaryTransationResult();
            var summary = await _vFinanceQuery.GetSummaryAsync(query.FromDate!.Value, query.ToDate!.Value, cancellationToken);
            result.Summary = summary;
            return result;
        }
    }
}
