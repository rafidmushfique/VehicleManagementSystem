using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblMenu
    {
        public int Id { get; set; }
        public string ParentId { get; set; }
        public string ContentName { get; set; }
        public string IconClass { get; set; }
        public string Href { get; set; }
        public int? OrderNo { get; set; }
    }
}
