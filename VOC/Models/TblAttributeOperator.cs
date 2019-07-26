using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblAttributeOperator
    {
        public int Id { get; set; }
        public string AttributeCode { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorDetail { get; set; }
        public string ModuleCode { get; set; }
        public bool? IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
