using System.ComponentModel.DataAnnotations.Schema;

namespace LILI_VMS.Models
{
    public class VMVendorInfo
    {
        public int Id { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Mobile { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        [NotMapped]
        public string UnitName { get; set; }
    }
}
