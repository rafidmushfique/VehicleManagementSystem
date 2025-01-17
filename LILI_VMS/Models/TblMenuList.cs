using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblMenuList
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string MenuName { get; set; }
        public string ParentMenuName { get; set; }
        public string PageUrl { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
