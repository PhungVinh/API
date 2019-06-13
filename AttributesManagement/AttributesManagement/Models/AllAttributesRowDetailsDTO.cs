using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class AllAttributesRowDetailsDTO : FormAttribute
    {
        public new object children { get; set; }
        public AllAttributesRowDetailsDTO()
        {
            children = new List<AttributesObjectDTO>();
        }
    }
}
