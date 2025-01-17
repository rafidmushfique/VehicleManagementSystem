using System;

namespace LILI_VMS.Models
{
    public class VMMaintenanceBill
    {
        public int Id { get; set; }
        public string RegistrationNo { get; set; }
        public string MaintenanceTypeName { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public decimal LastMaintenanceKM { get; set;}
        public decimal CurrentMaintenanceKM { get; set; }
        public string SuppWorkShopName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Driver { get; set; }
        
        public string PartsCode { get; set; }
        public string PartsName { get; set; }
        public string UOMName { get; set; }
        public decimal RequitionQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public string PreparedBy { get; set; }
        public string CheckedBy { get; set; }
        public string RecommendedBy { get; set; }
        public string ApprovedBy { get; set; }

    }
}
