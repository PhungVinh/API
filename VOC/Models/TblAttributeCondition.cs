using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblAttributeCondition
    {
        public int Id { get; set; }
        public string AttributeCode { get; set; }
        public string ConditionCode { get; set; }
        public string FunctionCode { get; set; }
        public string ConditionValue { get; set; }
        public string ModuleCode { get; set; }
    }
}
