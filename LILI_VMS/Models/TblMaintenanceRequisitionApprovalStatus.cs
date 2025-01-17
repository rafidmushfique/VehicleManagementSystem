using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblMaintenanceRequisitionApprovalStatus
    {
        public int Id { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime RequisitionStatusDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }
        public string UnitCode { get; set; }
        public virtual TblStatus ApprovalStatusNavigation { get; set; }
        public virtual TblMaintenanceRequisition RequisitionNoNavigation { get; set; }
    }
}
