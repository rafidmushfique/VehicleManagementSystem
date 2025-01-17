using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblPartsSetup
    {
        public int Id { get; set; }
        public string PartsCode { get; set; }
        public string PartsName { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Origin { get; set; }
        public string Uomcode { get; set; }
        public string Remarks { get; set; }
        public string PlantCode { get; set; }
        public long PlantId { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string NpartsName { get; set; }
        [NotMapped]
        public string Uomname { get; set; }
    }
}
