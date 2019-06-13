using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Models
{
    public class PagePaging
    {
        public int? TotalPage { get; set; }
        public int? CurrPage { get; set; }
        public int? PageSize { get; set; }
    }
}
