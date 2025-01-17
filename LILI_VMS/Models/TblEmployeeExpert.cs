using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblEmployeeExpert
    {
        public int? Id { get; set; }
        public string EmpId { get; set; }
        public int ExpertiesId { get; set; }

        [NotMapped]
        public string ExpertArea { get; set; }

        [NotMapped]
        public string Description { get; set; }

        public TblEmployee Emp { get; set; }
    }
}
