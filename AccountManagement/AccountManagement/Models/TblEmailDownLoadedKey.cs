using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblEmailDownLoadedKey
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
