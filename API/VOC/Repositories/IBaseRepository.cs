using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Repositories
{
    public interface IBaseRepository
    {
        void LoadContext(string orgCode, IDistributedCache distributed);
    }
}
