using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblAuthority
    {
        public int AuthorityId { get; set; }
        public string AuthorityName { get; set; }
        public string AuthorityType { get; set; }
        public string AuthorityDescription { get; set; }
        public int? OrganizationId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsLock { get; set; }
    }
}
