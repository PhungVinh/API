using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblCimsattributeForm
    {
        public int AttributeFormId { get; set; }
        public int? FormId { get; set; }
        public int? AttributeId { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeType { get; set; }
        public int? AttrOrder { get; set; }
        public int? AttributeColumn { get; set; }
        public int? RowIndex { get; set; }
        public string RowTitle { get; set; }
        public string ChildCode { get; set; }
        public bool? IsShowLabel { get; set; }
        public string DefaultValue { get; set; }
    }
}
