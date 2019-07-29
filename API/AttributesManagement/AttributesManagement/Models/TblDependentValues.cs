using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblDependentValues
    {
        public int Id { get; set; }
        public string AttributeCode { get; set; }
        public string CalculatingType { get; set; }
        public string CalculatingDetail { get; set; }
        public string AttributeInfomation { get; set; }
        public string AttributeCondition { get; set; }
        public string ConditionCode { get; set; }
        public string ConditionValue { get; set; }
        public string FunctionCode { get; set; }
        public string ModuleCode { get; set; }
    }
}
