using System;
using System.Collections.Generic;
using System.Xml;
using LILI_VMS.Models;
using LILI_VMS.Models.ManageViewModels;
using LILI_VMS.Models.ReportsViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LILI_VMS.Models
{
    public partial class dbVehicleManagementContext : DbContext
    {
      
        private readonly IConfiguration _configuration;
        public dbVehicleManagementContext(IConfiguration configuration)
           
        {
            _configuration = configuration;
        }

        #region Store Procedure VM
        public virtual DbSet<VehicleInformationVM> VehicleInformationVM { get; set; } 
        public virtual DbSet<VMMaintenanceBill> VMMaintenanceBill { get; set; }
        public virtual DbSet<VehicleWiseExpenditureReport> VehicleWiseExpenditureReport { get; set; }
        public virtual DbSet<GPListVM> GPListVM { get; set; } 
        public virtual DbSet<PlantVM> PlantVm { get; set; } 
        public virtual DbSet<GpBillIndexVM> GpBillIndexVM { get; set; }
        public virtual DbSet<BillInfoVM> BillInfoVM { get; set; } 
        public virtual DbSet<RequisitionInfoVM> RequisitionInfoVM { get; set; } 
        public virtual DbSet<VMCustomerInfo> VMCustomerInfo { get; set; } 
        public virtual DbSet<VMVendorInfo> VMVendorInfo { get; set; }
        public virtual DbSet<SpResponseVM> SpResponseVM { get; set; } 
        public virtual DbSet<VMVendorListForSMS> VMVendorListForSMS { get; set; }
        #endregion

        #region Base Project DbSet
        public virtual DbSet<TblFuelLog> TblFuelLogs { get; set; }
        public virtual DbSet<TblFuelLogDetail> TblFuelLogDetails { get; set; }
        public virtual DbSet<TblFuelType> TblFuelTypes { get; set; }
        public virtual DbSet<TblReturnTripInfo> TblReturnTripInfos { get; set; }
        public virtual DbSet<TblUserAuthRole> TblUserAuthRoles { get; set; }
        public virtual DbSet<TblAuthRoleType> TblAuthRoleTypes { get; set; }
        public virtual DbSet<TblWorkShopType> TblWorkShopTypes { get; set; }
        public virtual DbSet<TblStatus> TblStatuses {  get; set; } 
        public virtual DbSet<VehicleMovementBillReport> VehicleMovementBillReport {  get; set; }
        public virtual DbSet<TblEmployee> TblEmployee { get; set; }
        public virtual DbSet<TblEmployeeEducationalDetail> TblEmployeeEducationalDetail { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
        public virtual DbSet<TblMenu> TblMenu { get; set; }
        public virtual DbSet<TblEmpGrade> TblEmpGrade { get; set; }
        public virtual DbSet<TblEmployeeExpert> TblEmployeeExpert { get; set; }
        public virtual DbSet<TblExpertArea> TblExpertArea { get; set; }
        public virtual DbSet<TblMenu> TblMenus { get; set; }
        public virtual DbSet<TblMenuList> TblMenuLists { get; set; }

        public virtual DbSet<TblBusinessUnitSetupInfo> TblBusinessUnitSetupInfos { get; set; }
        public virtual DbSet<TblPartsSetup> TblPartsSetups { get; set; }
        public virtual DbSet<TblPartsUom> TblPartsUoms { get; set; }
        public virtual DbSet<TblSupplierWorkshopInformation> TblSupplierWorkshopInformations { get; set; }
        public virtual DbSet<TblVehicleOwner> TblVehicleOwners { get; set; }
        public virtual DbSet<TblVehicleSetup> TblVehicleSetups { get; set; }
        public virtual DbSet<TblVehicleDocumentDetail> TblVehicleDocumentDetails { get; set; }
        public virtual DbSet<TblVehicleType> TblVehicleTypes { get; set; }
        public virtual DbSet<TblDocumentType> TblDocumentTypes { get; set; }
        public virtual DbSet<TblMaintenanceRequisition> TblMaintenanceRequisitions { get; set; }
        public virtual DbSet<TblMaintenanceRequisitionPartsDetail> TblMaintenanceRequisitionPartsDetails { get; set; }
        public virtual DbSet<TblMaintenanceRequisitionApprovalStatus> TblMaintenanceRequisitionApprovalStatus { get; set; }
        public virtual DbSet<TblMaintenanceType> TblMaintenanceTypes { get; set; }
        public virtual DbSet<TblMaintenanceBill> TblMaintenanceBills { get; set; }
        public virtual DbSet<TblMaintenanceBillPartsDetail> TblMaintenanceBillPartsDetails { get; set; }
        public virtual DbSet<TblMaintenanceBillApprovalStatus> TblMaintenanceBillApprovalStatuses { get; set; }
        public virtual DbSet<TblExpenseHeadSetup> TblExpenseHeadSetups { get; set; }
        public virtual DbSet<TblGpbill> TblGpbills { get; set; }
        public virtual DbSet<TblGpbillApprovalStatus> TblGpbillApprovalStatuses { get; set; }
        public virtual DbSet<TblGpbillExpenseDetail> TblGpbillExpenseDetails { get; set; }
        public virtual DbSet<TblGpbillGpdetail> TblGpbillGpdetails { get; set; }
        public virtual DbSet<TblUserWiseUnitAndPlantCode> TblUserWiseUnitAndPlantCodes { get; set; }
        public virtual DbSet<TblVehicleSize> TblVehicleSizes { get; set; }

        public virtual DbSet<TblBidProposal> TblBidProposals { get; set; }
        public virtual DbSet<TblBidSchedule> TblBidSchedules { get; set; }
        public virtual DbSet<TblBidScheduleCustomer> TblBidScheduleCustomers { get; set; }
        public virtual DbSet<TblBidScheduleVendor> TblBidScheduleVendors { get; set; }
        public virtual DbSet<TblBidSubmissionApproval> TblBidSubmissionApprovals { get; set; }
        public virtual DbSet<TblBidWinnerVehicleInfo> TblBidWinnerVehicleInfos { get; set; }
        
        public virtual DbSet<TblTyreChangeLog> TblTyreChangeLogs { get; set; }
        public virtual DbSet<TblTyreSetup> TblTyreSetups { get; set; }
        public virtual DbSet<TblUserWiseVendorCode> TblUserWiseVendorCodes { get; set; }

        public virtual DbSet<TblVendorSmsdetail> TblVendorSmsdetails { get; set; }
        public virtual DbSet<TblVendorSmsmaster> TblVendorSmsmasters { get; set; }
        #endregion




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TblAuthRoleType>(entity =>
            {
                entity.ToTable("tblAuthRoleType");

                entity.Property(e => e.AuthTypeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AuthTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
          
            modelBuilder.Entity<TblUserWiseUnitAndPlantCode>(entity =>
            {
                entity.ToTable("tblUserWiseUnitAndPlantCode");

                entity.Property(e => e.UnitCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
                entity.Property(e => e.PlantId)
                   .HasMaxLength(10)
                   .IsUnicode(false);
            });
            modelBuilder.Entity<TblEmployee>(entity =>
            {
                entity.ToTable("tblEmployee");

                entity.HasIndex(e => e.EmpId)
                    .HasName("IX_tblEmployee")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Department)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId)
                   
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpGrade)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.InverseIdNavigation)
                    .HasForeignKey<TblEmployee>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployee_tblEmployee");
            });

            modelBuilder.Entity<TblEmployeeEducationalDetail>(entity =>
            {
                entity.ToTable("tblEmployeeEducationalDetail");

                entity.Property(e => e.Comment)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpId)
                   
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Result)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.tblEmpEducationalDetail)
                    .HasPrincipalKey(p => p.EmpId)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployeeEducationalDetails_tblEmployee");
            });

           


            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<MenuMaster>(entity =>
            {
                entity.HasKey(e => new { e.MenuIdentity, e.MenuId, e.MenuName });

                entity.Property(e => e.MenuIdentity).ValueGeneratedOnAdd();

                entity.Property(e => e.MenuId)
                    .HasColumnName("MenuID")
                    .HasMaxLength(30)
                    .IsUnicode(false);
               

                entity.Property(e => e.MenuName)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MenuOrder);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.MenuFileName)
                   
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuUrl)
                   
                    .HasColumnName("MenuURL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuId)
                   
                    .HasColumnName("Parent_MenuID")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UseYn)
                    .HasColumnName("USE_YN")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.UserRoll)
                   
                    .HasColumnName("User_Roll")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.ToTable("tblMenu");

                entity.Property(e => e.ContentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Href)
                    .HasColumnName("href")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IconClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEmpGrade>(entity =>
            {
                entity.ToTable("tblEmpGrade");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GradeName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEmployeeExpert>(entity =>
            {
                entity.ToTable("tblEmployeeExpert");

                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EmpId)
                   
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmployeeExpert)
                    .HasPrincipalKey(p => p.EmpId)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblEmployeeExpert_tblEmployee");
            });

            modelBuilder.Entity<TblExpertArea>(entity =>
            {
                entity.ToTable("tblExpertArea");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpertArea)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblMenu");

                entity.Property(e => e.ContentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Href)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("href");

                entity.Property(e => e.IconClass)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblMenuList>(entity =>
            {
                entity.ToTable("tblMenuList");

                entity.Property(e => e.ActionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                   
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModuleName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PageUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParentMenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBusinessUnitSetupInfo>(entity =>
            {
                entity.ToTable("tblBusinessUnitSetupInfo");

                entity.HasIndex(e => e.BusinessCode, "IX_tblBusinessUnitSetupInfo")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);
      
                entity.Property(e => e.BusinessCode)
                   
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.FgPlantId)
                  .HasMaxLength(50)
                  .IsUnicode(false);
                entity.Property(e => e.BusinessName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblPartsSetup>(entity =>
            {
                entity.ToTable("tblPartsSetup");

                entity.HasIndex(e => e.PartsCode, "IX_tblPartsSetup")
                    .IsUnique();

                entity.Property(e => e.Brand)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NpartsName)
                    .HasMaxLength(50)
                    .HasColumnName("NPartsName");

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Origin)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PartsCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PartsName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlantCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Uomcode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UOMCode");
            });

            modelBuilder.Entity<TblPartsUom>(entity =>
            {
                entity.ToTable("tblPartsUOM");

                entity.HasIndex(e => e.Uomcode, "IX_tblPartsUOM")
                    .IsUnique();

                entity.Property(e => e.Uomcode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UOMCode");

                entity.Property(e => e.Uomname)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("UOMName");
            });

            modelBuilder.Entity<TblSupplierWorkshopInformation>(entity =>
            {
                entity.ToTable("tblSupplierWorkshopInformation");

                entity.HasIndex(e => e.SuppWorkShopCode, "IX_tblSupplierWorkshopInformation")
                    .IsUnique();

                entity.Property(e => e.AccountName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.BankName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Bin)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Branch)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoutingNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SuppWorkShopCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SuppWorkShopName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tin)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vat)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.TblSupplierWorkshopInformations)
                    .HasPrincipalKey(p => p.WstypeCode)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblSupplierWorkshopInformation_tblWorkShopType");
            });

            modelBuilder.Entity<TblVehicleDocumentDetail>(entity =>
            {
                entity.ToTable("tblVehicleDocumentDetail");

                entity.Property(e => e.DocumentTypeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IssueDate).HasColumnType("datetime");

                entity.Property(e => e.Location)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalFileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.DocumentTypeCodeNavigation)
                    .WithMany(p => p.TblVehicleDocumentDetails)
                    .HasPrincipalKey(p => p.DocumentTypeCode)
                    .HasForeignKey(d => d.DocumentTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVehicleDocumentDetail_tblDocumentType");

                entity.HasOne(d => d.VehicleCodeNavigation)
                    .WithMany(p => p.TblVehicleDocumentDetails)
                    .HasPrincipalKey(p => p.VehicleCode)
                    .HasForeignKey(d => d.VehicleCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVehicleDocumentDetail_tblVehicleSetup");
            });

            modelBuilder.Entity<TblVehicleOwner>(entity =>
            {
                entity.ToTable("tblVehicleOwner");

                entity.Property(e => e.OwnerCode)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblVehicleSetup>(entity =>
            {
                entity.ToTable("tblVehicleSetup");

                entity.HasIndex(e => e.VehicleCode, "IX_tblVehicleSetup")
                    .IsUnique();

                entity.Property(e => e.Brand)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UnitCode)
                  .IsRequired()
                  .HasMaxLength(10)
                  .IsUnicode(false);

                entity.Property(e => e.SizeOfVehicle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                   
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CapacityMt)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("CapacityMT");

                entity.Property(e => e.Cc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CC");

                entity.Property(e => e.Colour)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Depriciation)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DriverDailyAllowance).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.DriverMobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DriverJoiningDate)
                        .HasColumnType("datetime")
                        .HasColumnName("DriverJoiningDate");

                entity.Property(e => e.DriverName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                  .HasMaxLength(10)
                  .IsUnicode(false);

                entity.Property(e => e.DriverStaffId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.FuelTankCapacity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Height)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HelperDailyAllowance).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.HelperMobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HelperJoiningDate)
                        .HasColumnType("datetime")
                        .HasColumnName("HelperJoiningDate");

                entity.Property(e => e.HelperName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HelperStaffId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.InclusionDate).HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.Kpl)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("KPL");

                entity.Property(e => e.Length)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModelNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PresentMeterKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("PresentMeterKM");

                entity.Property(e => e.PurchasePrice)
                    .HasColumnType("numeric(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationNo)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeOfVehicle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TyreSize)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleCode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Vendor)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VendorMobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Weight)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Width)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessCodeNavigation)
                    .WithMany(p => p.TblVehicleSetups)
                    .HasPrincipalKey(p => p.BusinessCode)
                    .HasForeignKey(d => d.BusinessCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVehicleSetup_tblBusinessUnitSetupInfo");
            });

            modelBuilder.Entity<TblVehicleType>(entity =>
            {
                entity.ToTable("tblVehicleType");

                entity.Property(e => e.VehicleTypeCode)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleTypeName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDocumentType>(entity =>
            {
                entity.ToTable("tblDocumentType");

                entity.HasIndex(e => e.DocumentTypeCode, "IX_tblDocumentType")
                    .IsUnique();

                entity.Property(e => e.DocumentTypeCode)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentTypeName)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblMaintenanceRequisition>(entity =>
            {
                entity.ToTable("tblMaintenanceRequisition");

                entity.HasIndex(e => e.RequisitionNo, "IX_tblMaintenanceRequisition")
                    .IsUnique();

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CurrentMaintenanceDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentMaintenanceKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("CurrentMaintenanceKM"); 
                
                entity.Property(e => e.RunOfKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("RunOfKm");

                entity.Property(e => e.Driver)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.LastMaintenanceDate).HasColumnType("datetime");

                entity.Property(e => e.LastMaintenanceKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("LastMaintenanceKM");

                entity.Property(e => e.MaintenanceTypeCode)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionDate).HasColumnType("datetime");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SuppWorkShopCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.MaintenanceTypeCodeNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitions)
                    .HasPrincipalKey(p => p.MaintenanceTypeCode)
                    .HasForeignKey(d => d.MaintenanceTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceRequisition_tblMaintenanceType");

                entity.HasOne(d => d.SuppWorkShopCodeNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitions)
                    .HasPrincipalKey(p => p.SuppWorkShopCode)
                    .HasForeignKey(d => d.SuppWorkShopCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceRequisition_tblSupplierWorkshopInformation");

                entity.HasOne(d => d.UnitCodeNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitions)
                    .HasPrincipalKey(p => p.BusinessCode)
                    .HasForeignKey(d => d.UnitCode)
                    .HasConstraintName("FK_tblMaintenanceRequisition_tblBusinessUnitSetupInfo");

                entity.HasOne(d => d.VehicleCodeNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitions)
                    .HasPrincipalKey(p => p.VehicleCode)
                    .HasForeignKey(d => d.VehicleCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceRequisition_tblVehicleSetup");
            });
            modelBuilder.Entity<TblMaintenanceRequisitionApprovalStatus>(entity =>
            {
                entity.ToTable("tblMaintenanceRequisitionApprovalStatus");

                entity.Property(e => e.ApprovalStatus)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Approver)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionStatusDate).HasColumnType("datetime");

                entity.HasOne(d => d.ApprovalStatusNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitionApprovalStatuses)
                    .HasPrincipalKey(p => p.StatusCode)
                    .HasForeignKey(d => d.ApprovalStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceRequisitionApprovalStatus_tblStatus");

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitionApprovalStatuses)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceRequisitionApprovalStatus_tblMaintenanceRequisition");
            });

            modelBuilder.Entity<TblMaintenanceRequisitionPartsDetail>(entity =>
            {
                entity.ToTable("tblMaintenanceRequisitionPartsDetail");

                entity.Property(e => e.Comments)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PartsCode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequitionQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ServiceCharge).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.UnitPrice).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.RequisitionNoNavigation)
                    .WithMany(p => p.TblMaintenanceRequisitionPartsDetails)
                    .HasPrincipalKey(p => p.RequisitionNo)
                    .HasForeignKey(d => d.RequisitionNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceRequisitionPartsDetail_tblMaintenanceRequisition");
            });

            modelBuilder.Entity<TblMaintenanceType>(entity =>
            {
                entity.ToTable("tblMaintenanceType");

                entity.HasIndex(e => e.MaintenanceTypeCode, "IX_tblMaintenanceType")
                    .IsUnique();

                entity.Property(e => e.MaintenanceTypeCode)
                   
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MaintenanceTypeName)
                   
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblExpenseHeadSetup>(entity =>
            {
                entity.ToTable("tblExpenseHeadSetup");

                entity.HasIndex(e => e.ExpenseCode, "IX_tblExpenseHeadSetup")
                    .IsUnique();

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCode)
                  .IsRequired()
                  .HasMaxLength(10)
                  .IsUnicode(false);
                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.ExpenseCode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseName)
                   
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.Rate).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Unit)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });



            modelBuilder.Entity<TblGpbill>(entity =>
            {
                entity.ToTable("tblGPBill");

                entity.HasIndex(e => e.BillNo, "IX_tblGPBill")
                    .IsUnique();

                entity.Property(e => e.BillDate).HasColumnType("datetime");

                entity.Property(e => e.IsReturned)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.BillNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("ClosingKM");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DriverMob)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DriverName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.EntranceDate).HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.HelperMob)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HelperName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.Kpl)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("KPL");

                entity.Property(e => e.LoadingDate).HasColumnType("datetime");

                entity.Property(e => e.RunOfKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("RunOfKM");

                entity.Property(e => e.StartingKm)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("StartingKM");

                entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TotalLoadMt)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("TotalLoadMT");

                entity.Property(e => e.UnitCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.BusinessCodeNavigation)
                    .WithMany(p => p.TblGpbillBusinessCodeNavigations)
                    .HasPrincipalKey(p => p.BusinessCode)
                    .HasForeignKey(d => d.BusinessCode)
                    .HasConstraintName("FK_tblGPBill_tblBusinessUnitSetupInfo");

                entity.HasOne(d => d.UnitCodeNavigation)
                    .WithMany(p => p.TblGpbillUnitCodeNavigations)
                    .HasPrincipalKey(p => p.BusinessCode)
                    .HasForeignKey(d => d.UnitCode)
                    .HasConstraintName("FK_tblGPBill_tblBusinessUnitSetupInfo1");

                entity.HasOne(d => d.VehicleCodeNavigation)
                    .WithMany(p => p.TblGpbills)
                    .HasPrincipalKey(p => p.VehicleCode)
                    .HasForeignKey(d => d.VehicleCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGPBill_tblVehicleSetup");
            });

            modelBuilder.Entity<TblGpbillApprovalStatus>(entity =>
            {
                entity.ToTable("tblGPBillApprovalStatus");

                entity.Property(e => e.ApprovalStatus)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Approver)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BillStatusDate).HasColumnType("datetime");

                entity.HasOne(d => d.ApprovalStatusNavigation)
                    .WithMany(p => p.TblGpbillApprovalStatuses)
                    .HasPrincipalKey(p => p.StatusCode)
                    .HasForeignKey(d => d.ApprovalStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGPBillApprovalStatus_tblStatus");

                entity.HasOne(d => d.BillNoNavigation)
                    .WithMany(p => p.TblGpbillApprovalStatuses)
                    .HasPrincipalKey(p => p.BillNo)
                    .HasForeignKey(d => d.BillNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGPBillApprovalStatus_tblGPBill");
            });

            modelBuilder.Entity<TblGpbillExpenseDetail>(entity =>
            {
                entity.ToTable("tblGPBillExpenseDetail");

                entity.Property(e => e.BillNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExpenseCode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Rate).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(18, 2)");

                 entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.BillNoNavigation)
                    .WithMany(p => p.TblGpbillExpenseDetails)
                    .HasPrincipalKey(p => p.BillNo)
                    .HasForeignKey(d => d.BillNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGPBillExpenseDetail_tblGPBill");
            });

            modelBuilder.Entity<TblGpbillGpdetail>(entity =>
            {
                entity.ToTable("tblGPBillGPDetail");

                entity.Property(e => e.BillNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Customer)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerAddress)
                   
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Gpno)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GPNo");

                entity.Property(e => e.LoadOfMt)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("LoadOfMT");

                entity.HasOne(d => d.BillNoNavigation)
                    .WithMany(p => p.TblGpbillGpdetails)
                    .HasPrincipalKey(p => p.BillNo)
                    .HasForeignKey(d => d.BillNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblGPBillGPDetail_tblGPBill");
            });

            modelBuilder.Entity<TblMaintenanceBill>(entity =>
            {
                entity.ToTable("tblMaintenanceBill");

                entity.HasIndex(e => e.BillNo, "IX_tblMaintenanceBill")
                    .IsUnique();

                entity.Property(e => e.BillDate).HasColumnType("datetime");

                entity.Property(e => e.BillNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.RequisitionNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.HasOne(d => d.RequisitionNoNavigation)
                   .WithMany(p => p.TblMaintenanceBills)
                   .HasPrincipalKey(p => p.RequisitionNo)
                   .HasForeignKey(d => d.RequisitionNo)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_tblMaintenanceBill_tblMaintenanceRequisition");
                entity.HasOne(d => d.UnitCodeNavigation)
                    .WithMany(p => p.TblMaintenanceBills)
                    .HasPrincipalKey(p => p.BusinessCode)
                    .HasForeignKey(d => d.UnitCode)
                    .HasConstraintName("FK_tblMaintenanceBill_tblBusinessUnitSetupInfo");
            });


            modelBuilder.Entity<TblMaintenanceBillPartsDetail>(entity =>
            {
                entity.ToTable("tblMaintenanceBillPartsDetail");

                entity.Property(e => e.BillNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PartsCode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequitionQty).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ServiceCharge).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TotalPrice).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.UnitPrice).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.BillNoNavigation)
                    .WithMany(p => p.TblMaintenanceBillPartsDetails)
                    .HasPrincipalKey(p => p.BillNo)
                    .HasForeignKey(d => d.BillNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceBillPartsDetail_tblMaintenanceBillPartsDetail");
            });

            modelBuilder.Entity<TblMaintenanceBillApprovalStatus>(entity =>
            {
                entity.ToTable("tblMaintenanceBillApprovalStatus");

                entity.Property(e => e.ApprovalStatus)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Approver)
                   
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BillNo)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RequisitionStatusDate).HasColumnType("datetime");

                entity.HasOne(d => d.ApprovalStatusNavigation)
                    .WithMany(p => p.TblMaintenanceBillApprovalStatuses)
                    .HasPrincipalKey(p => p.StatusCode)
                    .HasForeignKey(d => d.ApprovalStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceBillApprovalStatus_tblStatus");

                entity.HasOne(d => d.BillNoNavigation)
                    .WithMany(p => p.TblMaintenanceBillApprovalStatuses)
                    .HasPrincipalKey(p => p.BillNo)
                    .HasForeignKey(d => d.BillNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMaintenanceBillApprovalStatus_tblMaintenanceBill");

            });
            modelBuilder.Entity<TblWorkShopType>(entity =>
            {
                entity.ToTable("tblWorkShopType");

                entity.HasIndex(e => e.WstypeCode, "IX_tblWorkShopType")
                    .IsUnique();

                entity.Property(e => e.WstypeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WSTypeCode");

                entity.Property(e => e.WstypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WSTypeName");
            });

            modelBuilder.Entity<TblStatus>(entity =>
            {
                entity.ToTable("tblStatus");

                entity.HasIndex(e => e.StatusCode, "IX_tblStatus")
                    .IsUnique();

                entity.Property(e => e.StatusCode)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StatusName)
                   
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblVehicleSize>(entity =>
            {
                entity.ToTable("tblVehicleSize");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.VehicleSizeCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleSizeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBidProposal>(entity =>
            {
                entity.ToTable("tblBidProposal");

                entity.HasIndex(e => e.BidProposalNo, "IX_tblBidProposal")
                    .IsUnique();

                entity.Property(e => e.BidAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.BidProposalDate).HasColumnType("datetime");

                entity.Property(e => e.BidProposalNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BidScheduleNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.UnitCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.BidScheduleNoNavigation)
                    .WithMany(p => p.TblBidProposals)
                    .HasPrincipalKey(p => p.BidScheduleNo)
                    .HasForeignKey(d => d.BidScheduleNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBidProposal_tblBidSchedule");
            });

            modelBuilder.Entity<TblBidSchedule>(entity =>
            {
                entity.ToTable("tblBidSchedule");

                entity.HasIndex(e => e.BidScheduleNo, "IX_tblBidSchedule")
                    .IsUnique();

                entity.Property(e => e.BidScheduleDate).HasColumnType("datetime");

                entity.Property(e => e.BidScheduleEndDate).HasColumnType("datetime");

                entity.Property(e => e.BidScheduleNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BidScheduleStartDate).HasColumnType("datetime");

                entity.Property(e => e.CapacityMt)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CapacityMT");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IsSendSms)
                    .HasColumnName("IsSendSMS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.LoadPoint)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBidScheduleCustomer>(entity =>
            {
                entity.ToTable("tblBidScheduleCustomer");

                entity.Property(e => e.Add1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Add2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BidScheduleNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BidScheduleNoNavigation)
                    .WithMany(p => p.TblBidScheduleCustomers)
                    .HasPrincipalKey(p => p.BidScheduleNo)
                    .HasForeignKey(d => d.BidScheduleNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBidScheduleCustomer_tblBidSchedule");
            });

            modelBuilder.Entity<TblBidScheduleVendor>(entity =>
            {
                entity.ToTable("tblBidScheduleVendor");

                entity.Property(e => e.Add1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Add2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BidScheduleNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);




                entity.Property(e => e.Smsmessage)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("SMSMessage")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Smsto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SMSTo");

   
                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VendorName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.BidScheduleNoNavigation)
                    .WithMany(p => p.TblBidScheduleVendors)
                    .HasPrincipalKey(p => p.BidScheduleNo)
                    .HasForeignKey(d => d.BidScheduleNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblBidScheduleVendor_tblBidSchedule");
            });

            modelBuilder.Entity<TblBidSubmissionApproval>(entity =>
            {
                entity.ToTable("tblBidSubmissionApproval");

                entity.HasIndex(e => e.ApprovalNo, "IX_tblBidSubmissionApproval")
                    .IsUnique();

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.ApprovalNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BidAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.BidScheduleNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBidWinnerVehicleInfo>(entity =>
            {
                entity.ToTable("tblBidWinnerVehicleInfo");

                entity.HasIndex(e => e.EntryNo, "IX_tblBidWinnerVehicleInfo")
                    .IsUnique();

                entity.Property(e => e.BidScheduleNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DriverContactNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DriverName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EntryDate).HasColumnType("datetime");

                entity.Property(e => e.EntryNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleRegNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
           

            modelBuilder.Entity<TblTyreChangeLog>(entity =>
            {
                entity.ToTable("tblTyreChangeLog");

                entity.Property(e => e.ChangeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Edate).HasColumnType("datetime");

                entity.Property(e => e.EndKm).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Euser)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Idate).HasColumnType("datetime");

                entity.Property(e => e.InstallationDate).HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrevVehicleCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StartingKm).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TyreChangeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TyreSetupCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);


                entity.HasOne(d => d.TyreSetupCodeNavigation)
                    .WithMany(p => p.TblTyreChangeLogs)
                    .HasPrincipalKey(p => p.TyreSetupCode)
                    .HasForeignKey(d => d.TyreSetupCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTyreChangeLog_tblTyreSetup");
            });

            modelBuilder.Entity<TblTyreSetup>(entity =>
            {
                entity.ToTable("tblTyreSetup");

                entity.HasIndex(e => e.TyreSetupCode, "IX_tblTyreSetup")
                    .IsUnique();

                entity.Property(e => e.Comments)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UnitCode)
                  .IsRequired()
                  .HasMaxLength(10)
                  .IsUnicode(false);
                entity.Property(e => e.Status)
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.Edate).HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idate).HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TyreNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TyreSize)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TyreSetupCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TyreBrand)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblUserWiseVendorCode>(entity =>
            {
                entity.ToTable("tblUserWiseVendorCode");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VendorName)
                   .HasMaxLength(500)
                   .IsUnicode(false);
            });
            modelBuilder.Entity<TblVendorSmsdetail>(entity =>
            {
                entity.ToTable("tblVendorSMSDetails");

                entity.Property(e => e.Add1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Add2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.Property(e => e.ReplyMessage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Smsmessage)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("SMSMessage")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Smsto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SMSTo");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VendorName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VendorSmsCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.VendorSmsCodeNavigation)
                    .WithMany(p => p.TblVendorSmsdetails)
                    .HasPrincipalKey(p => p.VendorSmsCode)
                    .HasForeignKey(d => d.VendorSmsCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblVendorSMSDetails_tblVendorSMSMaster");
            });

            modelBuilder.Entity<TblVendorSmsmaster>(entity =>
            {
                entity.ToTable("tblVendorSMSMaster");

                entity.HasIndex(e => e.VendorSmsCode, "IX_tblVendorSMSMaster")
                    .IsUnique();

                entity.Property(e => e.Edate)
                    .HasColumnType("datetime")
                    .HasColumnName("EDate");      

                entity.Property(e => e.ScheduleDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ScheduleDate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUser");

                entity.Property(e => e.Idate)
                    .HasColumnType("datetime")
                    .HasColumnName("IDate");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUser");

                entity.Property(e => e.UnitCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VendorSmsCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblUserAuthRole>(entity =>
            {
                entity.ToTable("tblUserAuthRole");

                entity.Property(e => e.AuthRole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole)
                    .HasMaxLength(20)
                    .IsUnicode(false);

            });
            modelBuilder.Entity<TblReturnTripInfo>(entity =>
            {
                entity.ToTable("tblReturnTripInfo");

                entity.Property(e => e.BillNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LoadingDate).HasColumnType("datetime");

                entity.Property(e => e.LoadingPoint)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.UnloadingDate).HasColumnType("datetime");

                entity.Property(e => e.UnloadingPoint)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.BillNoNavigation)
                    .WithMany(p => p.TblReturnTripInfos)
                    .HasPrincipalKey(p => p.BillNo)
                    .HasForeignKey(d => d.BillNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblReturnTripInfo_tblGPBill");
            });

            modelBuilder.Entity<TblFuelLog>(entity =>
            {
                entity.ToTable("tblFuelLog");

                entity.HasIndex(e => e.FuelLogNo, "IX_tblFuelLog")
                    .IsUnique();

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Edate).HasColumnType("datetime");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FuelLogDate).HasColumnType("datetime");

                entity.Property(e => e.FuelLogNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Idate).HasColumnType("datetime");

                entity.Property(e => e.Iuser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.VehicleCodeNavigation)
                    .WithMany(p => p.TblFuelLogs)
                    .HasPrincipalKey(p => p.VehicleCode)
                    .HasForeignKey(d => d.VehicleCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFuelLog_tblVehicleSetup");
            });

            modelBuilder.Entity<TblFuelLogDetail>(entity =>
            {
                entity.ToTable("tblFuelLogDetail");

                entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.FuelLogNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FuelStationName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FuelTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.CurrentFuelQty).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.Rate).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.FuelLogNoNavigation)
                    .WithMany(p => p.TblFuelLogDetails)
                    .HasPrincipalKey(p => p.FuelLogNo)
                    .HasForeignKey(d => d.FuelLogNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblFuelLogDetail_tblFuelLog");
            });

            modelBuilder.Entity<TblFuelType>(entity =>
            {
                entity.ToTable("tblFuelType");

                entity.Property(e => e.FuelTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FuelTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
