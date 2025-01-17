using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblExpenseHeadSetup
    {
        public int Id { get; set; }
        public string ExpenseCode { get; set; }
        public string ExpenseName { get; set; }
        public string Unit { get; set; }
        public decimal Rate { get; set; }
        public string Comments { get; set; }
        public string UnitCode { get; set; }
        public long? PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
    }
}
