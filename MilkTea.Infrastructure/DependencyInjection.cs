using Microsoft.Extensions.DependencyInjection;
using MilkTea.Domain.Catalog.Repositories;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.Pricing.Repositories;
using MilkTea.Domain.SharedKernel.Repositories;
using MilkTea.Infrastructure.Repositories;
using MilkTea.Infrastructure.Repositories.Catalog;
using MilkTea.Infrastructure.Repositories.Identity;
using MilkTea.Infrastructure.Repositories.Ordering;
using MilkTea.Infrastructure.Repositories.TableManagement;

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

        // TableManagement
        services.AddScoped<IDinnerTableRepository, DinnerTableRepository>();

        // Identity
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // TODO: Add remaining repository registrations as needed
        // services.AddScoped<IPriceListRepository, PriceListRepository>();
        // services.AddScoped<IPromotionRepository, PromotionRepository>();
        // services.AddScoped<IMaterialRepository, MaterialRepository>();
        // services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        // services.AddScoped<IDefinitionRepository, DefinitionRepository>();
        // services.AddScoped<ISizeRepository, SizeRepository>();
        // services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        // services.AddScoped<IRoleRepository, RoleRepository>();
        // services.AddScoped<IPermissionRepository, PermissionRepository>();
        // services.AddScoped<IGenderRepository, GenderRepository>();

        return services;
    }
}
