using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblAttributeAdvanced
    {
        public int AdvancedId { get; set; }
        public string AdvancedName { get; set; }
        public string AdvancedDescription { get; set; }
        public string AdvancedDataType { get; set; }
        public string AdvancedObject { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
