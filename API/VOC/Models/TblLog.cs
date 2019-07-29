using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblLog
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ActionCode { get; set; }
        public string ObjectId { get; set; }
        public string Value { get; set; }
        public string AttributeCode { get; set; }
        public string ModuleParentCode { get; set; }
        public int? AuthorityId { get; set; }
        public int? OrgId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
