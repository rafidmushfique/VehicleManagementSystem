using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblBidSchedule
    {
        public TblBidSchedule()
        {
            CustomerNames = new List<string>();
            TblBidProposals = new HashSet<TblBidProposal>();
            TblBidScheduleCustomers = new List<TblBidScheduleCustomer>();
            TblBidScheduleVendors = new List<TblBidScheduleVendor>();
        }

        public int Id { get; set; }
        public string BidScheduleNo { get; set; }
        public DateTime BidScheduleDate { get; set; }
        public DateTime BidScheduleStartDate { get; set; }
        public DateTime BidScheduleEndDate { get; set; }
        public string LoadPoint { get; set; }
        public string CapacityMt { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string UnitCode { get; set; }
        public bool IsSendSms { get; set; }
        public long PlantId { get; set; }
        [NotMapped]
        public string ErrorMessage { get; set; } = string.Empty; 
        public virtual ICollection<TblBidProposal> TblBidProposals { get; set; }
        public virtual List<TblBidScheduleCustomer> TblBidScheduleCustomers { get; set; }
        public virtual List<TblBidScheduleVendor> TblBidScheduleVendors { get; set; }
        [NotMapped]
        public string UnitName { get; set; }
        [NotMapped]
        public string VendorCode { get; set; }
        [NotMapped]
        public string VendorName { get; set; }
        [NotMapped]
        public string VendorAddress { get; set; }
        [NotMapped]
        public string VendorContact { get; set; }
        [NotMapped]
        public decimal BidAmount { get; set; }
  
        [NotMapped]
        public string VehicleRegNo { get; set; }
        [NotMapped]
        public string DriverName { get; set; }
        [NotMapped]
        public string DriverContact { get; set; }
        [NotMapped]
        public string BidApproverComment { get; set; }
        [NotMapped]
        public string VendorComment { get; set; }

        [NotMapped]
        public List<string> CustomerNames { get; set; }
        [NotMapped]
        public string BidCount { get; set; }
    }
}
