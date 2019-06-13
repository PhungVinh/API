using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class PageInfo<T>
    {
        public int totalPage { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }

        public PageInfo(IEnumerable<T> items, int currentPage = 1, int pageSize = 15)
        {
            List<T> Data = items.ToList();
            count = items.Count();
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            totalPage = (int)Math.Ceiling(count / (double)this.pageSize);
        }
    }
}
