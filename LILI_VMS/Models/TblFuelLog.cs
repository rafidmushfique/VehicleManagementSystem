using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblFuelLog
    {
        public TblFuelLog()
        {
            TblFuelLogDetails = new List<TblFuelLogDetail>();
        }

        public int Id { get; set; }
        public string FuelLogNo { get; set; }
        public string VehicleCode { get; set; }
        public DateTime FuelLogDate { get; set; }
        public string Comments { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }
        public long PlantId { get; set; }
        [NotMapped]
        public string RegistrationNo { get; set; }

        public virtual TblVehicleSetup VehicleCodeNavigation { get; set; }
        public virtual List<TblFuelLogDetail> TblFuelLogDetails { get; set; }
    }
}
