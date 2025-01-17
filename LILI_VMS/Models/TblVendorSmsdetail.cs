using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class TblVendorSmsdetail
    {
        public int Id { get; set; }
        public string VendorSmsCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Smsto { get; set; }
        public string Smsmessage { get; set; }
        public byte Try { get; set; }
        public string Status { get; set; }
        public DateTime? SentDate { get; set; }
        public long? ReceiveId { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ReplyMessage { get; set; }

        public virtual TblVendorSmsmaster VendorSmsCodeNavigation { get; set; }
    }
}
