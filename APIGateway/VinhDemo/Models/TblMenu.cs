using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblMenu
    {
        public int Id { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public string ParentCode { get; set; }
        public int? MenuIndex { get; set; }
        public string MenuIcon { get; set; }
        public string MenuBadge { get; set; }
        public string MenuState { get; set; }
        public string MenuShortLable { get; set; }
        public string MenuMainState { get; set; }
        public bool? MenuTarget { get; set; }
        public string MenuType { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsActive { get; set; }
    }
}
