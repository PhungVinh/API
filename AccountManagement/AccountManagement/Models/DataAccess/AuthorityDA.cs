using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.Repositories;
using AccountManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AccountManagement.Models.DataAccess
{
    public class AuthorityDA : IAuthorityRepository
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        private AccountCommon accountCommon = new AccountCommon();
        private SP_Authority sp = new SP_Authority();
        private IDistributedCache _distributedCache;
        private IConfiguration _config;

        public AuthorityDA(IDistributedCache distributedCache, IConfiguration config)
        {
            _distributedCache = distributedCache;
            _config = config;
        }

        /// <summary>
        /// Function get list and search Authority with paging
        /// CreatedBy: HaiHM
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <param name="strFilter">Arr parameter</param>
        /// <returns>object filter</returns>
        public object GetAuthorityList(string strFilter)
        {
            try
            {
                List<List<dynamic>> obj = sp.SearchAuthority(strFilter);
                var response = new { data = obj[0], paging = obj[1] };

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Function get menu 
        /// CreatedBy: HaiHM
        /// CreatedDate: 27/5/2019
        /// </summary>
        /// <returns></returns>
        private object GetMenuAdmin()
        {
            int index = 0;
            try
            {
                List<MenuAndRoleViewModel> lstResult = new List<MenuAndRoleViewModel>();
                List<TblMenu> lstMenuParent = db.TblMenu.Where(m => m.ParentCode == AccountConstant.MENU_PARENT_CODE && m.IsActive == true && m.IsDelete == false).OrderBy(o => o.CreateDate).ToList();
                foreach (var item in lstMenuParent)
                {
                    List<TblMenu> lst = db.TblMenu.Where(mChild => mChild.IsActive == true && mChild.IsDelete == false && mChild.ParentCode == item.MenuCode
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_ADMIN_DEPART)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_ATTR_RELATION)
                                                        ).OrderBy(o => o.MenuIndex).ToList();
                    if (lst != null && lst.Count > 0)
                    {
                        foreach (var itemChild in lst)
                        {
                            MenuAndRoleViewModel model = new MenuAndRoleViewModel();
                            index += 1;
                            model.index += index;
                            model.MenuCode = itemChild.MenuCode;
                            model.MenuName = itemChild.MenuName;
                            model.ParentCode = itemChild.ParentCode;
                            model.AuthorityId = null;
                            model.IsEncypt = false;
                            model.IsShowAll = false;
                            model.IsShow = false;
                            model.IsAdd = false;
                            model.IsEditAll = false;
                            model.IsEdit = false;
                            model.IsDeleteAll = false;
                            model.IsDelete = false;
                            model.IsImport = false;
                            model.IsExport = false;
                            model.IsPrint = false;
                            model.IsApprove = false;
                            model.IsEnable = false;
                            model.IsPermission = false;
                            model.IsFirstExtend = false;
                            model.IsSecondExtend = false;
                            model.IsThirdExtend = false;
                            model.IsFouthExtend = false;

                            lstResult.Add(model);
                        }

                    }
                    else
                    {
                        MenuAndRoleViewModel model = new MenuAndRoleViewModel();
                        index += 1;
                        model.index += index;
                        model.MenuCode = item.MenuCode;
                        model.MenuName = item.MenuName;
                        model.ParentCode = item.MenuCode;
                        model.AuthorityId = null;
                        model.IsEncypt = false;
                        model.IsShowAll = false;
                        model.IsShow = false;
                        model.IsAdd = false;
                        model.IsEditAll = false;
                        model.IsEdit = false;
                        model.IsDeleteAll = false;
                        model.IsDelete = false;
                        model.IsImport = false;
                        model.IsExport = false;
                        model.IsPrint = false;
                        model.IsApprove = false;
                        model.IsEnable = false;
                        model.IsPermission = false;
                        model.IsFirstExtend = false;
                        model.IsSecondExtend = false;
                        model.IsThirdExtend = false;
                        model.IsFouthExtend = false;

                        lstResult.Add(model);
                    }
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Function GetMenuAdminByParentCode
        /// Haihm
        /// </summary>
        /// <param name="ParentCode"></param>
        /// <returns></returns>
        private object GetMenuAdminByParentCode(string ParentCode)
        {
            int index = 0;
            try
            {
                List<MenuAndRoleViewModel> lstResult = new List<MenuAndRoleViewModel>();
                TblMenu menuParent = db.TblMenu.Where(m => m.ParentCode.Equals(AccountConstant.MENU_PARENT_CODE)
                                                        && m.MenuCode.Equals(ParentCode)
                                                        && m.IsActive == true && m.IsDelete == false).OrderBy(o => o.MenuIndex).FirstOrDefault();

                if(menuParent != null)
                {
                    List<TblMenu> lst = db.TblMenu.Where(mChild => mChild.IsActive == true && mChild.IsDelete == false
                                                        && mChild.ParentCode.Equals(ParentCode)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_ADMIN_DEPART)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_ATTR_RELATION)).OrderBy(o => o.MenuIndex).ToList<TblMenu>();
                    if (lst.Count() > 0)
                    {
                        foreach (var itemChild in lst)
                        {
                            MenuAndRoleViewModel model = new MenuAndRoleViewModel();
                            index += 1;
                            model.index += index;
                            model.MenuCode = itemChild.MenuCode;
                            model.MenuName = itemChild.MenuName;
                            model.ParentCode = itemChild.ParentCode;
                            model.AuthorityId = null;
                            model.IsEncypt = false;
                            model.IsShowAll = false;
                            model.IsShow = false;
                            model.IsAdd = false;
                            model.IsEditAll = false;
                            model.IsEdit = false;
                            model.IsDeleteAll = false;
                            model.IsDelete = false;
                            model.IsImport = false;
                            model.IsExport = false;
                            model.IsPrint = false;
                            model.IsApprove = false;
                            model.IsEnable = false;
                            model.IsPermission = false;
                            model.IsFirstExtend = false;
                            model.IsSecondExtend = false;
                            model.IsThirdExtend = false;
                            model.IsFouthExtend = false;

                            lstResult.Add(model);
                        }

                    }
                    else
                    {
                        MenuAndRoleViewModel model = new MenuAndRoleViewModel();
                        index += 1;
                        model.index += index;
                        model.MenuCode = menuParent.MenuCode;
                        model.MenuName = menuParent.MenuName;
                        model.ParentCode = menuParent.MenuCode;
                        model.AuthorityId = null;
                        model.IsEncypt = false;
                        model.IsShowAll = false;
                        model.IsShow = false;
                        model.IsAdd = false;
                        model.IsEditAll = false;
                        model.IsEdit = false;
                        model.IsDeleteAll = false;
                        model.IsDelete = false;
                        model.IsImport = false;
                        model.IsExport = false;
                        model.IsPrint = false;
                        model.IsApprove = false;
                        model.IsEnable = false;
                        model.IsPermission = false;
                        model.IsFirstExtend = false;
                        model.IsSecondExtend = false;
                        model.IsThirdExtend = false;
                        model.IsFouthExtend = false;

                        lstResult.Add(model);
                    }
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Function get list Role(Menu)
        /// CreatedBy: HaiHM
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <returns>object</returns>
        public object SearchMenuAndRole(string ParentCode)
        {
            if(!string.IsNullOrEmpty(ParentCode))
            {
                var data = GetMenuAdminByParentCode(ParentCode);
                var response =  new { data = data };
                return response;
            }
            else
            {
                var data = GetMenuAdmin();
                var response = new { data = data };
                return response;
            }
            
        }

        /// <summary>
        /// Add Authority with role(menu)
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="model">object</param>
        /// <returns>object new</returns>
        public TblAuthority CreateAuthority(AuthorityRoleViewModel model)
        {
            TblAuthority authority = model.tblAuthority;
            TblAuthority auT = null;
            try
            {
                auT = db.TblAuthority.Where(au => au.IsDelete == false && au.AuthorityName.TrimEnd().TrimStart().ToLower() == authority.AuthorityName.TrimEnd().TrimStart().ToLower()
                && au.OrganizationId == authority.OrganizationId).FirstOrDefault();
                if (auT == null)
                {
                    using (var ts = new TransactionScope())
                    {
                        //AuthorityId
                        auT = new TblAuthority();
                        auT.AuthorityName = authority.AuthorityName.TrimStart().TrimEnd();
                        auT.AuthorityDescription = authority.AuthorityDescription.TrimStart().TrimEnd();
                        auT.OrganizationId = authority.OrganizationId;
                        auT.CreateBy = authority.CreateBy;
                        auT.CreateDate = DateTime.Now;
                        auT.UpdateBy = authority.CreateBy;
                        auT.UpdateDate = DateTime.Now;
                        auT.IsDelete = false;
                        auT.IsLock = authority.IsLock;
                        db.TblAuthority.Add(auT);
                        db.SaveChanges();

                        // Add table reference
                        if (model.tblRoles != null)
                        {
                            AddRole(model.tblRoles, auT.AuthorityId);
                        }
                        //TransactionScope complete
                        ts.Complete();
                    }
                    return auT;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// Function Add list role in authority(new created)
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="roles">list role</param>
        /// <param name="authId">id authority ID(new created)</param>
        private void AddRole(List<TblRole> roles, int authId)
        {
            if (roles.Count > 0)
            {
                // Add TblRole
                using (var ts = new TransactionScope())
                {
                    foreach (var item in roles)
                    {
                        TblRole role = new TblRole();
                        role.AuthorityId = authId;
                        role.MenuCode = item.MenuCode;
                        role.IsEncypt = item.IsEncypt;
                        role.IsShowAll = item.IsShowAll;
                        role.IsShow = item.IsShow;
                        role.IsAdd = item.IsAdd;
                        role.IsEditAll = item.IsEditAll;
                        role.IsEdit = item.IsEdit;
                        role.IsDeleteAll = item.IsDeleteAll;
                        role.IsDelete = item.IsDelete;
                        role.IsImport = item.IsImport;
                        role.IsExport = item.IsExport;
                        role.IsPrint = item.IsPrint;
                        role.IsApprove = item.IsApprove;
                        role.IsEnable = item.IsEnable;
                        role.IsPermission = item.IsPermission;
                        role.IsFirstExtend = item.IsFirstExtend;
                        role.IsSecondExtend = item.IsSecondExtend;
                        role.IsThirdExtend = item.IsThirdExtend;
                        role.IsFouthExtend = item.IsFouthExtend;
                        db.TblRole.Add(role);
                        db.SaveChanges();
                    }
                    //TransactionScope complete
                    ts.Complete();
                }
            }
        }

        /// <summary>
        /// Delete list role of authority
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// </summary>
        /// <param name="authId">id authority</param>
        private void DeleteRole(int authId)
        {
            try
            {
                List<TblRole> list = db.TblRole.Where(r => r.AuthorityId == authId).ToList<TblRole>();
                if (list.Count() > 0)
                {
                    using (var ts = new TransactionScope())
                    {
                        foreach (var item in list)
                        {
                            db.TblRole.Remove(item);
                            db.SaveChanges();
                        }
                        //TransactionScope complete
                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Function Edit Authority (update authority with role(menu))
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        public TblAuthority UpdateAuthority(AuthorityRoleViewModel model)
        {
            TblAuthority authority = model.tblAuthority;
            TblAuthority auT = null;

            try
            {
                auT = db.TblAuthority.Where(au => au.IsDelete == false && au.AuthorityId == authority.AuthorityId).FirstOrDefault();
                if (auT != null)
                {
                    // check ten co bi trung hay ko
                    if (auT.AuthorityName != authority.AuthorityName)
                    {
                        if (db.TblAuthority.Where(au => au.IsDelete == false && au.AuthorityName == authority.AuthorityName && au.OrganizationId ==
                                authority.OrganizationId).Count() > 0)
                            return null;
                    }
                    using (var ts = new TransactionScope())
                    {
                        auT.AuthorityName = authority.AuthorityName.TrimEnd().TrimStart();
                        auT.AuthorityDescription = authority.AuthorityDescription.TrimEnd().TrimStart();
                        auT.UpdateBy = authority.UpdateBy;
                        auT.UpdateDate = DateTime.Now;
                        auT.IsLock = authority.IsLock;
                        db.Entry(auT).State = EntityState.Modified;
                        db.SaveChanges();
                        DeleteRole(auT.AuthorityId);
                        if (model.tblRoles != null)
                        {
                            // Add table reference
                            AddRole(model.tblRoles, auT.AuthorityId);
                        }

                        //TransactionScope complete
                        ts.Complete();
                    }
                    return auT;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// Function delete Authority
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="idAuth">delete authority(isDelete = true)</param>
        /// <returns>true or false</returns>
        public bool DeleteAuthority(int idAuth)
        {
            try
            {
                TblAuthority auT = db.TblAuthority.Where(au => au.IsDelete == false && au.AuthorityId == idAuth).FirstOrDefault();
                if (auT != null)
                {
                    // change status here
                    auT.IsDelete = true;
                    db.Entry(auT).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Function copy Authorityby id
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// </summary>
        /// <param name="authorityId"></param>
        /// <returns>role of authority and role not of authority</returns>
        public object CopyAuthority(int authorityId)
        {
            try
            {
                List<List<dynamic>> obj = sp.CopyAuthority(authorityId);

                var response = new { dataNotSelect = obj[0], dataSelected = obj[1] };

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Function get organization by Code
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// </summary>
        /// <param name="orgCode">filter</param>
        /// <returns>null or organization same filter</returns>
        public TblOrganization GetOrganization(string orgCode)
        {
            try
            {
                TblOrganization org = db.TblOrganization.Where(o => o.IsDelete == false && o.OrganizationCode == orgCode).FirstOrDefault();
                return org;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Load distributedCache
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// </summary>
        /// <param name="distributedCache"></param>
        public void LoadDistributedCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Function get cache by key
        /// CreatedBy: HaiHm
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <param name="cacheKey">key</param>
        /// <returns>object json</returns>
        public string GetStringCache(string cacheKey)
        {
            return _distributedCache.GetString(cacheKey);
        }

        /// <summary>
        /// Function set cache by key(update or add new)
        /// CreatedBy: HaiHm
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <param name="cacheKey">key</param>
        /// <param name="obj">object</param>
        public void SetStringCache(string cacheKey, Object obj)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            _distributedCache.SetString(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(obj), options.SetSlidingExpiration(TimeSpan.FromMinutes(1)));
        }

        /// <summary>
        /// Function check duplicate name authority
        /// CreatedBy: HaiHM
        /// CreatedDate: 25/5/2019
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public bool CheckDuplicate(int id, string name, string orgCode)
        {
            bool result = false;
            try
            {
                TblOrganization org = GetOrganization(orgCode);
                if (!string.IsNullOrEmpty(id.ToString()) && !string.IsNullOrEmpty(name) && id != 0)
                {
                    TblAuthority aut = db.TblAuthority.Where(o => o.OrganizationId == org.OrganizationId && o.IsDelete == false && o.AuthorityId == id).FirstOrDefault();
                    if(aut != null)
                    {
                        if(name.Trim().ToLower() != aut.AuthorityName.Trim().ToLower())
                        {
                            if(db.TblAuthority.Where(o => o.OrganizationId == org.OrganizationId && o.IsDelete == false 
                                        && o.AuthorityName.Trim().ToLower() == name.Trim().ToLower()).Count() > 0)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                            
                    }
                    if (name != aut.AuthorityName)
                    {
                        if (db.TblAuthority.Where(au => au.IsDelete == false && au.AuthorityName == name && au.OrganizationId == id).Count() > 0)
                        {
                            return false;
                        }
                    }
                }
                TblAuthority authority = db.TblAuthority.Where(au => au.OrganizationId == org.OrganizationId && au.IsDelete == false && au.AuthorityName.TrimEnd().TrimStart().ToLower() == name.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                if(authority == null)
                {
                    return true;
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return true;
            }

            return result;
        }

        #region vudt
        /// <summary>
        /// GetModuleList method: Danh sách module phân quyền
        /// </summary>
        /// <returns>trả về danh sách module phân quyền</returns>
        public IEnumerable<TblMenu> GetModuleList()
        {
            return db.TblMenu.AsNoTracking().AsEnumerable();
        }

        /// <summary>
        /// GrantAuthority method: Phân quyền cho người dùng
        /// </summary>
        /// <param name="users"></param>
        /// <param name="authorityId"></param>
        /// <returns>message</returns>
        public string GrantAuthority(List<UserDTO> users, int authorityId)
        {
            if (users.Count() == 0)
            {
                return Messages.MS00010;
            }
            foreach (var user in users)
            {
                var result = db.TblAuthorityUser.Where(x => x.AuthorityId == authorityId && x.UserId == user.UserId).FirstOrDefault();
                if (result != null)
                {
                    // return user.UserName + " đã có trong nhóm quyền: " + db.TblAuthority.Where(x => x.AuthorityId == authorityId).FirstOrDefault().AuthorityName;
                    // revoke authority of user
                    db.TblAuthorityUser.Remove(result);
                }
                else
                {
                    // add aothirity for user
                    var model = new TblAuthorityUser() { AuthorityId = authorityId, UserId = user.UserId };
                    db.TblAuthorityUser.Add(model);
                }
            }
            db.SaveChanges();
            return Messages.MS0004;
        }

        /// <summary>
        /// CreatedBy: VuDT1
        /// ModifiedBy: HaiHM
        /// GetAuthorityNameOfUser method: Tên nhóm quyền của user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Tên nhóm quyền</returns>
        public string GetAuthorityNameOfUser(int userId)
        {
            try
            {
                int authorityId = db.TblAuthorityUser.Where(x => x.UserId == userId).FirstOrDefault().AuthorityId.Value;
                string authorityName = db.TblAuthority.Where(x => x.AuthorityId == authorityId).FirstOrDefault().AuthorityName;
                if (authorityName.Equals(AccountConstant.SuperAdmin))
                {
                    return AccountConstant.SuperAdmin;
                }
                else if (authorityName.Equals(AccountConstant.Admin))
                {
                    return AccountConstant.Admin;
                }
                return AccountConstant.Member;
            }
            catch (Exception ex)
            {
                //Default = Member
                Console.WriteLine(ex.Message);
                return AccountConstant.Member;
            }
        }

        /// <summary>
        /// AuthorityInformation method: Thông tin phân quyền của user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Danh sách phân quyền</returns>
        public object AuthorityInformation(int userId)
        {
            try
            {
                // Lấy nhóm quyền của user
                var authorityId = db.TblAuthorityUser.Where(x => x.UserId == userId).FirstOrDefault().AuthorityId;
                // Lấy module phân quyền của user
                var lstRole = db.TblRole.Where(x => x.AuthorityId == authorityId).ToList().Select(x => new TblRoleViewModel
                {
                    AuthorityId = x.AuthorityId,
                    MenuCode = x.MenuCode,
                    IsEncypt = x.IsEncypt,
                    IsShow = x.IsShow,
                    IsAdd = x.IsAdd,
                    IsEdit = x.IsEdit,
                    IsDelete = x.IsDelete,
                    IsImport = x.IsImport,
                    IsExport = x.IsExport,
                    IsPrint = x.IsPrint,
                    IsApprove = x.IsApprove,
                    IsEnable = x.IsEnable,
                    IsPermission = x.IsPermission,
                    IsFirstExtend = x.IsFirstExtend,
                    IsSecondExtend = x.IsSecondExtend,
                    IsThirdExtend = x.IsThirdExtend,
                    IsFouthExtend = x.IsFouthExtend,
                    MenuName = db.TblMenu.Where(c => c.MenuCode == x.MenuCode).FirstOrDefault().MenuName
                });
                return lstRole;
            }
            catch (Exception ex)
            {
                //return "Bạn chưa có quyền nào";
                throw ex;
            }

        }

        /// <summary>
        ///  GetUsersToGrantAuthority method: Danh sách users để thêm mới vào nhóm quyền
        /// </summary>
        /// <param name="authorityId"></param>
        /// <returns>Danh sách users</returns>
        public object GetUsersToGrantAuthority(int authorityId, int userId)
        {
            try
            {
                //var model = db.TblUsers.Where(x => !db.TblAuthorityUser.Select(c => c.UserId).Contains(x.Id)).AsNoTracking().AsEnumerable();
                //Danh sách users đã có trong nhóm quyền
                var usersGrantedAuthority = db.TblUsers.Where(x => db.TblAuthorityUser.Where(c => c.AuthorityId == authorityId).Select(c => c.UserId).Contains(x.Id)).AsNoTracking().Select(x => new UserDTO()
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    IsGrantedAuthority = true
                }).ToList();
                usersGrantedAuthority.Remove(usersGrantedAuthority.Where(x => x.UserId == userId).FirstOrDefault());
                //Danh sách users chưa có trong nhóm quyền
                var organizationId = db.TblOrganizationUser.Where(x => x.UserId == userId).FirstOrDefault().OrganizationId;
                var lstUser = db.TblUsers.Where(x => db.TblOrganizationUser.Where(c => c.OrganizationId == organizationId).Select(c => c.UserId).Contains(x.Id)).AsNoTracking().ToList();
                //var result = db.TblUsers.Where(x => !db.TblAuthorityUser.Where(c => c.AuthorityId == authorityId).Select(c => c.UserId).Contains(x.Id)).AsNoTracking().Select(x => new UserDTO()
                var result = lstUser.Where(x => !db.TblAuthorityUser.Where(c => c.AuthorityId == authorityId).Select(c => c.UserId).Contains(x.Id)).Select(x => new UserDTO()
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    IsLock = x.IsLock,
                    IsDelete = x.IsDelete,
                    IsGrantedAuthority = false
                }).ToList();
                var userNotGrantedAuthority = new List<TblUserViewModel>();
                //filter users có bị lock hay đã delete
                foreach (var user in result)
                {
                    if (user.IsLock == false && user.IsDelete == false)
                    {
                        userNotGrantedAuthority.Add(user);
                    }
                }
                List<TblUserViewModel> model = new List<TblUserViewModel>();
                model.AddRange(usersGrantedAuthority);
                model.AddRange(userNotGrantedAuthority);
                return model;
            }
            catch (Exception)
            {
                //return "Error: Không có nhóm quyền này";
                return AuthorityConstant.ExceptionGetUsersToGrantAuthority;
            }
        }
        #endregion
    }
}
