using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblTabControl
    {
        public int Id { get; set; }
        public string TabObject { get; set; }
        public string TabCode { get; set; }
        public string Title { get; set; }
        public string DisplayFormat { get; set; }
        public string ModuleCode { get; set; }
        public string DependentField { get; set; }
    }
}
