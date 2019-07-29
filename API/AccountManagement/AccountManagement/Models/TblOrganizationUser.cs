using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblOrganizationUser
    {
        public int OrganizationUserId { get; set; }
        public int? OrganizationId { get; set; }
        public int? UserId { get; set; }

        public TblOrganization Organization { get; set; }
        public TblUsers User { get; set; }
    }
}
