using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblSupplierWorkshopInformation
    {
        public TblSupplierWorkshopInformation()
        {
            TblMaintenanceRequisitions = new HashSet<TblMaintenanceRequisition>();
        }
        public int Id { get; set; }
        public string Type { get; set; }
        public string SuppWorkShopCode { get; set; }
        public string SuppWorkShopName { get; set; }
        public string OwnerName { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string Comments { get; set; }
        public long? PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string Tin { get; set; }
        public string Bin { get; set; }
        public string Vat { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string RoutingNumber { get; set; }
        public virtual TblWorkShopType TypeNavigation { get; set; }
        public virtual ICollection<TblMaintenanceRequisition> TblMaintenanceRequisitions { get; set; }
    }
}
