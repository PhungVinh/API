using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblTabBar
    {
        public int Id { get; set; }
        public string TabObject { get; set; }
        public string MenuCode { get; set; }
        public int? QuantityTab { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
