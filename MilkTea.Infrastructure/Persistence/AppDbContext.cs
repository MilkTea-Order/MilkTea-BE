using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

    private static readonly TimeZoneInfo VietnamTimeZone = GetVietnamTimeZone();

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

    // ===== Finance =====
    public DbSet<CollectAndSpendGroupEntity> CollectAndSpendGroups { get; set; }
    public DbSet<CollectAndSpendEntity> CollectAndSpends { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var dateTimeConverter = new UtcToVietnamDbDateTimeConverter();
        var nullableConverter = new NullableUtcToVietnamDbDateTimeConverter();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }

                if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableConverter);
                }
            }
        }
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly,
            type => type.Namespace != null && (
                type.Namespace.Contains(".Configurations.Order") ||
                type.Namespace.Contains(".Configurations.Catalog") ||
                type.Namespace.Contains(".Configurations.TableManagement") ||
                type.Namespace.Contains(".Configurations.Inventory") ||
                type.Namespace.Contains(".Configurations.User") ||
                type.Namespace.Contains(".Configurations.Configuration") ||
                type.Namespace.Contains(".Configurations.Finance")
            )
        );
    }

    private sealed class UtcToVietnamDbDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        public UtcToVietnamDbDateTimeConverter()
            : base(
                v => ConvertUtcToVietnamDbTime(v),
                v => ConvertVietnamDbTimeToUtc(v)
            )
        {
        }
    }

    private sealed class NullableUtcToVietnamDbDateTimeConverter : ValueConverter<DateTime?, DateTime?>
    {
        public NullableUtcToVietnamDbDateTimeConverter()
            : base(
                v => v.HasValue ? ConvertUtcToVietnamDbTime(v.Value) : v,
                v => v.HasValue ? ConvertVietnamDbTimeToUtc(v.Value) : v
            )
        {
        }
    }

    private static TimeZoneInfo GetVietnamTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh"); // Linux/macOS
        }
    }

    private static DateTime ConvertUtcToVietnamDbTime(DateTime value)
    {
        var utc = value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);

        var local = TimeZoneInfo.ConvertTimeFromUtc(utc, VietnamTimeZone);

        return DateTime.SpecifyKind(local, DateTimeKind.Unspecified);
    }

    private static DateTime ConvertVietnamDbTimeToUtc(DateTime value)
    {
        var local = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);

        var utc = TimeZoneInfo.ConvertTimeToUtc(local, VietnamTimeZone);

        return DateTime.SpecifyKind(utc, DateTimeKind.Utc);
    }


}
