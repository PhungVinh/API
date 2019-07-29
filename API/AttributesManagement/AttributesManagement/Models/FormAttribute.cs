using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class FormAttribute
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public int RowId { get; set; }
        public string RowTitle { get; set; }
        public List<AttributeObject> children { get; set; }
        public FormAttribute()
        {
            children = new List<AttributeObject>();
        }

    }
}
