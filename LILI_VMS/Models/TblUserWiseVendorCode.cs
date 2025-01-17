using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblUserWiseVendorCode
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
    }
}
