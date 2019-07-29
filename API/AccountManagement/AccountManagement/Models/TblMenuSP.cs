using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Models
{
    public class TblMenuSP
    {
        public string Id { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public int OrderIndex { get; set; }
        public string Icon { get; set; }
    }
}
