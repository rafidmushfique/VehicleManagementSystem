using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblVehicleSetup
    {

        public TblVehicleSetup()
        {
            TblFuelLogs = new HashSet<TblFuelLog>();
            TblGpbills = new List<TblGpbill>();
            TblMaintenanceRequisitions = new HashSet<TblMaintenanceRequisition>();
            TblVehicleDocumentDetails = new List<TblVehicleDocumentDetail>();
        }
        public int Id { get; set; }
        public string BusinessCode { get; set; }
        public string VehicleCode { get; set; }
        public DateTime InclusionDate { get; set; }
        public string Owner { get; set; }
        public string Vendor { get; set; }
        public string VendorMobile { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string Depriciation { get; set; }
        public int? ManufacturingYear { get; set; }
        public string ModelNo { get; set; }
        public string Brand { get; set; }
        public string Colour { get; set; }
        public string Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string TypeOfVehicle { get; set; }
        public decimal PresentMeterKm { get; set; }
        public string TyreSize { get; set; }
        public int QtyofTyre { get; set; }
        public string Cc { get; set; }
        public decimal CapacityMt { get; set; }
        public decimal Kpl { get; set; }
        public string FuelTankCapacity { get; set; }
        public string DriverName { get; set; }
        public string DriverStaffId { get; set; }
        public string DriverMobileNo { get; set; }
        public DateTime? DriverJoiningDate { get; set; }
        public decimal DriverDailyAllowance { get; set; }
        public string HelperName { get; set; }
        public string HelperStaffId { get; set; }
        public string HelperMobileNo { get; set; }
        public DateTime? HelperJoiningDate { get; set; }
        public decimal HelperDailyAllowance { get; set; }
        public string Comments { get; set; }
        public long? PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string SizeOfVehicle { get; set; }
        public string UnitCode { get; set; }
        public string Status { get; set; }
        //public virtual ICollection<TblVehicleDocumentDetail> TblVehicleDocumentDetails { get; set; }

        public List<TblVehicleDocumentDetail> TblVehicleDocumentDetails { get; set; }

        [NotMapped]
        public string DocumentTypeCode { get; set; }

        [NotMapped]
        public DateTime IssueDate { get; set; }
        [NotMapped]
        public DateTime ExpiryDate { get; set; }
        [NotMapped]
        public string BusinessName { get; set; }
        public virtual ICollection<TblFuelLog> TblFuelLogs { get; set; }
        public virtual TblBusinessUnitSetupInfo BusinessCodeNavigation { get; set; }
        public virtual ICollection<TblGpbill> TblGpbills { get; set; }
        public virtual ICollection<TblMaintenanceRequisition> TblMaintenanceRequisitions { get; set; }


        [NotMapped]
        public DateTime? LastMaintenanceDate { get; set; }
        [NotMapped]
        public decimal LastMaintenanceKm { get; set; }
        [NotMapped]
        public IFormFile Attachment { get; set; }
    }
}
