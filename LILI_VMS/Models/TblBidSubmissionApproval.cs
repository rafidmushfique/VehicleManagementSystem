using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblBidSubmissionApproval
    {
        public int Id { get; set; }
        public string ApprovalNo { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string BidScheduleNo { get; set; }
        public string VendorCode { get; set; }
        public decimal BidAmount { get; set; }
        public string Comments { get; set; }
    }
}
