using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblCimsform
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }
        public string MenuCode { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public string FormType { get; set; }
        public bool? IsContinute { get; set; }
        public string ChildCode { get; set; }
    }
}
