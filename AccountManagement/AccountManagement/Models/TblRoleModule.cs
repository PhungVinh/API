using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblRoleModule
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string MenuCode { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsImport { get; set; }
        public bool? IsExport { get; set; }
        public bool? IsPrint { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsEnable { get; set; }
        public bool? IsPermission { get; set; }

        public TblRole Role { get; set; }
    }
}
