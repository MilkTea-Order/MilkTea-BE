using FluentValidation;
using MediatR;
using MilkTea.Shared.Extensions;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.Exceptions;

namespace Shared.Abstractions.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
                                    (IEnumerable<IValidator<TRequest>> validators)
                                    : IPipelineBehavior<TRequest, TResponse>
                                    where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _vValidators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_vValidators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
                                        _vValidators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
                            .SelectMany(r => r.Errors)
                            .Where(f => f is not null)
                            .ToList();

        if (failures.Count == 0) return await next();

        var errorData = failures
            .GroupBy(f => f.ErrorCode)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var fields = g
                        .Select(x => x.PropertyName)
                        .Where(p => !p.IsNullOrWhiteSpace())
                        .Distinct()
                        .ToList();

                    return fields.Count == 1 ? (object)fields[0] : fields;
                }
            );

        var primaryCode = errorData.Count == 1 ? errorData.Keys.First() : null;
        throw new RequestValidationException("VALIDATION_ERROR", errorData, primaryCode);
    }
}

