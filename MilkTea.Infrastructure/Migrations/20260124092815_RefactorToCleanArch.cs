using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MilkTea.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToCleanArch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "definitiongroup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_definitiongroup", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gender",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gender", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "kindofhotpot",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kindofhotpot", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "materialsgroup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialsgroup", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "materialsstatus",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialsstatus", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissiongrouptype",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissiongrouptype", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "size",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RankIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_size", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "statusofdinnertable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusofdinnertable", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "statusoforder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusoforder", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "statusofpricelist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusofpricelist", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "statusofpromotion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusofpromotion", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "definition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValueImage = table.Column<byte[]>(type: "longblob", nullable: true),
                    IsEdit = table.Column<int>(type: "int", nullable: false),
                    IsEncrypt = table.Column<int>(type: "int", nullable: false),
                    DefinitionGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_definition", x => x.ID);
                    table.ForeignKey(
                        name: "FK_definition_definitiongroup_DefinitionGroupID",
                        column: x => x.DefinitionGroupID,
                        principalTable: "definitiongroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissiongroup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionGroupTypeID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissiongroup", x => x.ID);
                    table.ForeignKey(
                        name: "FK_permissiongroup_permissiongrouptype_PermissionGroupTypeID",
                        column: x => x.PermissionGroupTypeID,
                        principalTable: "permissiongrouptype",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GenderID = table.Column<int>(type: "int", nullable: false),
                    BirthDay = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentityCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartWorkingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndWorkingDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PositionID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    CellPhone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalaryByHour = table.Column<int>(type: "int", nullable: true),
                    ShiftFrom = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ShiftTo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CalcSalaryByMinutes = table.Column<int>(type: "int", nullable: true),
                    TimekeepingOther = table.Column<int>(type: "int", nullable: false),
                    Bank_AccountNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bank_AccountName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bank_QRCode = table.Column<byte[]>(type: "longblob", nullable: true),
                    BankName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsBreakTime = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    BreakTimeFrom = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BreakTimeTo = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_employees_gender_GenderID",
                        column: x => x.GenderID,
                        principalTable: "gender",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employees_position_PositionID",
                        column: x => x.PositionID,
                        principalTable: "position",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employees_status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "menugroup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menugroup", x => x.ID);
                    table.ForeignKey(
                        name: "FK_menugroup_status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.ID);
                    table.ForeignKey(
                        name: "FK_role_status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "dinnertable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Position = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    Longs = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    EmptyPicture = table.Column<byte[]>(type: "longblob", nullable: false),
                    UsingPicture = table.Column<byte[]>(type: "longblob", nullable: false),
                    StatusOfDinnerTableID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dinnertable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dinnertable_statusofdinnertable_StatusOfDinnerTableID",
                        column: x => x.StatusOfDinnerTableID,
                        principalTable: "statusofdinnertable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pricelist",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CurrencyID = table.Column<int>(type: "int", nullable: false),
                    StatusOfPriceListID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricelist", x => x.ID);
                    table.ForeignKey(
                        name: "FK_pricelist_currency_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "currency",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pricelist_statusofpricelist_StatusOfPriceListID",
                        column: x => x.StatusOfPriceListID,
                        principalTable: "statusofpricelist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "promotionontotalbill",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StopDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProPercent = table.Column<int>(type: "int", nullable: true),
                    ProAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotionontotalbill", x => x.ID);
                    table.ForeignKey(
                        name: "FK_promotionontotalbill_statusofpromotion_StatusID",
                        column: x => x.StatusID,
                        principalTable: "statusofpromotion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnitID = table.Column<int>(type: "int", nullable: true),
                    UnitID_Max = table.Column<int>(type: "int", nullable: true),
                    StyleQuantity = table.Column<int>(type: "int", nullable: true),
                    MaterialsGroupID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_materials_materialsgroup_MaterialsGroupID",
                        column: x => x.MaterialsGroupID,
                        principalTable: "materialsgroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_materials_materialsstatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "materialsstatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_materials_unit_UnitID",
                        column: x => x.UnitID,
                        principalTable: "unit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_materials_unit_UnitID_Max",
                        column: x => x.UnitID_Max,
                        principalTable: "unit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionGroupID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.ID);
                    table.ForeignKey(
                        name: "FK_permission_permissiongroup_PermissionGroupID",
                        column: x => x.PermissionGroupID,
                        principalTable: "permissiongroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeesID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StoppedBy = table.Column<int>(type: "int", nullable: true),
                    StoppedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdatedBy = table.Column<int>(type: "int", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Password_ResetBy = table.Column<int>(type: "int", nullable: true),
                    Password_ResetDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_users_employees_EmployeesID",
                        column: x => x.EmployeesID,
                        principalTable: "employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "menu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Formula = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AvatarPicture = table.Column<byte[]>(type: "longblob", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuGroupID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    TasteQTy = table.Column<int>(type: "int", nullable: true),
                    PrintSticker = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu", x => x.ID);
                    table.ForeignKey(
                        name: "FK_menu_menugroup_MenuGroupID",
                        column: x => x.MenuGroupID,
                        principalTable: "menugroup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_menu_status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_menu_unit_UnitID",
                        column: x => x.UnitID,
                        principalTable: "unit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DinnerTableID = table.Column<int>(type: "int", nullable: false),
                    OrderBy = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CancelledBy = table.Column<int>(type: "int", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StatusOfOrderID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentedBy = table.Column<int>(type: "int", nullable: true),
                    PaymentedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PaymentedTotal = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PaymentedType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddNoteBy = table.Column<int>(type: "int", nullable: true),
                    AddNoteDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ChangeBy = table.Column<int>(type: "int", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MergedBy = table.Column<int>(type: "int", nullable: true),
                    MergedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BillNo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PromotionID = table.Column<int>(type: "int", nullable: true),
                    PromotionPercent = table.Column<int>(type: "int", nullable: true),
                    PromotionAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ActionBy = table.Column<int>(type: "int", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_orders_dinnertable_DinnerTableID",
                        column: x => x.DinnerTableID,
                        principalTable: "dinnertable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_promotionontotalbill_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "promotionontotalbill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_statusoforder_StatusOfOrderID",
                        column: x => x.StatusOfOrderID,
                        principalTable: "statusoforder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaterialsID = table.Column<int>(type: "int", nullable: false),
                    QuantityImport = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    QuantityCurrent = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PriceImport = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AmountTotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ImportFromSuppliersID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_warehouse_materials_MaterialsID",
                        column: x => x.MaterialsID,
                        principalTable: "materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_warehouse_status_StatusID",
                        column: x => x.StatusID,
                        principalTable: "status",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permissiondetail",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissiondetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_permissiondetail_permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "permission",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "refreshtokens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsRevoked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshtokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_refreshtokens_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userandrole",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userandrole", x => new { x.UserID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_userandrole_role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userandrole_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "menu_size",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_size", x => new { x.MenuID, x.SizeID });
                    table.ForeignKey(
                        name: "FK_menu_size_menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_menu_size_size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "menuandmaterials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false),
                    MaterialsID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menuandmaterials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_menuandmaterials_materials_MaterialsID",
                        column: x => x.MaterialsID,
                        principalTable: "materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_menuandmaterials_menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_menuandmaterials_size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pricelistdetail",
                columns: table => new
                {
                    PriceListID = table.Column<int>(type: "int", nullable: false),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    SizeID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricelistdetail", x => new { x.PriceListID, x.MenuID, x.SizeID });
                    table.ForeignKey(
                        name: "FK_pricelistdetail_menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pricelistdetail_pricelist_PriceListID",
                        column: x => x.PriceListID,
                        principalTable: "pricelist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pricelistdetail_size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ordersdetail",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MenuID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PriceListID = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CancelledBy = table.Column<int>(type: "int", nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Note = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KindOfHotpot1ID = table.Column<int>(type: "int", nullable: true),
                    KindOfHotpot2ID = table.Column<int>(type: "int", nullable: true),
                    SizeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordersdetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ordersdetail_kindofhotpot_KindOfHotpot1ID",
                        column: x => x.KindOfHotpot1ID,
                        principalTable: "kindofhotpot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ordersdetail_kindofhotpot_KindOfHotpot2ID",
                        column: x => x.KindOfHotpot2ID,
                        principalTable: "kindofhotpot",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ordersdetail_menu_MenuID",
                        column: x => x.MenuID,
                        principalTable: "menu",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ordersdetail_orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ordersdetail_pricelist_PriceListID",
                        column: x => x.PriceListID,
                        principalTable: "pricelist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ordersdetail_size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "size",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "warehouserollback",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    WarehouseID = table.Column<int>(type: "int", nullable: false),
                    QuantitySubtract = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouserollback", x => x.ID);
                    table.ForeignKey(
                        name: "FK_warehouserollback_orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_warehouserollback_warehouse_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "warehouse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roledetail",
                columns: table => new
                {
                    PermissionDetailID = table.Column<int>(type: "int", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roledetail", x => new { x.PermissionDetailID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_roledetail_permissiondetail_PermissionDetailID",
                        column: x => x.PermissionDetailID,
                        principalTable: "permissiondetail",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_roledetail_role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userandpermissiondetail",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PermissionDetailID = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userandpermissiondetail", x => new { x.UserID, x.PermissionDetailID });
                    table.ForeignKey(
                        name: "FK_userandpermissiondetail_permissiondetail_PermissionDetailID",
                        column: x => x.PermissionDetailID,
                        principalTable: "permissiondetail",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userandpermissiondetail_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_definition_DefinitionGroupID",
                table: "definition",
                column: "DefinitionGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_dinnertable_StatusOfDinnerTableID",
                table: "dinnertable",
                column: "StatusOfDinnerTableID");

            migrationBuilder.CreateIndex(
                name: "IX_employees_GenderID",
                table: "employees",
                column: "GenderID");

            migrationBuilder.CreateIndex(
                name: "IX_employees_PositionID",
                table: "employees",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_employees_StatusID",
                table: "employees",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_materials_MaterialsGroupID",
                table: "materials",
                column: "MaterialsGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_materials_StatusID",
                table: "materials",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_materials_UnitID",
                table: "materials",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_materials_UnitID_Max",
                table: "materials",
                column: "UnitID_Max");

            migrationBuilder.CreateIndex(
                name: "IX_menu_MenuGroupID",
                table: "menu",
                column: "MenuGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_menu_StatusID",
                table: "menu",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_menu_UnitID",
                table: "menu",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_menu_size_SizeID",
                table: "menu_size",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_menuandmaterials_MaterialsID",
                table: "menuandmaterials",
                column: "MaterialsID");

            migrationBuilder.CreateIndex(
                name: "IX_menuandmaterials_MenuID",
                table: "menuandmaterials",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_menuandmaterials_SizeID",
                table: "menuandmaterials",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_menugroup_StatusID",
                table: "menugroup",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_orders_DinnerTableID",
                table: "orders",
                column: "DinnerTableID");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PromotionID",
                table: "orders",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_orders_StatusOfOrderID",
                table: "orders",
                column: "StatusOfOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ordersdetail_KindOfHotpot1ID",
                table: "ordersdetail",
                column: "KindOfHotpot1ID");

            migrationBuilder.CreateIndex(
                name: "IX_ordersdetail_KindOfHotpot2ID",
                table: "ordersdetail",
                column: "KindOfHotpot2ID");

            migrationBuilder.CreateIndex(
                name: "IX_ordersdetail_MenuID",
                table: "ordersdetail",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_ordersdetail_OrderID",
                table: "ordersdetail",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ordersdetail_PriceListID",
                table: "ordersdetail",
                column: "PriceListID");

            migrationBuilder.CreateIndex(
                name: "IX_ordersdetail_SizeID",
                table: "ordersdetail",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_permission_PermissionGroupID",
                table: "permission",
                column: "PermissionGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_permissiondetail_PermissionID",
                table: "permissiondetail",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_permissiongroup_PermissionGroupTypeID",
                table: "permissiongroup",
                column: "PermissionGroupTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_pricelist_CurrencyID",
                table: "pricelist",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_pricelist_StatusOfPriceListID",
                table: "pricelist",
                column: "StatusOfPriceListID");

            migrationBuilder.CreateIndex(
                name: "IX_pricelistdetail_MenuID",
                table: "pricelistdetail",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_pricelistdetail_SizeID",
                table: "pricelistdetail",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_promotionontotalbill_StatusID",
                table: "promotionontotalbill",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_refreshtokens_UserID",
                table: "refreshtokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_role_StatusID",
                table: "role",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_roledetail_RoleID",
                table: "roledetail",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_userandpermissiondetail_PermissionDetailID",
                table: "userandpermissiondetail",
                column: "PermissionDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_userandrole_RoleID",
                table: "userandrole",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_users_EmployeesID",
                table: "users",
                column: "EmployeesID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_StatusID",
                table: "users",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_warehouse_MaterialsID",
                table: "warehouse",
                column: "MaterialsID");

            migrationBuilder.CreateIndex(
                name: "IX_warehouse_StatusID",
                table: "warehouse",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_warehouserollback_OrderID",
                table: "warehouserollback",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_warehouserollback_WarehouseID",
                table: "warehouserollback",
                column: "WarehouseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "definition");

            migrationBuilder.DropTable(
                name: "menu_size");

            migrationBuilder.DropTable(
                name: "menuandmaterials");

            migrationBuilder.DropTable(
                name: "ordersdetail");

            migrationBuilder.DropTable(
                name: "pricelistdetail");

            migrationBuilder.DropTable(
                name: "refreshtokens");

            migrationBuilder.DropTable(
                name: "roledetail");

            migrationBuilder.DropTable(
                name: "userandpermissiondetail");

            migrationBuilder.DropTable(
                name: "userandrole");

            migrationBuilder.DropTable(
                name: "warehouserollback");

            migrationBuilder.DropTable(
                name: "definitiongroup");

            migrationBuilder.DropTable(
                name: "kindofhotpot");

            migrationBuilder.DropTable(
                name: "menu");

            migrationBuilder.DropTable(
                name: "pricelist");

            migrationBuilder.DropTable(
                name: "size");

            migrationBuilder.DropTable(
                name: "permissiondetail");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "warehouse");

            migrationBuilder.DropTable(
                name: "menugroup");

            migrationBuilder.DropTable(
                name: "currency");

            migrationBuilder.DropTable(
                name: "statusofpricelist");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "dinnertable");

            migrationBuilder.DropTable(
                name: "promotionontotalbill");

            migrationBuilder.DropTable(
                name: "statusoforder");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "permissiongroup");

            migrationBuilder.DropTable(
                name: "gender");

            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "statusofdinnertable");

            migrationBuilder.DropTable(
                name: "statusofpromotion");

            migrationBuilder.DropTable(
                name: "materialsgroup");

            migrationBuilder.DropTable(
                name: "materialsstatus");

            migrationBuilder.DropTable(
                name: "unit");

            migrationBuilder.DropTable(
                name: "permissiongrouptype");
        }
    }
}
