using Microsoft.Extensions.Caching.Distributed;
//using OrganizationManagement.EmailService;
using OrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationManagement.Repositories
{
    public interface IOrganizationRepository 
    {
        object GetOrganizationList(string strFilter);
        object GetPosition(string strFilter);
        object GetListLogUser();
        object GetOrganization();
 
        string GetStringCache(string cacheKey);
        void SetStringCache(string cacheKey, Object obj);
     
      

        // Vinh
        int AddlogUser(TblLogUser logUser);
        int UpdateLogUser(TblLogUser logUser);
        int DeleteLogUser(int id);
        object GetLogUserList(string OrganizationCode);
        object SearchLogUser(string textSeach, int currentPage, int recordPerPage);
        object SearchCategory(string textSearch, int currentPage, int recordPerPage);
        object SearchUserVinhDemo(string textSearch, int currPage, int recordperpage);        
    }
}
