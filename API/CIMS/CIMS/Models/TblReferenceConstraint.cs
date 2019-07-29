using System;
using System.Collections.Generic;

namespace CIMS.Models
{
    public partial class TblReferenceConstraint
    {
        public int Id { get; set; }
        public string AttributeCode { get; set; }
        public int? ConstraintId { get; set; }
        public string MenuCode { get; set; }
    }
}
