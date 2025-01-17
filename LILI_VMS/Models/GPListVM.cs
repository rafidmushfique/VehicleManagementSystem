using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public class GPListVM
    {
        public int Id { get; set; }
        public string GPNo { get; set; }
        public DateTime GPDate { get; set; }
        public string VehicleNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public decimal LoadMT { get; set; }
        public decimal TotalLoadMT { get; set; }
        [NotMapped]
        public string GpDateString { get; set; }
        //public DateTime LoadingDate { get; set; }
        //public DateTime EntranceDate { get; set;}
    }
}
