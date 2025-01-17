using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblReturnTripInfo
    {
        public int Id { get; set; }
        public string BillNo { get; set; }
        public DateTime LoadingDate { get; set; }
        public DateTime UnloadingDate { get; set; }
        public string LoadingPoint { get; set; }
        public string UnloadingPoint { get; set; }
        public string CustomerName { get; set; }
        public decimal Weight { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comments { get; set; }

        public virtual TblGpbill BillNoNavigation { get; set; }
    }
}
