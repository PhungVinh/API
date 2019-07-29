using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblAuthorityUser
    {
        public int AuthorityUserId { get; set; }
        public int? AuthorityId { get; set; }
        public int? UserId { get; set; }

        public TblUsers User { get; set; }
    }
}
