using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblVocstepAttributes
    {
        public int StepAttributesId { get; set; }
        public int? StepId { get; set; }
        public int? AttributeId { get; set; }
        public int? AttributeIndex { get; set; }
    }
}
