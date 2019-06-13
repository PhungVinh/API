using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class AttributesObjectDTO : AttributeObject
    {
        public int RowIndex { get; set; }
        public int AttrOrder { get; set; }
        public int AttributeColumn { get; set; }
        public string AttributeDescription { get; set; }
        public int? AttributeWidth { get; set; }
        public string CategoryParentCode { get; set; }
        public string[] DetailRefer { get; set; }
        //public bool? IsDuplicate { get; set; }
        public bool? IsReuse { get; set; }
        public bool? IsVisible { get; set; }
        public string DefaultValueWithTextBox { get; set; }
        public bool? IsShowLabel { get; set; }
        public List<TblCategory> ListCategory { get; set; }
        public AttributesObjectDTO()
        {
            ListCategory = new List<TblCategory>();
        }
    }
}
