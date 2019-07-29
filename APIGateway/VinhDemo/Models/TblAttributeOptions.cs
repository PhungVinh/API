using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblAttributeOptions
    {
        public int AttributeOptionsId { get; set; }
        public int? AttributeId { get; set; }
        public int? OptionId { get; set; }
        public string OptionValue { get; set; }
        public string OptionLabel { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
