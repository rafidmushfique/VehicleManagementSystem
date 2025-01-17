using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblStatus
    {
        public TblStatus()
        {
            TblGpbillApprovalStatuses = new HashSet<TblGpbillApprovalStatus>();
            TblMaintenanceBillApprovalStatuses = new HashSet<TblMaintenanceBillApprovalStatus>();
            TblMaintenanceRequisitionApprovalStatuses = new HashSet<TblMaintenanceRequisitionApprovalStatus>();
        }
        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public int StatusOrder { get; set; }
        public virtual ICollection<TblGpbillApprovalStatus> TblGpbillApprovalStatuses { get; set; }
        public virtual ICollection<TblMaintenanceBillApprovalStatus> TblMaintenanceBillApprovalStatuses { get; set; }
        public virtual ICollection<TblMaintenanceRequisitionApprovalStatus> TblMaintenanceRequisitionApprovalStatuses { get; set; }
    }
}
