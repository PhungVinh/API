using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblVocworkflowSteps
    {
        public int WorkflowStepsId { get; set; }
        public int? WorkflowId { get; set; }
        public int? StepId { get; set; }
        public int? StepIndex { get; set; }
    }
}
