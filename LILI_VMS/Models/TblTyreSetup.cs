using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblTyreSetup
    {
        public TblTyreSetup()
        {
            TblTyreChangeLogs = new HashSet<TblTyreChangeLog>();
        }

        public int Id { get; set; }
        public string TyreSetupCode { get; set; }
        public string TyreNumber { get; set; }
        public string TyreBrand { get; set; }  
        public string TyreSize { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string UnitCode { get; set; }
        public DateTime Idate { get; set; }
        public string Iuser { get; set; }
        public DateTime? Edate { get; set; }
        public string Euser { get; set; }
        public long PlantId { get; set; }
        public virtual ICollection<TblTyreChangeLog> TblTyreChangeLogs { get; set; }
    }
}
