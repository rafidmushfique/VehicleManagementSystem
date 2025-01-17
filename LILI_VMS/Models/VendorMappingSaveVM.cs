using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public class VendorMappingSaveVM
    {
        public VendorMappingSaveVM()
        {
            TblUserWiseVendorCode = new List<TblUserWiseVendorCode>();
        }

        public List<TblUserWiseVendorCode> TblUserWiseVendorCode { get; set; }
    }
}
