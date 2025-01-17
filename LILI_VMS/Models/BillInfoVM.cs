using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public class BillInfoVM
    {

        public int Id { get; set; }
        public string BillNo { get; set; }
        public DateTime LoadingDate { get; set; }
        public DateTime BillDate { get; set; }
        public Decimal KPL { get; set; }
        public string DriverName { get; set; }
        public string Weight { get; set; }
        public string RegNo { get; set; }
        public decimal ClosingKM { get; set; }
        public decimal RunOfKm { get; set; }
        public decimal StartingKm { get; set; }
        public decimal TotalLoadMt { get; set; }
        public string PreparedBy { get; set; }
        public string CheckedBy { get; set; }
        public string ApprovedBy { get; set; }
        
        public string VehicleSize { get; set; }
        public decimal CarryingMT { get; set; }
        public string HelperName { get; set; }
        public string BusinessName { get; set; }
        public string Comments { get; set; }
    }
}
