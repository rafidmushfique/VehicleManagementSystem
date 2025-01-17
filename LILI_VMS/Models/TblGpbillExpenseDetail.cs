using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblGpbillExpenseDetail
    {
        public int Id { get; set; }
        public string BillNo { get; set; }
        public string ExpenseCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Comments { get; set; }
        [NotMapped]
        public string ExpenseName { get; set; }
        [NotMapped]
        public string Unit { get; set; }
        [NotMapped]
        public decimal Total { get; set; }
        public virtual TblGpbill BillNoNavigation { get; set; }
    }
}
