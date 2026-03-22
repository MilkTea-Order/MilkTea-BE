using FluentValidation;
using MilkTea.Application.Features.Finance.Abstractions.Queries;
using MilkTea.Application.Features.Finance.Models.Results;
using MilkTea.Domain.SharedKernel.Constants;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Finance.Queries
{
    public class GetSummaryCollectAndSpend : IQuery<GetSummaryCollectAndSpendResult>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public sealed class GetSummaryCollectAndSpendValidator : AbstractValidator<GetSummaryCollectAndSpend>
    {
        public GetSummaryCollectAndSpendValidator()
        {
            RuleFor(x => x.FromDate)
                .NotNull()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetSummaryCollectAndSpend.FromDate));

            RuleFor(x => x.ToDate)
                .NotNull()
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetSummaryCollectAndSpend.ToDate));

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate)
                .WithErrorCode(ErrorCode.E0036)
                .OverridePropertyName(nameof(GetSummaryCollectAndSpend.FromDate) + nameof(GetSummaryCollectAndSpend.ToDate));
        }
    }

    public sealed class GetSummaryCollectAndSpendHandler(IFinanceQuery financeQuery) : IQueryHandler<GetSummaryCollectAndSpend, GetSummaryCollectAndSpendResult>
    {
        private readonly IFinanceQuery _vFinanceQuery = financeQuery;

        public async Task<GetSummaryCollectAndSpendResult> Handle(GetSummaryCollectAndSpend query, CancellationToken cancellationToken)
        {
            var result = new GetSummaryCollectAndSpendResult();
            var summary = await _vFinanceQuery.GetSummaryAsync(query.FromDate!.Value, query.ToDate!.Value, cancellationToken);
            result.Summary = summary;
            return result;
        }
    }
}
