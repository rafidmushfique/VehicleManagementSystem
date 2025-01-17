using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblBidScheduleVendor
    {
        public int Id { get; set; }
        public string BidScheduleNo { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Smsto { get; set; }
        public string Smsmessage { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Mobile { get; set; }

        [NotMapped]
        public decimal BidAmount { get; set; }
        [NotMapped]
        public string Comments { get; set; }
        public virtual TblBidSchedule BidScheduleNoNavigation { get; set; }
    }
}
