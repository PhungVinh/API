using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblEmailDownLoadedKey
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
