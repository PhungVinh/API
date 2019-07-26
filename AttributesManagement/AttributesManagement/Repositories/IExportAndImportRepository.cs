using AttributesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Repositories
{
    public interface IExportAndImportRepository : IBaseRepository
    {
        string ExportCustomer(string rootFolder);
        void SetStringCache(string cacheKey, Object obj);
        void SetContextFactory(ConnectionStrings connectionStrings);
    }
}
