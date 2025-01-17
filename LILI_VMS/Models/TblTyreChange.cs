using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblTyreChange
    {
        public TblTyreChange()
        {
            TblTyreChangeLogs = new List<TblTyreChangeLog>();
        }

        public int Id { get; set; }
        public string TyreChangeCode { get; set; }
        public string VehicleCode { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }
        public string Comments { get; set; }

        public virtual List<TblTyreChangeLog> TblTyreChangeLogs { get; set; }
    }
}
