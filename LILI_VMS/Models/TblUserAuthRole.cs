using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public partial class TblUserAuthRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MenuName { get; set; }
        public string AuthRole { get; set; }
        public int MenuIdentity { get; set; }
        public string UserRole { get; set; }
        //public string Iuser { get; set; }
        //public string Euser { get; set; }
        //public DateTime Idate { get; set; }
        //public DateTime? Edate { get; set; }
        
    }
}
