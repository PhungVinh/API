using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOC.Models;
using VOC.ViewModel;

namespace VOC.Repositories
{
    public interface IVOCProcessRepository
    {
        object SearchVOCProcess(int userId, string orgCode, string isShowAll, string textSearch, string isActive, int currPage, int recordperpage);

        Task<object> CopyVOCProcess(string VOCProcessCode, int version, string orgCode);

        int AddVOCProcess(ObjectVOCProccess model, string userName);

        int EditVOCProcess(ObjectVOCProccess model, string userName);

        int DeleteVOCProcess(int idVOCProcess, string userName);

        object ConditionStepVOCProcess();

        object SearchVersionVOCProcess();

        object ConfigFormVOCProcess();

        object ConfigVOCProcess();

        object ConfigStepVOCProcess();

        object GetUserAssignee(string textSearch, string orgCode, string vocProcessCode, int version, string stepCode);

        void LoadContext(string orgCode, IDistributedCache distributedCache);

        void SetStringCache(string cacheKey, Object obj);

        void SetContextFactory(TblConnectionConfig connectionStrings);

        Task<string> GetStringAsync(string cacheKey);

        object SearchVersion(string userName, string VOCProcessCode, string IsActive, int currPage, int recordperpage, string orgCode);

        Task<object> GetListUserSearchVersion(string VOCProcessCode, string orgCode);

        int SwitchStatus(VOCProcessStepsViewModel step);

        object VesionWhenAddVOC();
    }
}
