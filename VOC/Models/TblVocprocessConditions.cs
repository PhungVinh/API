using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblVocprocessConditions
    {
        public int Id { get; set; }
        public string VocprocessCode { get; set; }
        public string ConditionType { get; set; }
        public string StepCode { get; set; }
        public string StepField { get; set; }
        public string Field { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
        public int? OrderNum { get; set; }
        public string NextCompare { get; set; }
        public int? Version { get; set; }
    }
}
