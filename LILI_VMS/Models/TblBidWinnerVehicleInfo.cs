using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblBidWinnerVehicleInfo
    {
        public int Id { get; set; }
        public string EntryNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string BidScheduleNo { get; set; }
        public string VendorCode { get; set; }
        public string VehicleRegNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
        public string Comments { get; set; }
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
        [NotMapped]
        public string Status { get; set; }


    }
}
