using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblRoleUsers
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }

        public TblRole Role { get; set; }
        public TblUsers User { get; set; }
    }
}
