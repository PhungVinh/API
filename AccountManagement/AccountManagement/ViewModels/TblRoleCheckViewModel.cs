using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class TblRoleCheckViewModel
    {
        public string MenuCode { get; set; }
        public bool? IsEncypt { get; set; }
        public bool? IsShowAll { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEditAll { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDeleteAll { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsImport { get; set; }
        public bool? IsExport { get; set; }
        public bool? IsPrint { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsEnable { get; set; }
        public bool? IsPermission { get; set; }
        public bool? IsFirstExtend { get; set; }
        public bool? IsSecondExtend { get; set; }
        public bool? IsThirdExtend { get; set; }
        public bool? IsFouthExtend { get; set; }
    }
}
