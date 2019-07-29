using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblVocprocessAssignee
    {
        public int Id { get; set; }
        public string VocprocessCode { get; set; }
        public string StepCode { get; set; }
        public int? UserId { get; set; }
        public int? Version { get; set; }
    }
}
