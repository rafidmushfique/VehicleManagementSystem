using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblVehicleDocumentDetail
    {
        public int Id { get; set; }
        public string VehicleCode { get; set; }
        public string DocumentTypeCode { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Location { get; set; }
        public string FileType { get; set; }


        [NotMapped]
        public string DocumentTypeName { get; set; }

        public virtual TblDocumentType DocumentTypeCodeNavigation { get; set; }
        public virtual TblVehicleSetup VehicleCodeNavigation { get; set; }
    }
}
