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

        TblUsers GetUsersCustom(string userName, string orgCode);

        TblUsers GetUsers(string userName);

        TblUsers GetUserById(int id);

        int AddUser(UserAndOrgViewModel user);

        int EditUser(UserAndOrgViewModel user);

        int DeleteUser(int userID);

        Task<object> SearchUser(string search);

        int ResetPassUser(TblUsers user);

        bool LockUser(TblUsers user);

        bool ActiveUser(TblUsers user);

        TblOrganization GetOrganizationId(int userId);

        //List<TblOrganization> GetOrganizationList();

        int ChangePassword(ChangePassViewModel model, TblUsers user);

        List<TblAuthority> ListAuthorityOfUser(int userId);

        int UpdateUser(TblUsers user, bool checkUpdateImage);

        Task<object> GetTblMenuParent();

        string GetStringCache(string cacheKey);

        Task<string> GetStringAsync(string cacheKey);
        
        void SetStringCache(string cacheKey, Object obj);

        object GetTblMenuSPs(int roleId);

        object GetMenuList(int userId);

        object GetAllCategory(string CategoryTypeCode);

        object GetDepartment(string OrganizationCode);

        string GetRolePermission(int userId, string MenuCode);

        bool CheckServicePack(TblOrganization org);

        object GetAllPermission(int userId);

        bool CheckPermission(int userIdLogin, int userIdChange);

        Task<string> ImageToBase64Async(string path);

        object GetAuthorityByUserId(int userId, int organizationId);

        bool CheckLoginAdminOrganization(string userName, string pw);

        byte[] GetImageAsync(string organizationCode, string imageFile);

    }
}
