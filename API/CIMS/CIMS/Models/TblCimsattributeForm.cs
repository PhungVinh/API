using System;
using System.Collections.Generic;

namespace CIMS.Models
{
    public partial class TblCimsattributeForm
    {
        public int AttributeFormId { get; set; }
        public int? FormId { get; set; }
        public int? AttributeId { get; set; }
        public int? RowIndex { get; set; }
        public int? AttrOrder { get; set; }
        public int? AttributeColumn { get; set; }
        public string AttributeCode { get; set; }
        public string ChildCode { get; set; }
        public bool? IsShowLabel { get; set; }
    }
}
