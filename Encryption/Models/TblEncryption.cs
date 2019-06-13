using System;
using System.Collections.Generic;

namespace Encryption.Models
{
    public partial class TblEncryption
    {
        public int EncryptionId { get; set; }
        public string AttributeCode { get; set; }
        public string ParentCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? EncryptionStatus { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string AttributeLabel { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsFirst { get; set; }
        public bool? IsDone { get; set; }
        public string ModuleName { get; set; }
        public bool? FinalizationStatus { get; set; }
    }
}
