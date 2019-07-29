using Microsoft.Extensions.Caching.Distributed;
using OrganizationManagement.EmailService;
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
        object GetServicePack(int OrganizationId);
        int UpdateOrganization(UpdateOrg model);
        int UpdateOrganizationLogo(TblOrganization Organization);
        int AddOrganization(UpdateOrg model, string token);
        int ActiveOrganization(int OrganizationId, bool IsCheckedOrg);
        int DeleteOrganization(int OrganizationId);
        int DeleteUserOrg(TblOrganizationUser OrgUser);
        void LoadDistributedCache(IDistributedCache distributedCache, IEmailService emailService);
        string GetStringCache(string cacheKey);
        void SetStringCache(string cacheKey, Object obj);
        TblOrganization GetOrganization(string orgCode);
        void GenerateDB(string orgCode, string token);
        List<TblConnectionConfig> GetAllConnection();
        Task<byte[]> GetImageAsync(string organizationCode, string imageFile);
        void UpdateLogoInforOrg(TblOrganization organization);


        #region HaiHM add get infor and update infor Organization
        Task<object> GetInforOrganization(int userId);
        int UpdateInforOrganization(TblOrganization organization, int userId, string username);
        object GetListOrg();
        #endregion
    }
}
