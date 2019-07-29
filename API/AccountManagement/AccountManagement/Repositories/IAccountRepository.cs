using AccountManagement.Common;
using AccountManagement.Models;
using AccountManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.Repositories
{
    public interface IAccountRepository : IBaseRepository
    {
        TblUsers GetUsersLogin(string userName, string pw);

        TblUsers GetUsersCustom(string userName, string orgCode, int userIdLogin);

        TblUsers GetUsers(string userName);

        TblUsers GetUserById(int id);

        int AddUser(UserAndOrgViewModel user, int userIdLogin);

        int EditUser(UserAndOrgViewModel user, int userIdLogin);

        int DeleteUser(int userID, string username);

        Task<object> SearchUser(int userId, string strFilter);

        int ResetPassUser(TblUsers user);

        bool LockUser(TblUsers user);

        bool ActiveUser(TblUsers user);

        TblOrganization GetOrganizationId(int userId);

        //List<TblOrganization> GetOrganizationList();

        int ChangePassword(ChangePassViewModel model, TblUsers user);

        List<TblAuthority> ListAuthorityOfUser(int userId);

        int UpdateUser(TblUsers user, bool checkUpdateImage, bool updatePass, bool checkResetSuccess, int userIdLogin);

        object GetTblMenuParent(bool switchData);

        string GetStringCache(string cacheKey);

        Task<string> GetStringAsync(string cacheKey);
        
        void SetStringCache(string cacheKey, Object obj);

        object GetTblMenuSPs(int roleId);

        object GetMenuList(int userId);

        List<TblCategory> GetAllCategory(string CategoryTypeCode, string orgCode, string CategoryCode);

        object GetDepartment(string OrganizationCode);

        string GetRolePermission(int userId, string MenuCode);

        bool CheckServicePack(TblOrganization org);

        object GetAllPermission(int userId);

        bool CheckPermission(int userIdLogin, int userIdChange);

        Task<string> ImageToBase64Async(string path);

        object GetAuthorityByUserId(int userId, int organizationId);

        bool CheckLoginAdminOrganization(string userName, string pw);

        byte[] GetImageAsync(string organizationCode, string imageFile);

        string GenerateJSONWebTokenReset(TblUsers userInfo);

        bool CheckTokenReset(string username, string codeReset);

    }
}
