using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class InfoAttribute : TblAttributes
    {
        public string ChildCode { get; set; }
        public new string[] DetailRefer { get; set; }
        public List<TblCategory> ListCategory { get; set; }
        public TblGeneratingValues GeneratingValues;
        public DependentValue DependentValues;
        public InfoAttribute()
        {
            ListCategory = new List<TblCategory>();
        }
    }
}
