using AttributesManagement.Models;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Repositories
{
    public interface IBaseRepository
    {
        void LoadContext(string orgCode, IDistributedCache distributed);
    }
}
