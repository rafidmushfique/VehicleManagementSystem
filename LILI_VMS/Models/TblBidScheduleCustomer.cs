using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblBidScheduleCustomer
    {
        public int Id { get; set; }
        public string BidScheduleNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Mobile { get; set; }

        public virtual TblBidSchedule BidScheduleNoNavigation { get; set; }
    }
}
