using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblAttributeEvent
    {
        public int Id { get; set; }
        public string AttributeCode { get; set; }
        public string EventCode { get; set; }
        public string EventType { get; set; }
        public string EventValue { get; set; }
        public string ConditionCode { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsEnable { get; set; }
        public string AffectedDetail { get; set; }
    }
}
