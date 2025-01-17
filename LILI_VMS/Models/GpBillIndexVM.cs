using System;

namespace LILI_VMS.Models
{
    public class GpBillIndexVM
    {
        public int Id { get; set; }
        public DateTime? BillDate { get; set; }
        public string BillNo { get; set; }
        public DateTime? LoadingDate { get; set; }
        public string Comments { get; set; }
        public string VehicleCode { get; set; }
        public string RegNo { get; set; }
        public string VehicleType { get; set; }
        public decimal? TotalLoadMt { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
