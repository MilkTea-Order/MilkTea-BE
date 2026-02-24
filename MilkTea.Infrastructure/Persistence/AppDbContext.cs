using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Catalog.Entities;
using MilkTea.Domain.Catalog.Entities.Menu;
using MilkTea.Domain.Catalog.Entities.Price;
using MilkTea.Domain.Catalog.Entities.Size;
using MilkTea.Domain.Catalog.Entities.Table;
using MilkTea.Domain.Catalog.Entities.Unit;
using MilkTea.Domain.Configuration.Entities;
using MilkTea.Domain.Inventory.Entities;
using MilkTea.Domain.Orders.Entities;
using MilkTea.Domain.Users.Entities;

namespace MilkTea.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region DbSet

    // ===== Order =====
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }

    // ===== Catalog =====
    #region Catalog

    // ===== Menu =====
    public DbSet<MenuGroupEntity> MenuGroups { get; set; }
    public DbSet<MenuEntity> Menus { get; set; }
    public DbSet<MenuSizeEntity> MenuSizes { get; set; }
    public DbSet<KindOfHotpot> KindOfHotpots { get; set; }

    // ===== Price List =====
    public DbSet<PriceListEntity> PriceLists { get; set; }
    public DbSet<PriceListDetailEntity> PriceListDetails { get; set; }
    public DbSet<CurrencyEntity> Currencies { get; set; }

    // ===== Promotion =====
    public DbSet<PromotionOnTotalBill> Promotions { get; set; }

    // ===== Size =====
    public DbSet<SizeEntity> Sizes { get; set; }

    // ===== Table =====
    public DbSet<TableEntity> Tables { get; set; }

    // ===== Unit =====
    public DbSet<UnitEntity> Units { get; set; }
    #endregion Catalog

    // ===== Inventory =====
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialsGroup> MaterialsGroups { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<WarehouseRollback> WarehouseRollbacks { get; set; }
    public DbSet<MenuMaterialRecipe> MenuMaterialRecipes { get; set; }

    // ===== Users =====
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleDetail> RoleDetails { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionDetail> PermissionDetails { get; set; }
    public DbSet<PermissionGroup> PermissionGroups { get; set; }
    public DbSet<PermissionGroupType> PermissionGroupTypes { get; set; }
    // Persistence-only junction tables (not domain entities)
    public DbSet<UserAndRole> UserRoles { get; set; }
    public DbSet<UserAndPermissionDetail> UserPermissions { get; set; }

    // ===== Configuration =====
    public DbSet<Definition> Definitions { get; set; }
    public DbSet<DefinitionGroup> DefinitionGroups { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from the new namespaces only
        // Filter to only include configurations from the new module-based folders
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly,
            type => type.Namespace != null && (
                type.Namespace.Contains(".Configurations.Order") ||
                type.Namespace.Contains(".Configurations.Catalog") ||
                type.Namespace.Contains(".Configurations.TableManagement") ||
                type.Namespace.Contains(".Configurations.Inventory") ||
                type.Namespace.Contains(".Configurations.Users") ||
                type.Namespace.Contains(".Configurations.Configuration")
            )
        );
    }
}
