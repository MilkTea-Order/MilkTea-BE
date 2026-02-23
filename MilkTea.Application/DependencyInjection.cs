using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Behaviors;
using System.Reflection;

namespace MilkTea.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        return services;
    }
}
