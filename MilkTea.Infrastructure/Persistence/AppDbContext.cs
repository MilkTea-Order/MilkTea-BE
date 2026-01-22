using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Entities.Config;
using MilkTea.Domain.Entities.Orders;
using MilkTea.Domain.Entities.Users;

namespace MilkTea.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        #region DbSet

        // ===== User and Permission Management =====
        public DbSet<UserAndPermissionDetail> UserAndPermissionDetail { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<PermissionDetail> PermissionDetail { get; set; }
        public DbSet<PermissionGroup> PermissionGroup { get; set; }
        public DbSet<PermissionGroupType> PermissionGroupType { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleDetail> RoleDetail { get; set; }
        public DbSet<UserAndRole> UserAndRole { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Order
        public DbSet<Currency> Currency { get; set; }
        public DbSet<DinnerTable> DinnerTable { get; set; }
        public DbSet<KindOfHotpot> KindOfHotpot { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialsGroup> MaterialsGroup { get; set; }
        public DbSet<MaterialsStatus> MaterialsStatus { get; set; }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuAndMaterial> MenuAndMaterials { get; set; }
        public DbSet<MenuGroup> MenuGroup { get; set; }
        public DbSet<MenuSize> MenuSize { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrdersDetail> OrdersDetail { get; set; }
        public DbSet<PriceList> PriceList { get; set; }
        public DbSet<PriceListDetail> PriceListDetail { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<StatusOfDinnerTable> StatusOfDinnerTable { get; set; }
        public DbSet<StatusOfOrder> StatusOfOrder { get; set; }
        public DbSet<StatusOfPriceList> StatusOfPriceList { get; set; }
        public DbSet<StatusOfPromotion> StatusOfPromotion { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<PromotionOnTotalBill> PromotionOnTotalBill { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<WarehouseRollback> WarehouseRollback { get; set; }


        public DbSet<Definition> Definition { get; set; }
        public DbSet<DefinitionGroup> DefinitionGroups { get; set; }



        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
