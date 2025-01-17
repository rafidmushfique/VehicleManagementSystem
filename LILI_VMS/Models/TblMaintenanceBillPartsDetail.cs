using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblMaintenanceBillPartsDetail
    {
        public int Id { get; set; }
        public string BillNo { get; set; }
        public string PartsCode { get; set; }
        public decimal RequitionQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ServiceCharge { get; set; }
        public string Comments { get; set; }
        [NotMapped]
        public string PartsName { get; set; }
        public virtual TblMaintenanceBill BillNoNavigation { get; set; }

       
    }
}
