using FluentValidation;
using MilkTea.Application.Features.Finance.Abstractions.Queries;
using MilkTea.Application.Features.Finance.Models.Results;
using Shared.Abstractions.CQRS;

namespace MilkTea.Application.Features.Finance.Queries
{
    public class GetTransactionGroupQuery : IQuery<GetTransactionGroupResult>
    {
    }

    public sealed class GetTransactionGroupQueryValidator : AbstractValidator<GetTransactionGroupQuery>
    {
        public GetTransactionGroupQueryValidator() { }
    }

    public class GetTransactionGroupQueryHandler(IFinanceQuery financeQuery) : IQueryHandler<GetTransactionGroupQuery, GetTransactionGroupResult>
    {
        private readonly IFinanceQuery _vFinanceQuery = financeQuery;
        public async Task<GetTransactionGroupResult> Handle(GetTransactionGroupQuery request, CancellationToken cancellationToken)
        {
            var result = new GetTransactionGroupResult();
            var groups = await _vFinanceQuery.GetCollectionAndSpendGroupsAsync(cancellationToken);
            result.Groups = groups;
            return result;
        }
    }
}