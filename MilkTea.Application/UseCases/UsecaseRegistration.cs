using Microsoft.Extensions.DependencyInjection;
using MilkTea.Application.Services.Orders;
using MilkTea.Application.UseCases.Orders;
using MilkTea.Application.UseCases.Users;

namespace MilkTea.Application.UseCases
{
    public static class UsecaseRegistration
    {
        public static IServiceCollection AddUsecases(this IServiceCollection services)
        {
            // Register your use cases here
            services.AddScoped<LoginWithUserNameUseCase>();
            services.AddScoped<UpdatePasswordUseCase>();
            services.AddScoped<GetTableByStatusUseCase>();
            services.AddScoped<GetGroupMenuUseCase>();
            services.AddScoped<GetMenuItemsOfGroupUseCase>();
            services.AddScoped<GetMenuSizeOfMenuUseCase>();
            services.AddScoped<EmployeeUpdateProfileUseCase>();
            services.AddScoped<AdminUpdateUserUseCase>();
            services.AddScoped<GetGroupMenuAvaliableUseCase>();
            services.AddScoped<GetMenuItemsAvaliableOfGroupUseCase>();
            services.AddScoped<CreateOrderUseCase>();
            services.AddScoped<GetOrdersByOrderByAndStatusUseCase>();
            services.AddScoped<GetOrderDetailByIDAndStatusUseCase>();
            services.AddScoped<CancelOrderUseCase>();
            services.AddScoped<CancelOrderDetailsUseCase>();
            services.AddScoped<LogoutUseCase>();
            services.AddScoped<GetUserProfileUseCase>();
            services.AddScoped<RefreshAccessTokenUseCase>();
            // ===== Orders – Validators =====
            services.AddScoped<OrderRequestValidator>();
            services.AddScoped<OrderItemValidator>();

            // ===== Orders – Services =====
            services.AddScoped<OrderStockService>();
            services.AddScoped<OrderPricingService>();
            services.AddScoped<OrderFactory>();
            return services;
        }
    }
}
