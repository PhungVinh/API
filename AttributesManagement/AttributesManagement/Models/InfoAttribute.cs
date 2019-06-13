using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class InfoAttribute : TblVocattributes
    {
        public string ChildCode { get; set; }
        public new string[] DetailRefer { get; set; }
        public List<TblCategory> ListCategory { get; set; }
        public InfoAttribute()
        {
            ListCategory = new List<TblCategory>();
        }
    }
}
