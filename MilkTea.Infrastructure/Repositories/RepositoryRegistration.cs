using Microsoft.Extensions.DependencyInjection;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Pricing.Repositories;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Infrastructure.Repositories.Identity;
using MilkTea.Infrastructure.Repositories.Catalog;
using MilkTea.Infrastructure.Repositories.Ordering;
using MilkTea.Infrastructure.Repositories.TableManagement;
using MilkTea.Infrastructure.Repositories.Pricing;
using MilkTea.Infrastructure.Repositories.Inventory;
using MilkTea.Infrastructure.Repositories.Configuration;

namespace MilkTea.Infrastructure.Repositories;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Identity repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        //services.AddScoped<IRoleRepository, RoleRepository>();

        // Catalog repositories
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();

        // Table management repositories
        services.AddScoped<IDinnerTableRepository, DinnerTableRepository>();
        services.AddScoped<ITableRepository, TableRepository>();

        // Configuration repositories
        services.AddScoped<IDefinitionRepository, DefinitionRepository>();

        // Pricing repositories
        services.AddScoped<IPriceListRepository, PriceListRepository>();

        // Inventory repositories
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();

        // Ordering repositories
        services.AddScoped<IOrderRepository, OrderRepository>();


        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

