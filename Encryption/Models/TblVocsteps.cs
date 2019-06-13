using System;
using System.Collections.Generic;

namespace Encryption.Models
{
    public partial class TblVocsteps
    {
        public TblVocsteps()
        {
            TblVocstepAttributesValue = new HashSet<TblVocstepAttributesValue>();
        }

        public int StepId { get; set; }
        public string StepName { get; set; }
        public int? OrganizationId { get; set; }
        public int? TemplateEmailId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }

        public ICollection<TblVocstepAttributesValue> TblVocstepAttributesValue { get; set; }
    }
}
