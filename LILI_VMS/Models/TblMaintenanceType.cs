using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblMaintenanceType
    {
        public TblMaintenanceType()
        {
            TblMaintenanceRequisitions = new HashSet<TblMaintenanceRequisition>();
        }

        public int Id { get; set; }
        public string MaintenanceTypeCode { get; set; }
        public string MaintenanceTypeName { get; set; }

        public virtual ICollection<TblMaintenanceRequisition> TblMaintenanceRequisitions { get; set; }
    }
}
