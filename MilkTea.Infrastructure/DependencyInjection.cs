using Microsoft.Extensions.DependencyInjection;
using MilkTea.Application.Ports.Hash.Password;
using MilkTea.Application.Ports.Hash.Permission;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Pricing.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Hash.Password;
using MilkTea.Infrastructure.Hash.Permission;
using MilkTea.Infrastructure.Repositories;
using MilkTea.Infrastructure.Repositories.Catalog;
using MilkTea.Infrastructure.Repositories.Configuration;
using MilkTea.Infrastructure.Repositories.Inventory;
using MilkTea.Infrastructure.Repositories.Ordering;
using MilkTea.Infrastructure.Repositories.Pricing;
using MilkTea.Infrastructure.Repositories.Users;

namespace MilkTea.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Ordering
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Catalog
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();

        // TableManagement
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<ITableRepository, TableRepository>();

        // Users
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();


        //Configuration
        services.AddScoped<IDefinitionRepository, DefinitionRepository>();

        //Pricing
        services.AddScoped<IPriceListRepository, PriceListRepository>();
        //services.AddScoped<IPromotionRepository, PromotionRepository>();

        //Invenotry
        //services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();

        // TODO: Add remaining repository registrations as needed



        // services.AddScoped<IGenderRepository, GenderRepository>();

        // Hashing 
        services.AddScoped<IPasswordHasher, RC2PasswordHasher>();
        services.AddScoped<IPermissionHasher, RC2PermissionHasher>();

        return services;
    }
}
