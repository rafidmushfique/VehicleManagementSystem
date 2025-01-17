using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblGpbillGpdetail
    {
        public int Id { get; set; }
        public string BillNo { get; set; }
        public string Gpno { get; set; }
        public decimal LoadOfMt { get; set; }
        public string Customer { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerAddress { get; set; }

        public virtual TblGpbill BillNoNavigation { get; set; }
    }
}
