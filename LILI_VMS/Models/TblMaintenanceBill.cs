using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblMaintenanceBill

    {
        public TblMaintenanceBill()
        {
            TblMaintenanceBillPartsDetails = new List<TblMaintenanceBillPartsDetail>();
            TblMaintenanceBillApprovalStatuses = new HashSet<TblMaintenanceBillApprovalStatus>();
        }
        
        public int Id { get; set; }
        public string BillNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BillDate { get; set; }
        public string RequisitionNo { get; set; }
        public string Comments { get; set; }
        public long? PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string UnitCode { get; set; }
        [NotMapped]
        public string RegistrationNo { get; set; }
        [NotMapped]
        public string MaintenanceTypeName { get; set; }

        [NotMapped]
        public int StatusOrder { get; set; }
        [NotMapped]
        public string VehicleType { get; set; }
        [NotMapped]
        public string VehicleNo{ get; set; }
        [NotMapped]
        public string Driver { get; set; }
        [NotMapped]
        public DateTime LastMaintenanceDate { get; set; }
        [NotMapped]
        public DateTime CurrentMaintenanceDate { get; set; }
        [NotMapped]
        public decimal LastMaintenanceKm { get; set; }
        [NotMapped]
        public decimal CurrentMaintenanceKm { get; set; }
        [NotMapped]
        public string SuppWorkShopCode { get; set; }
        [NotMapped]
        public string SuppWorkShopName { get; set; }
        [NotMapped]
        public string PartsCode { get; set; }
        [NotMapped]
        public string PartsName { get; set; }
        [NotMapped]
        public decimal RequitionQty { get; set; }
        [NotMapped]
        public decimal UnitPrice { get; set; }
        [NotMapped]
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public decimal ServiceCharge { get; set; }
        [NotMapped]
        public string Status { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RequisitionDate { get; set; }
        public virtual TblBusinessUnitSetupInfo UnitCodeNavigation { get; set; }
        public virtual TblMaintenanceRequisition RequisitionNoNavigation { get; set; }
        public virtual ICollection<TblMaintenanceBillApprovalStatus> TblMaintenanceBillApprovalStatuses { get; set; }
        public List<TblMaintenanceBillPartsDetail> TblMaintenanceBillPartsDetails { get; set; }

    }
}
