using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class ListCategory
    {
        public string CategoryCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public object children { get; set; }
        public ListCategory()
        {
            children = new object();
        }
    }
    public class ListCategoryChild
    {
        public string CategoryCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public string CategoryName { get; set; }
    }
}
