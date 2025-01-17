using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblVendorSmsmaster
    {
        public TblVendorSmsmaster()
        {
            TblVendorSmsdetails = new List<TblVendorSmsdetail>();
        }

        public int Id { get; set; }
        public string VendorSmsCode { get; set; }
        public string Iuser { get; set; }
        public DateTime Idate { get; set; }
        public string Euser { get; set; }
        public DateTime? Edate { get; set; }
        public string UnitCode { get; set; }
        public DateTime ScheduleDate { get; set; }
        public long PlantId { get; set; }
        public virtual List<TblVendorSmsdetail> TblVendorSmsdetails { get; set; }
    }
}
