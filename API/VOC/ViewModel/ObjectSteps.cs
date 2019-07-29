using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOC.Models;

namespace VOC.ViewModel
{
    public class ObjectSteps
    {
        //public TblVocprocessSteps stepParent;
        public string VocprocessCode { get; set; }
        public string StepCode { get; set; }
        public string ParentCode { get; set; }
        public string StepName { get; set; }
        public int? FormId { get; set; }
        public int? ConditionId { get; set; }
        public int? OrderNum { get; set; }
        public int? Version { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool? IsFinish { get; set; }
        public bool? InProgress { get; set; }
        public int? DurationStepDay { get; set; }
        public int? DurationStepHour { get; set; }
        public int? DurationStepMinute { get; set; }
        public bool? IsNoStep { get; set; }
        public List<TblVocprocessSteps> stepChilds;

    }
}
