using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblTyreChangeLog
    {
        public int Id { get; set; }
        public string TyreChangeCode { get; set; }
        public string TyreSetupCode { get; set; }
        public DateTime InstallationDate { get; set; }
        public string PrevVehicleCode { get; set; }
        public string VehicleCode { get; set; }
        public string Status { get; set; }
        public bool IsSpare { get; set; }
        public string Comments { get; set; }
        public decimal? StartingKm { get; set; }
        public decimal? EndKm { get; set; }
        public string ChangeType { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }
        public long PlantId { get; set; }
        [NotMapped]
        public string TyreNumber { get; set; }
        [NotMapped]
        public string TyreBrand { get; set; }
        [NotMapped]
        public string TyreSize { get; set; }
        [NotMapped]
        public string CurrentTyreCode { get; set; }
        [NotMapped]
        public string RegistrationNo { get; set; }
        [NotMapped]
        public string PrevRegistrationNo { get; set; }
        public virtual TblTyreSetup TyreSetupCodeNavigation { get; set; }
    }
}
