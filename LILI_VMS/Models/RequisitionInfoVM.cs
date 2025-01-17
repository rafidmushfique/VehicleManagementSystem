using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public class RequisitionInfoVM
    {
        public RequisitionInfoVM()
        {
            PartsDetails = new List<TblMaintenanceRequisitionPartsDetail>();
        }
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string RequisitionNo { get; set; }
        public string SuppWorkShopName { get; set; }
        public string MaintenanceTypeName { get; set; }
        public string Address { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime CurrentMaintenanceDate { get; set; }
        public decimal LastMaintenanceKM { get; set; }
        public decimal CurrentMaintenanceKM { get; set; }
        public string RegistrationNo { get; set; }
        public string DriverName { get; set; }
        public decimal TotalRunKM { get; set; }
        public string DriverMobileNo { get; set; }
        public string PreparedBy { get; set; }
        public string RecommendBy { get; set; }
        public string ApprovedBy { get; set; }
        [NotMapped]
        public string PlantName { get; set; }
        [NotMapped]
        public decimal TotalPriceSum { get; set; }
        [NotMapped]
        public string AmountInWords { get; set; }
        [NotMapped]
        public List<TblMaintenanceRequisitionPartsDetail> PartsDetails { get; set; }
    }
}
