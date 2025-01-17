using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public class EmployeeSetupVM
    {
        public EmployeeSetupVM()
        {
            TblUserMenuAuth = new List<TblUserAuthRole>();
        }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string[] PlantCodes { get; set; }
        public IList<TblUserAuthRole> TblUserMenuAuth { get; set; }
    }
}
