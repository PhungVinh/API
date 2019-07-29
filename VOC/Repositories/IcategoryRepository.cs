using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOC.Models;

namespace VOC.Repositories
{
    public interface IcategoryRepository
    {
        object GetAllCategory(string TextSearch, int currPage, int recodperpage);
        void LoadContext(string orgCode, IDistributedCache distributedCache);
        void SetStringCache(string cacheKey, Object obj);
        void SetContextFactory(TblConnectionConfig connectionStrings);
    }
}
