using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblBusinessUnitSetupInfo
    {
        public TblBusinessUnitSetupInfo()
        {
            
            TblVehicleSetups = new HashSet<TblVehicleSetup>();
            TblGpbillBusinessCodeNavigations = new HashSet<TblGpbill>();
            TblGpbillUnitCodeNavigations = new HashSet<TblGpbill>();
            TblMaintenanceBills = new HashSet<TblMaintenanceBill>();
            TblMaintenanceRequisitions = new HashSet<TblMaintenanceRequisition>();
            TblVehicleSetups = new HashSet<TblVehicleSetup>();
        }
        public int Id { get; set; }
        public string Type { get; set; }
        public string BusinessCode { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public long PlantId { get; set; }
        public string FgPlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public virtual ICollection<TblGpbill> TblGpbillBusinessCodeNavigations { get; set; }
        public virtual ICollection<TblGpbill> TblGpbillUnitCodeNavigations { get; set; }
        public virtual ICollection<TblMaintenanceBill> TblMaintenanceBills { get; set; }
        public virtual ICollection<TblMaintenanceRequisition> TblMaintenanceRequisitions { get; set; }
        public virtual ICollection<TblVehicleSetup> TblVehicleSetups { get; set; }
    }
}
