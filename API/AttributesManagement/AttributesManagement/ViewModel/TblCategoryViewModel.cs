using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.ViewModel
{
    public class TblCategoryViewModel
    {
        public Int64 index { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryTypeName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
