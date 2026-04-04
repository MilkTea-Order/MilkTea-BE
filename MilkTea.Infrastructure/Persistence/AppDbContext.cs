using Microsoft.EntityFrameworkCore;
using MilkTea.Domain.Auth.Entities;
using MilkTea.Domain.Catalog;
using MilkTea.Domain.Catalog.Material.Entities;
using MilkTea.Domain.Catalog.Menu.Entities;
using MilkTea.Domain.Catalog.Price.Entities;
using MilkTea.Domain.Catalog.Size.Entities;
using MilkTea.Domain.Catalog.Table.Entities;
using MilkTea.Domain.Catalog.Unit.Entities;
using MilkTea.Domain.Configuration.Entities;
using MilkTea.Domain.Finance.Entities;
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

    // ==== Material =====
    public DbSet<MaterialEntity> Materials { get; set; }
    public DbSet<MaterialsGroupEntity> MaterialsGroups { get; set; }
    public DbSet<MenuMaterialRecipeEntity> MenuMaterialRecipes { get; set; }
    #endregion Catalog

    // ===== Inventory =====
    public DbSet<WarehouseEntity> Warehouses { get; set; }
    public DbSet<WarehouseRollbackEntity> WarehouseRollbacks { get; set; }
    public DbSet<ImportFromSupplierEntity> ImportFromSuppliers { get; set; }


    // ===== Users =====
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<OtpEntity> Otps { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<RoleDetailEntity> RoleDetails { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<PermissionDetailEntity> PermissionDetails { get; set; }
    public DbSet<PermissionGroupEntity> PermissionGroups { get; set; }
    public DbSet<PermissionGroupTypeEntity> PermissionGroupTypes { get; set; }
    // Persistence-only junction tables (not domain entities)
    public DbSet<UserAndRoleEntity> UserRoles { get; set; }
    public DbSet<UserAndPermissionDetailEntity> UserPermissions { get; set; }

    // ===== Configuration =====
    public DbSet<Definition> Definitions { get; set; }
    public DbSet<DefinitionGroup> DefinitionGroups { get; set; }

    // ===== Finance =====
    public DbSet<CollectAndSpendGroupEntity> CollectAndSpendGroups { get; set; }
    public DbSet<CollectAndSpendEntity> CollectAndSpends { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly,
            type => type.Namespace != null && (
                type.Namespace.Contains(".Configurations.Order") ||
                type.Namespace.Contains(".Configurations.Catalog") ||
                type.Namespace.Contains(".Configurations.TableManagement") ||
                type.Namespace.Contains(".Configurations.Inventory") ||
                type.Namespace.Contains(".Configurations.User") ||
                type.Namespace.Contains(".Configurations.Auth") ||
                type.Namespace.Contains(".Configurations.Configuration") ||
                type.Namespace.Contains(".Configurations.Finance")
            )
        );
    }
}
