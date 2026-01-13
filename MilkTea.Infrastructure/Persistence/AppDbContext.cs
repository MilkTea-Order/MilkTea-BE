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

        //public DbSet<AttendanceData> AttendanceData { get; set; }
        //public DbSet<AttendanceDataRecordType> AttendanceDataRecordTypes { get; set; }
        //public DbSet<AttendanceDataStatus> AttendanceDataStatuses { get; set; }
        //public DbSet<CollectAndSpend> CollectAndSpend { get; set; }
        //public DbSet<CollectAndSpendGroup> CollectAndSpendGroups { get; set; }

        //public DbSet<ImportFromSupplier> ImportFromSuppliers { get; set; }
        //public DbSet<ImportFromSuppliersStatus> ImportFromSuppliersStatus { get; set; }
        //public DbSet<Overtime> Overtime { get; set; }
        //public DbSet<OvertimeStatus> OvertimeStatus { get; set; }
        //public DbSet<Payroll> Payroll { get; set; }
        //public DbSet<PayrollDetail> PayrollDetail { get; set; }
        //public DbSet<PayrollStatus> PayrollStatus { get; set; }
        //public DbSet<Supplier> Suppliers { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Composite Keys
            //User
            modelBuilder.Entity<RoleDetail>()
                .HasKey(x => new { x.PermissionDetailID, x.RoleID });

            modelBuilder.Entity<UserAndPermissionDetail>()
                .HasKey(x => new { x.UserID, x.PermissionDetailID });

            modelBuilder.Entity<UserAndRole>()
                .HasKey(x => new { x.UserID, x.RoleID });

            //Order
            modelBuilder.Entity<MenuSize>()
             .HasKey(x => new { x.MenuID, x.SizeID });

            modelBuilder.Entity<PriceListDetail>()
                .HasKey(x => new { x.PriceListID, x.MenuID, x.SizeID });

            #endregion

            #region Foreign Key Relationships

            // ===== Employee Relationships =====
            modelBuilder.Entity<Employee>()
                .HasOne<Gender>(e => e.Gender)
                .WithMany()
                .HasForeignKey<Employee>(e => e.GenderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne<Position>(e => e.Position)
                .WithMany()
                .HasForeignKey<Employee>(e => e.PositionID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne<Status>(e => e.Status)
                .WithMany()
                .HasForeignKey<Employee>(e => e.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== User Relationships =====
            modelBuilder.Entity<User>()
                .HasOne<Employee>(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.EmployeesID)
                .OnDelete(DeleteBehavior.Restrict);

            // đảm bảo 1 Employee chỉ có 1 User (unique index ở mức DB)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.EmployeesID)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne<Status>(u => u.Status)
                .WithMany()
                .HasForeignKey<User>(u => u.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Permission Relationships =====
            modelBuilder.Entity<Permission>()
                .HasOne<PermissionGroup>(p => p.PermissionGroup)
                .WithMany()
                .HasForeignKey<Permission>(p => p.PermissionGroupID)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== PermissionDetail Relationships =====
            modelBuilder.Entity<PermissionDetail>()
                .HasOne<Permission>(pd => pd.Permission)
                .WithMany()
                .HasForeignKey<PermissionDetail>(pd => pd.PermissionID)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== PermissionGroup Relationships =====
            modelBuilder.Entity<PermissionGroup>()
                .HasOne<PermissionGroupType>(pg => pg.PermissionGroupType)
                .WithMany()
                .HasForeignKey<PermissionGroup>(pg => pg.PermissionGroupTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Role Relationships =====
            modelBuilder.Entity<Role>()
                .HasOne<Status>(r => r.Status)
                .WithMany()
                .HasForeignKey<Role>(r => r.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== RoleDetail Relationships =====
            modelBuilder.Entity<RoleDetail>()
                .HasOne<Role>(rd => rd.Role)
                .WithMany()
                .HasForeignKey<RoleDetail>(rd => rd.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoleDetail>()
                .HasOne<PermissionDetail>(rd => rd.PermissionDetail)
                .WithMany()
                .HasForeignKey<RoleDetail>(rd => rd.PermissionDetailID)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== UserAndRole Relationships =====
            modelBuilder.Entity<UserAndRole>()
                .HasOne<User>(ur => ur.User)
                .WithMany()
                .HasForeignKey<UserAndRole>(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAndRole>()
                .HasOne<Role>(ur => ur.Role)
                .WithMany()
                .HasForeignKey<UserAndRole>(ur => ur.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== UserAndPermissionDetail Relationships =====
            modelBuilder.Entity<UserAndPermissionDetail>()
                .HasOne<User>(up => up.User)
                .WithMany()
                .HasForeignKey<UserAndPermissionDetail>(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAndPermissionDetail>()
                .HasOne<PermissionDetail>(up => up.PermissionDetail)
                .WithMany()
                .HasForeignKey<UserAndPermissionDetail>(up => up.PermissionDetailID)
                .OnDelete(DeleteBehavior.Cascade);

            //  ===== RefreshToken Relationships =====
            modelBuilder.Entity<RefreshToken>()
                .HasOne<User>(rt => rt.User)
                .WithMany()
                .HasForeignKey<RefreshToken>(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //===== Order Relationships =====
            modelBuilder.Entity<Order>()
                .HasOne<DinnerTable>(o => o.DinnerTable)
                .WithMany()
                .HasForeignKey<Order>(o => o.DinnerTableID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne<StatusOfOrder>(o => o.StatusOfOrder)
                .WithMany()
                .HasForeignKey<Order>(o => o.StatusOfOrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne<PromotionOnTotalBill>(o => o.PromotionOnTotalBill)
                .WithMany()
                .HasForeignKey<Order>(o => o.PromotionID)
                .OnDelete(DeleteBehavior.SetNull);

            //// ===== OrdersDetail Relationships =====
            //modelBuilder.Entity<OrdersDetail>()
            //    .HasOne<Order>()
            //    .WithMany()
            //    .HasForeignKey(od => od.OrderID)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany<OrdersDetail>(o => o.OrdersDetails)
                .WithOne(od => od.Order)
                .HasForeignKey<OrdersDetail>(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrdersDetail>()
                .HasOne<Menu>(od => od.Menu)
                .WithMany()
                .HasForeignKey<OrdersDetail>(od => od.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdersDetail>()
                .HasOne<Size>(od => od.Size)
                .WithMany()
                .HasForeignKey<OrdersDetail>(od => od.SizeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdersDetail>()
                .HasOne<PriceList>(od => od.PriceList)
                .WithMany()
                .HasForeignKey<OrdersDetail>(od => od.PriceListID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdersDetail>()
                .HasOne<KindOfHotpot>(od => od.KindOfHotpot1)
                .WithMany()
                .HasForeignKey<OrdersDetail>(od => od.KindOfHotpot1ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdersDetail>()
                .HasOne<KindOfHotpot>(od => od.KindOfHotpot2)
                .WithMany()
                .HasForeignKey<OrdersDetail>(od => od.KindOfHotpot2ID)
                .OnDelete(DeleteBehavior.SetNull);

            //// ===== DinnerTable Relationships =====
            modelBuilder.Entity<DinnerTable>()
                .HasOne<StatusOfDinnerTable>(dt => dt.StatusOfDinnerTable)
                .WithMany()
                .HasForeignKey<DinnerTable>(dt => dt.StatusOfDinnerTableID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== Menu Relationships =====
            modelBuilder.Entity<Menu>()
                .HasOne<MenuGroup>(m => m.MenuGroup)
                .WithMany()
                .HasForeignKey<Menu>(m => m.MenuGroupID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Menu>()
                .HasOne<Unit>(m => m.Unit)
                .WithMany()
                .HasForeignKey<Menu>(m => m.UnitID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Menu>()
                .HasOne<Status>(m => m.Status)
                .WithMany()
                .HasForeignKey<Menu>(m => m.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== MenuGroup Relationships =====
            modelBuilder.Entity<MenuGroup>()
                .HasOne<Status>(mg => mg.Status)
                .WithMany()
                .HasForeignKey<MenuGroup>(mg => mg.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== MenuSize Relationships =====
            modelBuilder.Entity<MenuSize>()
                .HasOne<Menu>(ms => ms.Menu)
                .WithMany()
                .HasForeignKey<MenuSize>(ms => ms.MenuID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuSize>()
                .HasOne<Size>(ms => ms.Size)
                .WithMany()
                .HasForeignKey<MenuSize>(ms => ms.SizeID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== MenuAndMaterial Relationships =====
            modelBuilder.Entity<MenuAndMaterial>()
                .HasOne<Menu>(mm => mm.Menu)
                .WithMany()
                .HasForeignKey<MenuAndMaterial>(mm => mm.MenuID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuAndMaterial>()
                .HasOne<Material>(mm => mm.Material)
                .WithMany()
                .HasForeignKey<MenuAndMaterial>(mm => mm.MaterialsID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuAndMaterial>()
                .HasOne<Size>(mm => mm.Size)
                .WithMany()
                .HasForeignKey<MenuAndMaterial>(mm => mm.SizeID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== Material Relationships =====
            modelBuilder.Entity<Material>()
                .HasOne<MaterialsGroup>(m => m.MaterialsGroup)
                .WithMany()
                .HasForeignKey<Material>(m => m.MaterialsGroupID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Material>()
                .HasOne<MaterialsStatus>(m => m.MaterialsStatus)
                .WithMany()
                .HasForeignKey<Material>(m => m.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Material>()
                .HasOne<Unit>(m => m.Unit)
                .WithMany()
                .HasForeignKey<Material>(m => m.UnitID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Material>()
                .HasOne<Unit>(m => m.UnitMax)
                .WithMany()
                .HasForeignKey<Material>(m => m.UnitID_Max)
                .OnDelete(DeleteBehavior.SetNull);

            //// ===== PriceList Relationships =====
            modelBuilder.Entity<PriceList>()
                .HasOne<Currency>(pl => pl.Currency)
                .WithMany()
                .HasForeignKey<PriceList>(pl => pl.CurrencyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PriceList>()
                .HasOne<StatusOfPriceList>(pl => pl.StatusOfPriceList)
                .WithMany()
                .HasForeignKey<PriceList>(pl => pl.StatusOfPriceListID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== PriceListDetail Relationships =====
            modelBuilder.Entity<PriceListDetail>()
                .HasOne<PriceList>(pld => pld.PriceList)
                .WithMany()
                .HasForeignKey<PriceListDetail>(pld => pld.PriceListID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PriceListDetail>()
                .HasOne<Menu>(pld => pld.Menu)
                .WithMany()
                .HasForeignKey<PriceListDetail>(pld => pld.MenuID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PriceListDetail>()
                .HasOne<Size>(pld => pld.Size)
                .WithMany()
                .HasForeignKey<PriceListDetail>(pld => pld.SizeID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== PromotionOnTotalBill Relationships =====
            modelBuilder.Entity<PromotionOnTotalBill>()
                .HasOne<StatusOfPromotion>(p => p.StatusOfPromotion)
                .WithMany()
                .HasForeignKey<PromotionOnTotalBill>(p => p.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== Warehouse Relationships =====
            modelBuilder.Entity<Warehouse>()
                .HasOne<Material>(w => w.Material)
                .WithMany()
                .HasForeignKey<Warehouse>(w => w.MaterialsID)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Warehouse>()
            //    .HasOne<ImportFromSupplier>()
            //    .WithMany()
            //    .HasForeignKey(w => w.ImportFromSuppliersID)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Warehouse>()
                .HasOne<Status>(w => w.Status)
                .WithMany()
                .HasForeignKey<Warehouse>(w => w.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== WarehouseRollback Relationships =====
            modelBuilder.Entity<WarehouseRollback>()
                .HasOne<Order>(wr => wr.Order)
                .WithMany()
                .HasForeignKey<WarehouseRollback>(wr => wr.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WarehouseRollback>()
                .HasOne<Warehouse>(wr => wr.Warehouse)
                .WithMany()
                .HasForeignKey<WarehouseRollback>(wr => wr.WarehouseID)
                .OnDelete(DeleteBehavior.Restrict);

            //// ===== ImportFromSupplier Relationships =====
            //modelBuilder.Entity<ImportFromSupplier>()
            //    .HasOne<Supplier>()
            //    .WithMany()
            //    .HasForeignKey(ifs => ifs.SuppliersID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<ImportFromSupplier>()
            //    .HasOne<ImportFromSuppliersStatus>()
            //    .WithMany()
            //    .HasForeignKey(ifs => ifs.StatusID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// ===== Supplier Relationships =====
            //modelBuilder.Entity<Supplier>()
            //    .HasOne<Status>()
            //    .WithMany()
            //    .HasForeignKey(s => s.StatusID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// ===== Payroll Relationships =====
            //modelBuilder.Entity<Payroll>()
            //    .HasOne<Employee>()
            //    .WithMany()
            //    .HasForeignKey(p => p.EmployeeID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Payroll>()
            //    .HasOne<PayrollStatus>()
            //    .WithMany()
            //    .HasForeignKey(p => p.StatusID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// ===== PayrollDetail Relationships =====
            //modelBuilder.Entity<PayrollDetail>()
            //    .HasOne<Payroll>()
            //    .WithMany()
            //    .HasForeignKey(pd => pd.PayrollID)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// ===== Overtime Relationships =====
            //modelBuilder.Entity<Overtime>()
            //    .HasOne<Employee>()
            //    .WithMany()
            //    .HasForeignKey(o => o.EmployeeID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Overtime>()
            //    .HasOne<OvertimeStatus>()
            //    .WithMany()
            //    .HasForeignKey(o => o.StatusID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Overtime>()
            //    .HasOne<Payroll>()
            //    .WithMany()
            //    .HasForeignKey(o => o.PayrollID)
            //    .OnDelete(DeleteBehavior.SetNull);

            //// ===== AttendanceData Relationships =====
            //modelBuilder.Entity<AttendanceData>()
            //    .HasOne<Employee>()
            //    .WithMany()
            //    .HasForeignKey(a => a.EmployeeID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<AttendanceData>()
            //    .HasOne<AttendanceDataStatus>()
            //    .WithMany()
            //    .HasForeignKey(a => a.StatusID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<AttendanceData>()
            //    .HasOne<AttendanceDataRecordType>()
            //    .WithMany()
            //    .HasForeignKey(a => a.RecordTypeID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<AttendanceData>()
            //    .HasOne<Payroll>()
            //    .WithMany()
            //    .HasForeignKey(a => a.PayrollID)
            //    .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<AttendanceData>()
            //    .HasOne<Employee>()
            //    .WithMany()
            //    .HasForeignKey(a => a.EmployeeID_ChangeShift)
            //    .OnDelete(DeleteBehavior.SetNull);

            //// ===== CollectAndSpend Relationships =====
            //modelBuilder.Entity<CollectAndSpend>()
            //    .HasOne<CollectAndSpendGroup>()
            //    .WithMany()
            //    .HasForeignKey(cas => cas.CollectAndSpendGroupID)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// ===== Definition Relationships =====
            modelBuilder.Entity<Definition>()
                .HasOne<DefinitionGroup>(d => d.DefinitionGroup)
                .WithMany()
                .HasForeignKey<Definition>(d => d.DefinitionGroupID)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}

