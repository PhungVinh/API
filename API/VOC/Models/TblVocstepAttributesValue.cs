using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblVocstepAttributesValue
    {
        public int StepAttributesValueId { get; set; }
        public int? StepId { get; set; }
        public int? AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }

        public TblVocsteps Step { get; set; }
    }
}
