using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Repositories
{
    public interface IBaseRepository
    {
        void LoadDistributedCache(IDistributedCache distributed);
    }
}
