﻿using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblConnectionConfig
    {
        public int Id { get; set; }
        public string ConnectionKey { get; set; }
        public string ConnectionValue { get; set; }
    }
}
