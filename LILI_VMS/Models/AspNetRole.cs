﻿using System;
using System.Collections.Generic;

namespace LILI_VMS.Models
{
    public partial class AspNetRole
    {
        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}
