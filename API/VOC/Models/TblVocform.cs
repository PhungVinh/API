using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblVocform
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string FormDescription { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
