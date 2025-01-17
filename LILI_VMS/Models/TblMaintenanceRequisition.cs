
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblMaintenanceRequisition
    {
        public TblMaintenanceRequisition()
        {
            TblMaintenanceBills = new HashSet<TblMaintenanceBill>();
            // TblMaintenanceRequisitionPartsDetails = new HashSet<TblMaintenanceRequisitionPartsDetail>();
            TblMaintenanceRequisitionPartsDetails = new List<TblMaintenanceRequisitionPartsDetail>();
            TblMaintenanceRequisitionApprovalStatuses = new HashSet<TblMaintenanceRequisitionApprovalStatus>();
        }

        public int Id { get; set; }
        public string RequisitionNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RequisitionDate { get; set; }
        public string? MaintenanceTypeCode { get; set; }
        public string VehicleCode { get; set; }
        public string Driver { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime CurrentMaintenanceDate { get; set; }
        public decimal LastMaintenanceKm { get; set; }
        public decimal CurrentMaintenanceKm { get; set; }
        public decimal RunOfKm { get; set; }
        public string SuppWorkShopCode { get; set; }
        public string Comments { get; set; }
        public long? PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string UnitCode { get; set; }

        [NotMapped]
        public string? RegistrationNo { get; set; }
        [NotMapped]
        public string? VehicleTypeName { get; set; }
        [NotMapped]
        public string? VehicleSize { get; set; }
        [NotMapped]
        public string? SuppWorkShopName { get; set; }

        [NotMapped]
        public string PartsCode { get; set; }
        [NotMapped] 
        public decimal RequitionQty { get; set; }
        [NotMapped]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public decimal ServiceCharge { get; set; }

        [NotMapped]
        public string? ApprovalStatus { get; set; }
        [NotMapped]
        public string? MaintenanceTypeName { get; set; }
        [NotMapped]
        public string? BillNo { get; set; }
        [NotMapped]
        public string Owner { get; set; }
        [NotMapped]
        public string Vendor { get; set; }
        public virtual TblMaintenanceType MaintenanceTypeCodeNavigation { get; set; }

        public virtual TblSupplierWorkshopInformation SuppWorkShopCodeNavigation { get; set; }
        public virtual TblBusinessUnitSetupInfo UnitCodeNavigation { get; set; }
        public virtual TblVehicleSetup VehicleCodeNavigation { get; set; }
       // public virtual ICollection<TblMaintenanceBill> TblMaintenanceBills { get; set; }
        public virtual ICollection<TblMaintenanceRequisitionApprovalStatus> TblMaintenanceRequisitionApprovalStatuses { get; set; }
        //public virtual ICollection<TblMaintenanceRequisitionPartsDetail> TblMaintenanceRequisitionPartsDetails { get; set; }
        public List<TblMaintenanceRequisitionPartsDetail> TblMaintenanceRequisitionPartsDetails { get; set; }
        public virtual ICollection<TblMaintenanceBill> TblMaintenanceBills { get; set; }


    }
}
