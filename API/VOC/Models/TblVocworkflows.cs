using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblVocworkflows
    {
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int? OrganizationId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
