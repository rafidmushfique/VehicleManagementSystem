using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LILI_VMS.Migrations
{
    public partial class AddingRMRateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MenuMaster_MenuIdentity",
                table: "MenuMaster");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "tblEmployee",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmpId",
                table: "tblEmployee",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpGrade",
                table: "tblEmployee",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tblEmployee",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                table: "tblEmployee",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_tblEmployee_EmpId",
                table: "tblEmployee",
                column: "EmpId");

            migrationBuilder.CreateTable(
                name: "GetTblProductionProcessDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentUseQty = table.Column<decimal>(nullable: false),
                    IssuedQty = table.Column<decimal>(nullable: false),
                    MaterialCode = table.Column<string>(nullable: true),
                    MaterialName = table.Column<string>(nullable: true),
                    PreviousUsedQty = table.Column<decimal>(nullable: false),
                    ProcessLoss = table.Column<decimal>(nullable: false),
                    ProcessNo = table.Column<string>(nullable: true),
                    ReqQuantity = table.Column<decimal>(nullable: false),
                    StdConsumptionQty = table.Column<decimal>(nullable: false),
                    TotalConsumption = table.Column<decimal>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    Wastage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetTblProductionProcessDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportApprovalFormForImport",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ACISpecNo = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    Business = table.Column<string>(nullable: true),
                    ConsumptioninPreviousYear = table.Column<decimal>(nullable: false),
                    DateEffectiveRevised = table.Column<string>(nullable: true),
                    ETD = table.Column<string>(nullable: true),
                    FGOpeningStockQuantity = table.Column<string>(nullable: true),
                    FGSalesConsumptionWeek = table.Column<string>(nullable: true),
                    ForTheManufactureOfFinishedProduct = table.Column<string>(nullable: true),
                    FormNo = table.Column<string>(nullable: true),
                    HistoricalHighestRate = table.Column<string>(nullable: true),
                    HistoricalLowestRate = table.Column<string>(nullable: true),
                    InTransitOnOrderSalesConsumptionWeek = table.Column<decimal>(nullable: false),
                    InTransitOnOrderStockQuantity = table.Column<decimal>(nullable: false),
                    IndentorID = table.Column<string>(nullable: true),
                    IndentorName = table.Column<string>(nullable: true),
                    LCValue = table.Column<decimal>(nullable: false),
                    LastAwardedManufacturer = table.Column<string>(nullable: true),
                    LastPurchaseRate = table.Column<string>(nullable: true),
                    LeadTimeMonth = table.Column<decimal>(nullable: false),
                    LeadTimeSalesConsumptionWeeks = table.Column<decimal>(nullable: false),
                    MAC = table.Column<decimal>(nullable: false),
                    ManufacturerCode = table.Column<string>(nullable: true),
                    ManufacturerName = table.Column<string>(nullable: true),
                    MaterialArrival = table.Column<string>(nullable: true),
                    NameofTheMaterial = table.Column<string>(nullable: true),
                    PRNo = table.Column<string>(nullable: true),
                    PRQuantity = table.Column<decimal>(nullable: false),
                    PriceUnit = table.Column<string>(nullable: true),
                    ProposedOrderQuantityUnit = table.Column<decimal>(nullable: false),
                    ProposedOrderSalesConsumptionWeek = table.Column<decimal>(nullable: false),
                    PurchasedinCurrentYear = table.Column<decimal>(nullable: false),
                    QCSourceArovalStatus = table.Column<string>(nullable: true),
                    RMInFGSalesConsumptionWeek = table.Column<decimal>(nullable: false),
                    RMInFGStockQuantity = table.Column<decimal>(nullable: false),
                    RMPMOpeningStockQuantity = table.Column<decimal>(nullable: false),
                    RMPMSalesConsumptionWeek = table.Column<decimal>(nullable: false),
                    ReferenceRateinPreviousYear = table.Column<string>(nullable: true),
                    RequirementinCurrentYear = table.Column<decimal>(nullable: false),
                    Revision = table.Column<string>(nullable: true),
                    ShelfLifeYear = table.Column<decimal>(nullable: false),
                    ShortDue = table.Column<string>(nullable: true),
                    SourceValidationInDGDA = table.Column<string>(nullable: true),
                    StandardOrderSize = table.Column<decimal>(nullable: false),
                    StockOfMaterialAsOn = table.Column<string>(nullable: true),
                    StockwhenatFactory67Week = table.Column<decimal>(nullable: false),
                    SubTotal123QuantityUnit = table.Column<decimal>(nullable: false),
                    SubTotal123SalesConsumptionWeeks = table.Column<decimal>(nullable: false),
                    SupplierCode = table.Column<string>(nullable: true),
                    SupplierName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Total45QuantityUnit = table.Column<decimal>(nullable: false),
                    Total45SalesConsumptionWeeks = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportApprovalFormForImport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportSubmission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BidQuantity = table.Column<decimal>(nullable: false),
                    Fobrate = table.Column<decimal>(nullable: false),
                    IndentorName = table.Column<string>(nullable: true),
                    IsDGDAValid = table.Column<bool>(nullable: false),
                    ItemCode = table.Column<string>(nullable: true),
                    ManufacturerCode = table.Column<string>(nullable: true),
                    ManufacturerCountryId = table.Column<long>(nullable: true),
                    ManufacturerName = table.Column<string>(nullable: true),
                    MinOrderQuantity = table.Column<decimal>(nullable: false),
                    QuotedRate = table.Column<decimal>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    ShipmentDate = table.Column<DateTime>(nullable: false),
                    SubmissionNo = table.Column<string>(nullable: true),
                    SupplierCode = table.Column<string>(nullable: true),
                    SupplierCountryId = table.Column<long>(nullable: true),
                    SupplierName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSubmission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportSupplierSubmission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BidQuantity = table.Column<decimal>(nullable: false),
                    Fobrate = table.Column<decimal>(nullable: false),
                    IndentorName = table.Column<string>(nullable: true),
                    IsDGDAValid = table.Column<bool>(nullable: false),
                    ItemCode = table.Column<string>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    ManufacturerCode = table.Column<string>(nullable: true),
                    ManufacturerCountryId = table.Column<long>(nullable: true),
                    ManufacturerName = table.Column<string>(nullable: true),
                    MinOrderQuantity = table.Column<decimal>(nullable: false),
                    QuotedRate = table.Column<decimal>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    ShipmentDate = table.Column<DateTime>(nullable: false),
                    SubmissionNo = table.Column<string>(nullable: true),
                    SupplierCode = table.Column<string>(nullable: true),
                    SupplierCountryId = table.Column<long>(nullable: true),
                    SupplierName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSupplierSubmission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCountry",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CountryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEmpGrade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GradeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmpGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployeeEducationalDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    EmpId = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ExamId = table.Column<int>(nullable: false),
                    Result = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployeeEducationalDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEmployeeEducationalDetails_tblEmployee",
                        column: x => x.EmpId,
                        principalTable: "tblEmployee",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployeeExpert",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmpId = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ExpertiesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployeeExpert", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEmployeeExpert_tblEmployee",
                        column: x => x.EmpId,
                        principalTable: "tblEmployee",
                        principalColumn: "EmpId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblExpertArea",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ExpertArea = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblExpertArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblLineSetup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    LineCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    LineName = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLineSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblMachineSetup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Capacity = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    MachineCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    MachineName = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMachineSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblMonthlyPlanning",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Month = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    PlanningNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMonthlyPlanning", x => x.Id);
                    table.UniqueConstraint("AK_tblMonthlyPlanning_PlanningNo", x => x.PlanningNo);
                });

            migrationBuilder.CreateTable(
                name: "tblQC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProcessNo = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    QCDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    QCNo = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    QCPassQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    QCQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    QCRejectQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    RequisitionNo = table.Column<string>(unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblQC", x => x.Id);
                    table.UniqueConstraint("AK_tblQC_QCNo", x => x.QCNo);
                });

            migrationBuilder.CreateTable(
                name: "tblQCParameter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    QCParameterCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    QCParameterName = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    QCParameterStandardValue = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    TypeCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblQCParameter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblQCParameterType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    TypeName = table.Column<string>(unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblQCParameterType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRequisition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatchNo = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    BusinessCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    NumberOfBatch = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    ProductCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    RequisitionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RequisitionNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRequisition", x => x.Id);
                    table.UniqueConstraint("AK_tblRequisition_RequisitionNo", x => x.RequisitionNo);
                });

            migrationBuilder.CreateTable(
                name: "tblRMRate",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClosingCost = table.Column<decimal>(nullable: false),
                    Edate = table.Column<DateTime>(nullable: true),
                    Euser = table.Column<string>(nullable: true),
                    Idate = table.Column<DateTime>(nullable: false),
                    ItemCode = table.Column<string>(maxLength: 20, nullable: false),
                    Iuser = table.Column<string>(nullable: true),
                    Period = table.Column<string>(maxLength: 6, nullable: false),
                    plantCode = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRMRate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblShiftSetup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    PlannedDownChangeTime = table.Column<decimal>(type: "numeric(18, 2)", nullable: false, defaultValueSql: "((0))"),
                    ProductiveShiftHour = table.Column<decimal>(type: "numeric(18, 2)", nullable: false, defaultValueSql: "((0))"),
                    ShiftCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ShiftName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    StandardShiftHour = table.Column<decimal>(type: "numeric(18, 2)", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblShiftSetup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "View_BOM",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatchSize = table.Column<decimal>(nullable: false),
                    BOMText = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    BOMType = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    BusinessCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    PlantId = table.Column<long>(nullable: false),
                    ProductCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    StandardOutput = table.Column<decimal>(nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View_BOM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "View_BOMDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BOMId = table.Column<long>(nullable: true),
                    CostPerProductUnit = table.Column<decimal>(type: "decimal(24, 8)", nullable: false),
                    ItemNo = table.Column<int>(nullable: false),
                    LossQuantity = table.Column<decimal>(type: "decimal(24, 8)", nullable: false),
                    MaterialCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    PerBatchQuantityExcludingLoss = table.Column<decimal>(type: "decimal(24, 8)", nullable: false),
                    QuantityPerBatch = table.Column<decimal>(type: "decimal(24, 8)", nullable: false),
                    Tolerance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View_BOMDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "View_IssueQuantity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Issue_Quantity_DC = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    MaterialCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    RequisitionNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View_IssueQuantity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "View_Material",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    AlternativeUoM = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    AUoMConversionValue = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    BaseUnit = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    BusinessCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    CompanyId = table.Column<long>(nullable: false),
                    ConversionValue = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    Discontinue = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EditIPAddress = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    InsertIPAddress = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    IUser = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    MaterialCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    MaterialName = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    MaterialTypeId = table.Column<long>(nullable: false),
                    SKUoM = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    SubBusinessCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    SubCategoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "View_Product",
                columns: table => new
                {
                    ProductCode = table.Column<string>(unicode: false, maxLength: 4, nullable: false),
                    Active = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    BrandCode = table.Column<string>(unicode: false, maxLength: 4, nullable: false),
                    BusiSumGroupCode = table.Column<string>(unicode: false, maxLength: 3, nullable: false),
                    Business = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Carton = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    DiscountStatus = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    DistDiscount = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    EffectedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    GroupCode = table.Column<string>(unicode: false, maxLength: 4, nullable: false),
                    MRP = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    PackSize = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    PCCC = table.Column<string>(unicode: false, maxLength: 4, nullable: false),
                    PrincipalCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    ProductCategory = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    ProductName = table.Column<string>(unicode: false, maxLength: 40, nullable: false),
                    ProductName1 = table.Column<string>(unicode: false, maxLength: 40, nullable: false),
                    RatePerCarton = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    ReportGroupCode = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    Show = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    SMSCODE = table.Column<string>(unicode: false, maxLength: 5, nullable: false),
                    SMSOrder = table.Column<string>(unicode: false, maxLength: 1, nullable: false),
                    StorageType = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    SubBusinessCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    VAT = table.Column<decimal>(type: "numeric(18, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View_Product", x => x.ProductCode);
                });

            migrationBuilder.CreateTable(
                name: "tblMonthlyPlanningDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    FGCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    PlanQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false, defaultValueSql: "((0))"),
                    PlanningNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    RevisedPlanQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMonthlyPlanningDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblMonthlyPlanningDetail_tblMonthlyPlanning",
                        column: x => x.PlanningNo,
                        principalTable: "tblMonthlyPlanning",
                        principalColumn: "PlanningNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblQCDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    QCNo = table.Column<string>(unicode: false, maxLength: 15, nullable: false),
                    QCParameterActualValue = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    QCParameterCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblQCDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblQCDetails_tblQC",
                        column: x => x.QCNo,
                        principalTable: "tblQC",
                        principalColumn: "QCNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblProductionProcess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IssueNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ManufacBatchEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ManufacBatchStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ManufacLineCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ManufacMachineCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ManufacMachineHour = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    ManufacManHour = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    ManufacNoOfWorker = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    ManufacShiftBreakDownChangeTime = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    ManufacShiftCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    PackBatchEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    PackBatchStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    PackLineCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    PackMachineCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    PackMachineHour = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    PackManHour = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    PackNoOfWorker = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    PackShiftBreakDownChangeTime = table.Column<decimal>(type: "numeric(18, 2)", nullable: true),
                    PackShiftCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProcessNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ProductionQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    RequisitionNo = table.Column<string>(unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProductionProcess", x => x.Id);
                    table.UniqueConstraint("AK_tblProductionProcess_ProcessNo", x => x.ProcessNo);
                    table.ForeignKey(
                        name: "FK_tblProductionProcess_tblRequisition",
                        column: x => x.RequisitionNo,
                        principalTable: "tblRequisition",
                        principalColumn: "RequisitionNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblRequisitionDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableStock = table.Column<decimal>(type: "numeric(18, 4)", nullable: false, defaultValueSql: "((0))"),
                    MaterialCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    RequiredQty = table.Column<decimal>(type: "numeric(18, 4)", nullable: false, defaultValueSql: "((0))"),
                    RequisitionNo = table.Column<string>(unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRequisitionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRequisitionDetail_tblRequisition",
                        column: x => x.RequisitionNo,
                        principalTable: "tblRequisition",
                        principalColumn: "RequisitionNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblReturn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    Comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    EDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EUser = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    IDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IssueNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    IUser = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    RequisitionNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReturnNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReturn", x => x.Id);
                    table.UniqueConstraint("AK_tblReturn_ReturnNo", x => x.ReturnNo);
                    table.ForeignKey(
                        name: "FK_tblReturn_tblRequisition",
                        column: x => x.RequisitionNo,
                        principalTable: "tblRequisition",
                        principalColumn: "RequisitionNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblProductionProcessDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentUseQty = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    IssuedQty = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    MaterialCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    PreviousUsedQty = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    ProcessLoss = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    ProcessNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ReqQuantity = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    StdConsumptionQty = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    TotalConsumption = table.Column<decimal>(type: "numeric(18, 4)", nullable: false),
                    Wastage = table.Column<decimal>(type: "numeric(18, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProductionProcessDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblProductionProcessDetail_tblProductionProcess",
                        column: x => x.ProcessNo,
                        principalTable: "tblProductionProcess",
                        principalColumn: "ProcessNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblReturnDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IssuedQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false),
                    MaterialCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    ReturnNo = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    ReturnQty = table.Column<decimal>(type: "numeric(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReturnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblReturnDetails_tblReturn",
                        column: x => x.ReturnNo,
                        principalTable: "tblReturn",
                        principalColumn: "ReturnNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployee",
                table: "tblEmployee",
                column: "EmpId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeEducationalDetail_EmpId",
                table: "tblEmployeeEducationalDetail",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEmployeeExpert_EmpId",
                table: "tblEmployeeExpert",
                column: "EmpId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMonthlyPlanning",
                table: "tblMonthlyPlanning",
                column: "PlanningNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblMonthlyPlanningDetail_PlanningNo",
                table: "tblMonthlyPlanningDetail",
                column: "PlanningNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductionProcess",
                table: "tblProductionProcess",
                column: "ProcessNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProductionProcess_RequisitionNo",
                table: "tblProductionProcess",
                column: "RequisitionNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductionProcessDetail_ProcessNo",
                table: "tblProductionProcessDetail",
                column: "ProcessNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblQC",
                table: "tblQC",
                column: "QCNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblQCDetails_QCNo",
                table: "tblQCDetails",
                column: "QCNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblRequisition",
                table: "tblRequisition",
                column: "RequisitionNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblRequisitionDetail_RequisitionNo",
                table: "tblRequisitionDetail",
                column: "RequisitionNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblReturn_RequisitionNo",
                table: "tblReturn",
                column: "RequisitionNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblReturn",
                table: "tblReturn",
                column: "ReturnNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblReturnDetails_ReturnNo",
                table: "tblReturnDetails",
                column: "ReturnNo");

            migrationBuilder.CreateIndex(
                name: "IX_tblBOMDetail",
                table: "View_BOMDetail",
                columns: new[] { "BOMId", "MaterialCode" },
                unique: true,
                filter: "[BOMId] IS NOT NULL AND [MaterialCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetTblProductionProcessDetail");

            migrationBuilder.DropTable(
                name: "ReportApprovalFormForImport");

            migrationBuilder.DropTable(
                name: "ReportSubmission");

            migrationBuilder.DropTable(
                name: "ReportSupplierSubmission");

            migrationBuilder.DropTable(
                name: "tblCountry");

            migrationBuilder.DropTable(
                name: "tblEmpGrade");

            migrationBuilder.DropTable(
                name: "tblEmployeeEducationalDetail");

            migrationBuilder.DropTable(
                name: "tblEmployeeExpert");

            migrationBuilder.DropTable(
                name: "tblExpertArea");

            migrationBuilder.DropTable(
                name: "tblLineSetup");

            migrationBuilder.DropTable(
                name: "tblMachineSetup");

            migrationBuilder.DropTable(
                name: "tblMonthlyPlanningDetail");

            migrationBuilder.DropTable(
                name: "tblProductionProcessDetail");

            migrationBuilder.DropTable(
                name: "tblQCDetails");

            migrationBuilder.DropTable(
                name: "tblQCParameter");

            migrationBuilder.DropTable(
                name: "tblQCParameterType");

            migrationBuilder.DropTable(
                name: "tblRequisitionDetail");

            migrationBuilder.DropTable(
                name: "tblReturnDetails");

            migrationBuilder.DropTable(
                name: "tblRMRate");

            migrationBuilder.DropTable(
                name: "tblShiftSetup");

            migrationBuilder.DropTable(
                name: "View_BOM");

            migrationBuilder.DropTable(
                name: "View_BOMDetail");

            migrationBuilder.DropTable(
                name: "View_IssueQuantity");

            migrationBuilder.DropTable(
                name: "View_Material");

            migrationBuilder.DropTable(
                name: "View_Product");

            migrationBuilder.DropTable(
                name: "tblMonthlyPlanning");

            migrationBuilder.DropTable(
                name: "tblProductionProcess");

            migrationBuilder.DropTable(
                name: "tblQC");

            migrationBuilder.DropTable(
                name: "tblReturn");

            migrationBuilder.DropTable(
                name: "tblRequisition");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_tblEmployee_EmpId",
                table: "tblEmployee");

            migrationBuilder.DropIndex(
                name: "IX_tblEmployee",
                table: "tblEmployee");

            migrationBuilder.DropColumn(
                name: "EmpGrade",
                table: "tblEmployee");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tblEmployee");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "tblEmployee");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "tblEmployee",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "EmpId",
                table: "tblEmployee",
                unicode: false,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 10);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MenuMaster_MenuIdentity",
                table: "MenuMaster",
                column: "MenuIdentity");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
