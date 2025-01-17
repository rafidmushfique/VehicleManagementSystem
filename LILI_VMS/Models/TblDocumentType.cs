using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblDocumentType
    {
        public TblDocumentType()
        {
            TblVehicleDocumentDetails = new HashSet<TblVehicleDocumentDetail>();
        }
        public int Id { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentTypeName { get; set; }
        public virtual ICollection<TblVehicleDocumentDetail> TblVehicleDocumentDetails { get; set; }
    }
}
