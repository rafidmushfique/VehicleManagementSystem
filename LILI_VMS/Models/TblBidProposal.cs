using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblBidProposal
    {
        public int Id { get; set; }
        public string BidProposalNo { get; set; }
        public DateTime BidProposalDate { get; set; }
        public string BidScheduleNo { get; set; }
        public decimal BidAmount { get; set; }
        public string Comments { get; set; }
        public string VendorCode { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string UnitCode { get; set; }
        [NotMapped]
        public string BidCreatorComment { get; set; }
        [NotMapped]
        public DateTime BidScheduleDate { get; set; }
        [NotMapped]
        public DateTime BidScheduleStartDate { get; set; }
        [NotMapped]
        public DateTime BidScheduleEndDate { get; set; }
        [NotMapped]
        public string LoadPoint { get; set; }
        [NotMapped]
        public string CapacityMt { get; set; }
        public virtual TblBidSchedule BidScheduleNoNavigation { get; set; }
    }
}
