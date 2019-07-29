using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class AttributeObject
    {
        public int AttributeId { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeLabel { get; set; }
        public int AttributeCol { get; set; }
        public string AttributeType { get; set; }
        public string DataType { get; set; }
        public int? MaximumLength { get; set; }
        public int? MinimumLength { get; set; }
        public string LabelControlForm { get; set; }
        public bool IsCategory { get; set; }
        public bool IsRequired { get; set; }
        public bool IsTableShow { get; set; }
        public bool? IsDuplicate { get; set; }
        public string DefaultValue { get; set; }
        public List<AttributeOption> children { get; set; }
        public AttributeObject()
        {
            children = new List<AttributeOption>();
        }
    }
}
