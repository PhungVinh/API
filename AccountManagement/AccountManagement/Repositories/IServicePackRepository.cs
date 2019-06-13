using AccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Repositories
{
    public interface IServicePackRepository
    {
        int AddServicePack(TblServicePack servicePack);
        int EditServicePack(TblServicePack servicePack);
        int DeleteServicePack(int id);
        int ActiveAndLockServicePack(int id);
        object GetListServicePack(string strFilter);
        TblServicePack GetServicePackById(int id);
    }
}
