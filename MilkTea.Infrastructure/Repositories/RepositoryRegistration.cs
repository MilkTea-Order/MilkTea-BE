using Microsoft.Extensions.DependencyInjection;
using MilkTea.Domain.Respositories;
using MilkTea.Domain.Respositories.Configs;
using MilkTea.Domain.Respositories.Orders;
using MilkTea.Domain.Respositories.Users;
using MilkTea.Infrastructure.Repositories.Configs;
using MilkTea.Infrastructure.Repositories.Orders;
using MilkTea.Infrastructure.Repositories.Users;

namespace MilkTea.Infrastructure.Repositories;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IGenderRepository, GenderRepository>();
        //services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IDenifitionRepository, DenifitionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPriceListRepository, PriceListRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IDinnerTableRepository, DinnerTableRepository>();
        services.AddScoped<IStatusOfOrderRepository, StatusOfOrderRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();

        return services;
    }
}

