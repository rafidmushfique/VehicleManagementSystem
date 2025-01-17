using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblWorkShopType
    {
        public TblWorkShopType()
        {
            TblSupplierWorkshopInformations = new HashSet<TblSupplierWorkshopInformation>();
        }

        public int Id { get; set; }
        public string WstypeCode { get; set; }
        public string WstypeName { get; set; }

        public virtual ICollection<TblSupplierWorkshopInformation> TblSupplierWorkshopInformations { get; set; }
    }
}
