using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblEmailTemplate
    {
        public int TeamplateId { get; set; }
        public string EmailName { get; set; }
        public string EmailHeader { get; set; }
        public string AttachedFile { get; set; }
        public string EmailContents { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? EmailActive { get; set; }
    }
}
