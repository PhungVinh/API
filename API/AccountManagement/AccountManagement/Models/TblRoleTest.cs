using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblRoleTest
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public string RoleDescription { get; set; }
        public int? OrganizationId { get; set; }
        public string RoleType { get; set; }
    }
}
