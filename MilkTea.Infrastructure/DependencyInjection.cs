using Microsoft.Extensions.DependencyInjection;
using MilkTea.Application.Features.Auth.Abstractions.Queries;
using MilkTea.Application.Features.Auth.Abstractions.Services;
using MilkTea.Application.Features.Catalog.Abstractions.Queries;
using MilkTea.Application.Features.Catalog.Abstractions.Services;
using MilkTea.Application.Features.Configuration.Abstractions.Services;
using MilkTea.Application.Features.Finance.Abstractions.Queries;
using MilkTea.Application.Features.Inventory.Abstractions;
using MilkTea.Application.Features.Orders.Abstractions;
using MilkTea.Application.Features.Orders.Abstractions.Services;
using MilkTea.Application.Features.User.Abstractions.Queries;
using MilkTea.Application.Features.User.Abstractions.Services;
using MilkTea.Domain.Auth.Repositories;
using MilkTea.Domain.Catalog;
using MilkTea.Domain.Catalog.Menu.Repositories;
using MilkTea.Domain.Catalog.Price.Repositories;
using MilkTea.Domain.Catalog.Size.Repositories;
using MilkTea.Domain.Catalog.Table.Repositories;
using MilkTea.Domain.Catalog.Unit.Repositories;
using MilkTea.Domain.Configuration.Repositories;
using MilkTea.Domain.Finance.Repositoties;
using MilkTea.Domain.Inventory.Repositories;
using MilkTea.Domain.Orders.Repositories;
using MilkTea.Domain.User.Repositories;
using MilkTea.Domain.Users.Repositories;
using MilkTea.Infrastructure.Features.Auth.Queries;
using MilkTea.Infrastructure.Features.Auth.Repositoties;
using MilkTea.Infrastructure.Features.Auth.Services;
using MilkTea.Infrastructure.Features.Catalog.Queries;
using MilkTea.Infrastructure.Features.Catalog.Repositories;
using MilkTea.Infrastructure.Features.Catalog.Services;
using MilkTea.Infrastructure.Features.Configuration.Repositories;
using MilkTea.Infrastructure.Features.Configuration.Services;
using MilkTea.Infrastructure.Features.Finance.Queries;
using MilkTea.Infrastructure.Features.Finance.Repositories;
using MilkTea.Infrastructure.Features.Inventory.Queries;
using MilkTea.Infrastructure.Features.Inventory.Repositories;
using MilkTea.Infrastructure.Features.Order.Queries;
using MilkTea.Infrastructure.Features.Order.Repositories;
using MilkTea.Infrastructure.Features.Order.Services;
using MilkTea.Infrastructure.Features.User.Queries;
using MilkTea.Infrastructure.Features.User.Repositories;
using MilkTea.Infrastructure.Features.User.Services;

namespace MilkTea.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Unit of Work
        services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
        services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
        services.AddScoped<IInventoryUnitOfWork, InventoryUnitOfWork>();
        services.AddScoped<IConfigurationUnitOfWork, ConfigurationUnitOfWork>();
        services.AddScoped<IFinanceUnitOfWork, FinanceUnitOfWork>();

        #region Order
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
        //Queries
        services.AddScoped<IOrderQuery, OrderQuery>();
        // Service
        services.AddScoped<IOrderServices, OrderServices>();
        #endregion Order

        #region Catalog
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<ICatalogUnitOfWork, CatalogUnitOfWork>();
        // Queries
        services.AddScoped<ICatalogQuery, CatalogQuery>();
        services.AddScoped<ITableQuery, TableQuery>();
        //Service 
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<IMaterialService, MaterialService>();
        #endregion Catalog



        #region Users
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        // Queries
        services.AddScoped<IUserQuery, UserQuery>();
        // Service
        services.AddScoped<IUserService, UserService>();
        #endregion Users


        #region Auth
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IResetPasswordTokenRepository, ResetPasswordTokenRepository>();
        // Queries
        services.AddScoped<IAuthQuery, AuthQuery>();
        services.AddScoped<IOtpQuery, OtpQuery>();
        // Service
        services.AddScoped<IAuthService, AuthService>();
        #endregion Auth

        #region Configuration
        services.AddScoped<IDefinitionRepository, DefinitionRepository>();
        //Service
        services.AddScoped<IConfigurationService, ConfigurationService>();
        #endregion Configuration

        //Pricing
        services.AddScoped<IPriceListRepository, PriceListRepository>();
        //services.AddScoped<IPromotionRepository, PromotionRepository>();

        #region Inventory
        //services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IInventoryQuery, InventoryQuery>();
        #endregion Inventory


        #region Finance
        services.AddScoped<IFinanceQuery, FinanceQuery>();
        services.AddScoped<IFinanceRepository, FinanceRepository>();
        #endregion Finance

        return services;
    }
}
