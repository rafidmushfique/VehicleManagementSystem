using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblUserWiseUnitAndPlantCode
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UnitCode { get; set; }
        public long PlantId { get; set; }
    }
}
