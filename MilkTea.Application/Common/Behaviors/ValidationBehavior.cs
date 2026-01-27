using FluentValidation;
using MediatR;
using MilkTea.Application.Common.Exceptions;
using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        // format: { "E0001": "Username" } (or list if multiple fields)
        var errorData = failures
            .GroupBy(f => string.IsNullOrWhiteSpace(f.ErrorCode) ? ErrorCode.ValidationFailed : f.ErrorCode)
            .ToDictionary(
                g => g.Key,
                g =>
                {
                    var fields = g
                        .Select(x => x.PropertyName)
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Distinct()
                        .ToList();

                    return fields.Count == 1 ? (object)fields[0] : fields;
                }
            );

        var primaryCode = errorData.Count == 1 ? errorData.Keys.First() : null;
        throw new RequestValidationException("VALIDATION_ERROR", errorData, primaryCode);
    }
}

