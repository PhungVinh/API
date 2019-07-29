using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblCimsFormHistory
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ChildCode { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
