using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblFuelLogDetail
    {
        public int Id { get; set; }
        public string FuelLogNo { get; set; }
        public string FuelStationName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string FuelTypeCode { get; set; }
        public decimal CurrentFuelQty { get; set; }
        [NotMapped]
        public string FuelTypeName { get; set; }
        public virtual TblFuelLog FuelLogNoNavigation { get; set; }
    }
}
