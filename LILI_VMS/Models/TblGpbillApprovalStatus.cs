using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblGpbillApprovalStatus
    {
        public int Id { get; set; }
        public string BillNo { get; set; }
        public DateTime BillStatusDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Approver { get; set; }
        public string UnitCode { get; set; }
        public virtual TblStatus ApprovalStatusNavigation { get; set; }
        public virtual TblGpbill BillNoNavigation { get; set; }
    }
}
