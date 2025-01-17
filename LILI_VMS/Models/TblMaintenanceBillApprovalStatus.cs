using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LILI_VMS.Models
{
    public partial class TblMaintenanceBillApprovalStatus
    {
        public int Id { get; set; }
        public string BillNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RequisitionStatusDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }
        public string UnitCode { get; set; }
        public virtual TblStatus ApprovalStatusNavigation { get; set; }
        public virtual TblMaintenanceBill BillNoNavigation { get; set; }
    }
}
