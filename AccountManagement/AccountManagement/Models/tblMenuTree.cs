using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Models
{
    public class tblMenuTree
    {
        public int? menuId { get; set; }
        public string code { get; set; }
        public string parentCode { get; set; }
        public string state { get; set; }
        public string shortLabel { get; set; }
        public string mainState { get; set; }
        public bool? target { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string icon { get; set; }
        public string badge { get; set; }
        public bool? IsShowSidebar { get; set; }
        public bool? IsEncyption { get; set; }
        public List<tblMenuTree> children { get; set; }
        public tblMenuTree()
        {
            children = new List<tblMenuTree>();
        }
    }
}
