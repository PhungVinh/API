using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblEmailConfigs
    {
        public Guid Id { get; set; }
        public string ServerPush { get; set; }
        public string PortPush { get; set; }
        public string ServerGet { get; set; }
        public string PortGet { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int? OrganizationId { get; set; }
        public int? MaximumPush { get; set; }
        public string Ssl { get; set; }
        public string Signature { get; set; }
    }
}
