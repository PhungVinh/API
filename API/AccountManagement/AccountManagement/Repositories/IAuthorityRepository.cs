using AccountManagement.Common;
using AccountManagement.Models;
using AccountManagement.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Repositories
{
    public interface IAuthorityRepository
    {
        string GetStringCache(string cacheKey);
        void SetStringCache(string cacheKey, Object obj);
        void LoadDistributedCache(IDistributedCache distributedCache);
        object GetAuthorityList(string strFilter);
        object SearchMenuAndRole(string ParentCode);
        TblAuthority CreateAuthority(AuthorityRoleViewModel model);
        TblAuthority UpdateAuthority(AuthorityRoleViewModel model);
        bool DeleteAuthority(int idAuth);
        object CopyAuthority(int authorityId);
        List<TblRoleViewModel> GetModuleList(int userId);
        TblOrganization GetOrganization(string orgCode);
        bool CheckDuplicate(int id, string name, string orgCode);
        object GetListAuthorityOfOrg(string orgCode);
        #region vudt       
        string GrantAuthority(List<UserDTO> users, int authorityId);
        object GetUsersToGrantAuthority(int authorityId, int userId, string orgCode);
        object AuthorityInformation(int userId);
        string GetAuthorityNameOfUser(int userId);
        #endregion
    }
}
