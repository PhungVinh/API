using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblAttributeConstraint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ControlType { get; set; }
        public string ContraintsType { get; set; }
        public string ContraintsValue { get; set; }
        public bool? IsContraintType { get; set; }
        public bool? IsContraintValue { get; set; }
        public string LinkContraints { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
