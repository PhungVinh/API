using System;
using System.Collections.Generic;

namespace Encryption.Models
{
    public partial class TblCimsattributeValue
    {
        public int AttributesValueId { get; set; }
        public int? AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public bool? IsDelete { get; set; }
        public string AttributeCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string RecordId { get; set; }
        public string Module { get; set; }
    }
}
