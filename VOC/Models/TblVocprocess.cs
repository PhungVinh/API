using System;
using System.Collections.Generic;

namespace VOC.Models
{
    public partial class TblVocprocess
    {
        public int ID { get; set; }
        public string VOCProcessCode { get; set; }
        public string VOCProcessName { get; set; }
        public string VOCProcessType { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public int? CurrentVersion { get; set; }
        public int? DurationDay { get; set; }
        public int? DurationHour { get; set; }
        public int? DurationMinute { get; set; }
    }
}
