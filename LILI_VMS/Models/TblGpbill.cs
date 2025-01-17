using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblGpbill
    {
        public TblGpbill()
        {
            CustomerName = new List<string>();
            TblGpbillApprovalStatuses = new HashSet<TblGpbillApprovalStatus>();
            TblGpbillExpenseDetails = new List<TblGpbillExpenseDetail>();
            TblGpbillGpdetails = new List<TblGpbillGpdetail>();
            TblReturnTripInfos = new List<TblReturnTripInfo> ();
        }

        public int Id { get; set; }
        public string BusinessCode { get; set; }
        public string BillNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime BillDate { get; set; }
        public string VehicleCode { get; set; }
        public string DriverName { get; set; }
        public string DriverMob { get; set; }
        public string HelperName { get; set; }
        public string HelperMob { get; set; }
        public decimal TotalLoadMt { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime LoadingDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime EntranceDate { get; set; }
        public decimal StartingKm { get; set; }
        public decimal ClosingKm { get; set; }
        public decimal RunOfKm { get; set; }
        public decimal Kpl { get; set; }
        public string Comments { get; set; }
        public long? PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UnitCode { get; set; }
        public string IsReturned { get; set; }
        [NotMapped]
        public string RegNo { get; set; }
        [NotMapped]
        public string Status { get; set; }
        [NotMapped]
        public string VehicleType { get; set; }

        [NotMapped]
        public decimal Total { get; set; }
        [NotMapped]
        public List<string> CustomerName { get; set; }
        public virtual ICollection<TblGpbillApprovalStatus> TblGpbillApprovalStatuses { get; set; }
        public virtual TblBusinessUnitSetupInfo BusinessCodeNavigation { get; set; }
        public virtual TblBusinessUnitSetupInfo UnitCodeNavigation { get; set; }
        public virtual TblVehicleSetup VehicleCodeNavigation { get; set; }
        public virtual List<TblGpbillExpenseDetail> TblGpbillExpenseDetails { get; set; }
        public virtual List<TblGpbillGpdetail> TblGpbillGpdetails { get; set; }

        public virtual List<TblReturnTripInfo> TblReturnTripInfos { get; set; }
    }
}
