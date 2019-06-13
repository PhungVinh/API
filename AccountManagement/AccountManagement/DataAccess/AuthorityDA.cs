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
using AccountManagement.Models;

namespace AccountManagement.DataAccess
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
                List<TblMenu> lstMenuParent = db.TblMenu.Where(m => m.ParentCode == AccountConstant.MENU_PARENT_CODE
                                    && m.IsActive == true
                                    && m.IsDelete == false
                                    && !m.MenuCode.Equals("PROFILE")
                                    ).OrderBy(o => o.MenuIndex).ToList();
                foreach (var item in lstMenuParent)
                {
                    List<TblMenu> lst = db.TblMenu.Where(mChild => mChild.IsActive == true && mChild.IsDelete == false && mChild.ParentCode == item.MenuCode
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_ADMIN_DEPART)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_PROFILE_ORGANIZATION)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_PROFILE_CHANGEPASSWORD)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_PROFILE_USER)
                                                        ).OrderBy(o => o.MenuIndex).ToList();
                    if (lst.Count > 0)
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
                            model.IsEncypt = !itemChild.IsEncyption;
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
                        model.IsEncypt = !item.IsEncyption;
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
                return lstResult.OrderBy(m => m.MenuCode);
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
                                                        && m.IsActive == true && m.IsDelete == false
                                                        && !m.MenuCode.Equals("PROFILE")
                                                        ).OrderBy(o => o.MenuIndex).FirstOrDefault();

                if (menuParent != null)
                {
                    List<TblMenu> lst = db.TblMenu.Where(mChild => mChild.IsActive == true && mChild.IsDelete == false
                                                        && mChild.ParentCode.Equals(ParentCode)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_ADMIN_DEPART)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_PROFILE_ORGANIZATION)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_PROFILE_CHANGEPASSWORD)
                                                        && !mChild.MenuCode.Equals(AccountConstant.MENU_PROFILE_USER)
                                                        ).OrderBy(o => o.MenuIndex).ToList<TblMenu>();
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
                            model.IsEncypt = !itemChild.IsEncyption;
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
                        model.IsEncypt = !menuParent.IsEncyption;
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
                return lstResult.OrderBy(m => m.MenuCode);
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
            if (!string.IsNullOrEmpty(ParentCode))
            {
                var data = GetMenuAdminByParentCode(ParentCode);
                var response = new { data = data };
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
                        TblMenu menu = db.TblMenu.Where(m => m.MenuCode == item.MenuCode && m.IsDelete == false).FirstOrDefault();
                        if (menu != null)
                        {
                            if (!menu.IsEncyption.HasValue)
                            {
                                role.IsEncypt = null;
                            }
                            else
                            {
                                if (menu.IsEncyption.Value)
                                {
                                    role.IsEncypt = item.IsEncypt;
                                }
                                else
                                {
                                    role.IsEncypt = null;
                                }
                            }
                        }
                        else
                        {
                            role.IsEncypt = null;
                        }
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
                //obj[0] auto null - lười quá chả sửa đâu
                List<MenuAndRoleViewModel> lstfinal = new List<MenuAndRoleViewModel>();
                List<MenuAndRoleViewModel> roles = obj[1].OfType<MenuAndRoleViewModel>().ToList();
                if (roles != null)
                {
                    foreach (var item in roles)
                    {
                        if (!string.IsNullOrEmpty(item.MenuCode))
                        {
                            if (item.MenuCode.Equals(AccountConstant.MENU_PROFILE_CHANGEPASSWORD) || item.MenuCode.Equals(AccountConstant.MENU_PROFILE_USER))
                            {

                            }
                            else
                            {
                                if (item.ParentCode.Contains(AccountConstant.MENU_PARENT_CODE))
                                {
                                    item.ParentCode = item.MenuCode;
                                }
                                TblMenu menu = db.TblMenu.Where(m => m.MenuCode == item.MenuCode && m.IsDelete == false).FirstOrDefault();
                                if (menu != null)
                                {
                                    if (!menu.IsEncyption.HasValue)
                                    {
                                        item.IsEncypt = null;
                                    }
                                    else
                                    {
                                        if (!menu.IsEncyption.Value)
                                        {
                                            item.IsEncypt = null;
                                        }
                                    }
                                }
                                lstfinal.Add(item);
                            }
                        }
                    }
                }
                var response = new { dataNotSelect = obj[0], dataSelected = lstfinal };

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
                    if (aut != null)
                    {
                        if (name.Trim().ToLower() != aut.AuthorityName.Trim().ToLower())
                        {
                            if (db.TblAuthority.Where(o => o.OrganizationId == org.OrganizationId && o.IsDelete == false
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
                if (authority == null)
                {
                    return true;
                }

            }
            catch (Exception ex)
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
        public List<TblRoleViewModel> GetModuleList(int userId)
        {
            var lstMenu = new List<string>();
            var lstRole = new List<TblRoleViewModel>();
            var lstRoleOutPut = new List<TblRoleViewModel>();
            var lstMenuCode = db.TblMenu.Where(x => x.ParentCode == AccountConstant.MENU_PARENT_CODE).ToList(); //
                                                                                                                //if (lstMenuCode.Count > 0)
                                                                                                                //{
                                                                                                                //var lstAuthId = db.TblAuthority.Where(x => db.TblAuthorityUser.Where(c => c.UserId == userId && x.IsLock == false && x.IsDelete == false).Select(c => c.AuthorityId).Contains(x.AuthorityId)).ToList();
                                                                                                                //if (lstAuthId.Count > 0)
                                                                                                                //{
                                                                                                                //    foreach (var authId in lstAuthId)
                                                                                                                //    {
                                                                                                                //        if (authId.AuthorityType == AccountConstant.Admin)
                                                                                                                //        {
                                                                                                                //            foreach (var menu in lstMenuCode)
                                                                                                                //            {
                                                                                                                //                lstMenu.Add(menu.MenuCode);
                                                                                                                //            }
                                                                                                                //            foreach (var menu in lstMenu)
                                                                                                                //            {
                                                                                                                //                var lstMenuChild = db.TblMenu.Where(x => x.ParentCode == menu).ToList();
                                                                                                                //                foreach (var menuChild in lstMenuChild)
                                                                                                                //                {
                                                                                                                //                    TblRoleViewModel roleOutPut = new TblRoleViewModel();
                                                                                                                //                    roleOutPut.ParentName = db.TblMenu.Where(x => x.MenuCode == menu).FirstOrDefault().MenuName;
                                                                                                                //                    roleOutPut.ParentCode = menu;
                                                                                                                //                    roleOutPut.MenuName = menuChild.MenuName;
                                                                                                                //                    roleOutPut.MenuCode = menuChild.MenuCode;
                                                                                                                //                    roleOutPut.IsEncypt = true;
                                                                                                                //                    roleOutPut.IsShow = true;
                                                                                                                //                    roleOutPut.IsShow = true;
                                                                                                                //                    roleOutPut.IsAdd = true;
                                                                                                                //                    roleOutPut.IsEdit = true;
                                                                                                                //                    roleOutPut.IsEditAll = true;
                                                                                                                //                    roleOutPut.IsDelete = true;
                                                                                                                //                    roleOutPut.IsDeleteAll = true;
                                                                                                                //                    roleOutPut.IsImport = true;
                                                                                                                //                    roleOutPut.IsExport = true;
                                                                                                                //                    roleOutPut.IsPrint = true;
                                                                                                                //                    roleOutPut.IsApprove = true;
                                                                                                                //                    roleOutPut.IsEnable = true;
                                                                                                                //                    roleOutPut.IsPermission = true;
                                                                                                                //                    roleOutPut.IsFirstExtend = true;
                                                                                                                //                    roleOutPut.IsSecondExtend = true;
                                                                                                                //                    roleOutPut.IsThirdExtend = true;
                                                                                                                //                    roleOutPut.IsFouthExtend = true;
                                                                                                                //                    lstRoleOutPut.Add(roleOutPut);
                                                                                                                //                }
                                                                                                                //            }
                                                                                                                //            return lstRoleOutPut;
                                                                                                                //        }
                                                                                                                //        else
                                                                                                                //        {
                                                                                                                //            foreach (var menuCode in lstMenuCode)
                                                                                                                //            {
                                                                                                                //                var result = db.TblRole.Where(x => x.MenuCode == menuCode.MenuCode && x.AuthorityId == authId.AuthorityId).FirstOrDefault();
                                                                                                                //                if (result != null)
                                                                                                                //                {
                                                                                                                //                    var role = new TblRoleViewModel();
                                                                                                                //                    var roleDTO = new RoleDTO();
                                                                                                                //                    roleDTO.MenuName = db.TblMenu.Where(c => c.MenuCode == menuCode.MenuCode).FirstOrDefault().MenuName;
                                                                                                                //                    roleDTO.Id = result.Id;
                                                                                                                //                    roleDTO.AuthorityId = result.AuthorityId;
                                                                                                                //                    roleDTO.IsEncypt = result.IsEncypt;
                                                                                                                //                    roleDTO.IsShowAll = result.IsShowAll;
                                                                                                                //                    roleDTO.IsShow = result.IsShow;
                                                                                                                //                    roleDTO.IsAdd = result.IsAdd;
                                                                                                                //                    roleDTO.IsEditAll = result.IsEditAll;
                                                                                                                //                    roleDTO.IsEdit = result.IsEdit;
                                                                                                                //                    roleDTO.IsDeleteAll = result.IsDeleteAll;
                                                                                                                //                    roleDTO.IsDelete = result.IsDelete;
                                                                                                                //                    roleDTO.IsImport = result.IsImport;
                                                                                                                //                    roleDTO.IsExport = result.IsExport;
                                                                                                                //                    roleDTO.IsPrint = result.IsPrint;
                                                                                                                //                    roleDTO.IsApprove = result.IsApprove;
                                                                                                                //                    roleDTO.IsEnable = result.IsEnable;
                                                                                                                //                    roleDTO.IsPermission = result.IsPermission;
                                                                                                                //                    roleDTO.IsFirstExtend = result.IsFirstExtend;
                                                                                                                //                    roleDTO.IsSecondExtend = result.IsSecondExtend;
                                                                                                                //                    roleDTO.IsThirdExtend = result.IsThirdExtend;
                                                                                                                //                    roleDTO.IsFouthExtend = result.IsFouthExtend;
                                                                                                                //                    role.Roles.Add(roleDTO);
                                                                                                                //                    lstMenu.Add(role.MenuName);
                                                                                                                //                }
                                                                                                                //            }
                                                                                                                //        }
                                                                                                                //    }
                                                                                                                //}
                                                                                                                //}
                                                                                                                //var lst = lstMenu.Distinct().ToList();
                                                                                                                //foreach (var item in lst)
                                                                                                                //{
                                                                                                                //var result = lstRole.Where(x => x.MenuName.Equals(item)).ToList();
                                                                                                                //if (result.Count > 0)
                                                                                                                //{
                                                                                                                //    TblRoleViewModel roleOutPut = new TblRoleViewModel();
                                                                                                                //    roleOutPut.MenuName = item;
                                                                                                                //    for (int i = 0; i < result.Count; i++)
                                                                                                                //    {
                                                                                                                //        roleOutPut.IsEncypt = result[i].IsEncypt == true ? true : false;
                                                                                                                //        roleOutPut.IsShow = result[i].IsShow == true ? true : false;
                                                                                                                //        roleOutPut.IsShow = result[i].IsShowAll == true ? true : false;
                                                                                                                //        roleOutPut.IsAdd = result[i].IsAdd == true ? true : false;
                                                                                                                //        roleOutPut.IsEdit = result[i].IsEdit == true ? true : false;
                                                                                                                //        roleOutPut.IsEditAll = result[i].IsEditAll == true ? true : false;
                                                                                                                //        roleOutPut.IsDelete = result[i].IsDelete == true ? true : false;
                                                                                                                //        roleOutPut.IsDeleteAll = result[i].IsDeleteAll == true ? true : false;
                                                                                                                //        roleOutPut.IsImport = result[i].IsImport == true ? true : false;
                                                                                                                //        roleOutPut.IsExport = result[i].IsExport == true ? true : false;
                                                                                                                //        roleOutPut.IsPrint = result[i].IsPrint == true ? true : false;
                                                                                                                //        roleOutPut.IsApprove = result[i].IsApprove == true ? true : false;
                                                                                                                //        roleOutPut.IsEnable = result[i].IsEnable == true ? true : false;
                                                                                                                //        roleOutPut.IsPermission = result[i].IsPermission == true ? true : false;
                                                                                                                //        roleOutPut.IsFirstExtend = result[i].IsFirstExtend == true ? true : false;
                                                                                                                //        roleOutPut.IsSecondExtend = result[i].IsSecondExtend == true ? true : false;
                                                                                                                //        roleOutPut.IsThirdExtend = result[i].IsThirdExtend == true ? true : false;
                                                                                                                //        roleOutPut.IsFouthExtend = result[i].IsFouthExtend == true ? true : false;
                                                                                                                //    }
                                                                                                                //lstRoleOutPut.Add(roleOutPut);
                                                                                                                //}
                                                                                                                //}
            return lstRoleOutPut;
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
                RoleDTO checkRoleFalse = new RoleDTO();
                var lstRole = new List<string>();
                var lstRoleDistinct = new List<string>();
                var lstMenuRole = new List<TblRole>();
                var lstRoleAll = new AllRoleDTO();
                var lstRoleOutPut = new List<TblRoleViewModel>();
                var lstMenuCode = db.TblMenu.Where(x => x.ParentCode == AccountConstant.MENU_PARENT_CODE && x.IsDelete == false && x.IsActive == true).ToList();

                var orgId = db.TblOrganizationUser.Where(x => x.UserId == userId).FirstOrDefault().OrganizationId;
                if (orgId != null)
                {
                    var authIdAdmin = db.TblAuthority.Where(x => x.OrganizationId == orgId && x.AuthorityType == AccountConstant.Admin && x.IsDelete == false).FirstOrDefault().AuthorityId;
                    if (authIdAdmin > 0)
                    {
                        var userIdOfAdminGroup = db.TblAuthorityUser.Where(x => x.AuthorityId == authIdAdmin).ToList();
                        var isAdminUser = userIdOfAdminGroup.Where(x => x.UserId == userId).FirstOrDefault();
                        if (isAdminUser != null)
                        {
                            foreach (var menu in lstMenuCode)
                            {
                                TblRoleViewModel tblRoleViewModel = new TblRoleViewModel();
                                switch (menu.MenuCode)
                                {
                                    case AccountConstant.MENU_CATEGORY:
                                        var roleDTO = new RoleDTO();
                                        tblRoleViewModel.ParentName = menu.MenuName;
                                        tblRoleViewModel.ParentCode = menu.MenuCode;
                                        roleDTO.MenuName = menu.MenuName;
                                        roleDTO.MenuCode = menu.MenuCode;
                                        roleDTO.IsEncypt = menu.IsEncyption == true ? menu.IsEncyption : roleDTO.IsEncypt;
                                        roleDTO.IsShow = true;
                                        roleDTO.IsShowAll = true;
                                        roleDTO.IsAdd = true;
                                        roleDTO.IsEdit = true;
                                        roleDTO.IsEditAll = true;
                                        roleDTO.IsDelete = true;
                                        roleDTO.IsDeleteAll = true;
                                        roleDTO.IsImport = true;
                                        roleDTO.IsExport = true;
                                        roleDTO.IsPrint = true;
                                        roleDTO.IsApprove = true;
                                        roleDTO.IsEnable = true;
                                        roleDTO.IsPermission = true;
                                        roleDTO.IsFirstExtend = true;
                                        roleDTO.IsSecondExtend = true;
                                        roleDTO.IsThirdExtend = true;
                                        roleDTO.IsFouthExtend = true;
                                        tblRoleViewModel.Roles.Add(roleDTO);
                                        lstRoleAll.AllRoles.Add(roleDTO);
                                        lstRoleOutPut.Add(tblRoleViewModel);
                                        break;
                                    case AccountConstant.MENU_CIMS_LIST:
                                        var roleDTO1 = new RoleDTO();
                                        tblRoleViewModel.ParentName = menu.MenuName;
                                        tblRoleViewModel.ParentCode = menu.MenuCode;
                                        roleDTO1.MenuName = menu.MenuName;
                                        roleDTO1.MenuCode = menu.MenuCode;
                                        roleDTO1.IsEncypt = menu.IsEncyption == true ? menu.IsEncyption : roleDTO1.IsEncypt;
                                        roleDTO1.IsShow = true;
                                        roleDTO1.IsShowAll = true;
                                        roleDTO1.IsAdd = true;
                                        roleDTO1.IsEdit = true;
                                        roleDTO1.IsEditAll = true;
                                        roleDTO1.IsDelete = true;
                                        roleDTO1.IsDeleteAll = true;
                                        roleDTO1.IsImport = true;
                                        roleDTO1.IsExport = true;
                                        roleDTO1.IsPrint = true;
                                        roleDTO1.IsApprove = true;
                                        roleDTO1.IsEnable = true;
                                        roleDTO1.IsPermission = true;
                                        roleDTO1.IsFirstExtend = true;
                                        roleDTO1.IsSecondExtend = true;
                                        roleDTO1.IsThirdExtend = true;
                                        roleDTO1.IsFouthExtend = true;
                                        tblRoleViewModel.Roles.Add(roleDTO1);
                                        lstRoleAll.AllRoles.Add(roleDTO1);
                                        lstRoleOutPut.Add(tblRoleViewModel);
                                        break;
                                    default:
                                        tblRoleViewModel.ParentName = menu.MenuName;
                                        tblRoleViewModel.ParentCode = menu.MenuCode;
                                        var lstMenuChild = db.TblMenu.Where(x => x.ParentCode == menu.MenuCode).ToList();
                                        foreach (var menuChild in lstMenuChild)
                                        {
                                            var roleDTO2 = new RoleDTO();
                                            roleDTO2.MenuName = menuChild.MenuName;
                                            roleDTO2.MenuCode = menuChild.MenuCode;
                                            if (menuChild.MenuCode == AccountConstant.MENU_ADMIN_USER)
                                            {
                                                roleDTO2.IsEncypt = menuChild.IsEncyption == true ? menuChild.IsEncyption : roleDTO2.IsEncypt;
                                            }
                                            else
                                            {
                                                roleDTO2.IsEncypt = menu.IsEncyption == true ? menu.IsEncyption : roleDTO2.IsEncypt;
                                            }                                                                                       
                                            roleDTO2.IsShow = true;
                                            roleDTO2.IsShowAll = true;
                                            roleDTO2.IsAdd = true;
                                            roleDTO2.IsEdit = true;
                                            roleDTO2.IsEditAll = true;
                                            roleDTO2.IsDelete = true;
                                            roleDTO2.IsDeleteAll = true;
                                            roleDTO2.IsImport = true;
                                            roleDTO2.IsExport = true;
                                            roleDTO2.IsPrint = true;
                                            roleDTO2.IsApprove = true;
                                            roleDTO2.IsEnable = true;
                                            roleDTO2.IsPermission = true;
                                            roleDTO2.IsFirstExtend = true;
                                            roleDTO2.IsSecondExtend = true;
                                            roleDTO2.IsThirdExtend = true;
                                            roleDTO2.IsFouthExtend = true;
                                            tblRoleViewModel.Roles.Add(roleDTO2);
                                            lstRoleAll.AllRoles.Add(roleDTO2);
                                        }
                                        lstRoleOutPut.Add(tblRoleViewModel);
                                        break;
                                }
                            }
                            lstRoleAll.RoleModule = lstRoleOutPut;
                            return lstRoleAll;
                        }
                        else
                        {
                            var lstAuth = db.TblAuthority.Where(x => x.OrganizationId == orgId && x.AuthorityType != AccountConstant.Admin && x.AuthorityType != AccountConstant.SuperAdmin && x.IsDelete == false && x.IsLock == false).ToList();
                            if (lstAuth.Count > 0)
                            {
                                var lstAuthUser = lstAuth.Where(x => db.TblAuthorityUser.Where(c => c.UserId == userId && x.IsLock == false && x.IsDelete == false).Select(c => c.AuthorityId).Contains(x.AuthorityId)).ToList();
                                if (lstAuthUser.Count > 0)
                                {
                                    foreach (var authUser in lstAuthUser)
                                    {
                                        var result = db.TblRole.Where(x => x.AuthorityId == authUser.AuthorityId).ToList();
                                        lstMenuRole.AddRange(result);
                                        foreach (var role in result)
                                        {
                                            lstRole.Add(role.MenuCode);
                                        }
                                    }
                                    lstRoleDistinct = lstRole.Distinct().ToList();
                                    var lstMenu = new List<TblMenu>();
                                    foreach (var menuCode in lstRoleDistinct)
                                    {
                                        var result = db.TblMenu.Where(x => x.MenuCode == menuCode && x.IsDelete == false && x.IsActive == true).FirstOrDefault();
                                        if (result != null)
                                        {
                                            lstMenu.Add(result);
                                        }
                                    }
                                    foreach (var item in lstMenu)
                                    {
                                        switch (item.MenuCode)
                                        {
                                            case AccountConstant.MENU_CATEGORY:
                                                TblRoleViewModel tblRoleViewModel = new TblRoleViewModel();
                                                tblRoleViewModel.ParentName = db.TblMenu.Where(x => x.MenuCode == item.MenuCode).FirstOrDefault().MenuName;
                                                tblRoleViewModel.ParentCode = item.MenuCode;
                                                var result = lstMenuRole.Where(x => x.MenuCode == item.MenuCode).ToList();
                                                if (result.Count > 0)
                                                {
                                                    RoleDTO roleDTO1 = new RoleDTO();
                                                    
                                                    foreach (var item1 in result)
                                                    {
                                                        roleDTO1 = accountCommon.JoinPermission(roleDTO1, item1);                                                      
                                                    }
                                                    roleDTO1.MenuName = item.MenuName;
                                                    roleDTO1.MenuCode = item.MenuCode;
                                                    if (!accountCommon.IsRoleFalse(checkRoleFalse, roleDTO1))
                                                    {
                                                        tblRoleViewModel.Roles.Add(roleDTO1);
                                                        lstRoleAll.AllRoles.Add(roleDTO1);
                                                        lstRoleOutPut.Add(tblRoleViewModel);
                                                    }
                                                }
                                                break;
                                            case AccountConstant.MENU_CIMS_LIST:
                                                TblRoleViewModel tblRoleViewModel1 = new TblRoleViewModel();
                                                tblRoleViewModel1.ParentName = db.TblMenu.Where(x => x.MenuCode == item.MenuCode).FirstOrDefault().MenuName;
                                                tblRoleViewModel1.ParentCode = item.MenuCode;
                                                var result1 = lstMenuRole.Where(x => x.MenuCode == item.MenuCode).ToList();
                                                if (result1.Count > 0)
                                                {
                                                    RoleDTO roleDTO2 = new RoleDTO();                                                    
                                                    foreach (var item1 in result1)
                                                    {
                                                        roleDTO2 = accountCommon.JoinPermission(roleDTO2, item1);
                                                    }
                                                    roleDTO2.MenuName = item.MenuName;
                                                    roleDTO2.MenuCode = item.MenuCode;
                                                    if (!accountCommon.IsRoleFalse(checkRoleFalse, roleDTO2))
                                                    {
                                                        tblRoleViewModel1.Roles.Add(roleDTO2);
                                                        lstRoleAll.AllRoles.Add(roleDTO2);
                                                        lstRoleOutPut.Add(tblRoleViewModel1);
                                                    }
                                                }
                                                break;
                                            case AccountConstant.MENU_PROFILE:
                                                TblRoleViewModel tblRoleViewModel2 = new TblRoleViewModel();
                                                tblRoleViewModel2.ParentName = db.TblMenu.Where(x => x.MenuCode == item.MenuCode).FirstOrDefault().MenuName;
                                                tblRoleViewModel2.ParentCode = item.MenuCode;
                                                RoleDTO roleDTO = new RoleDTO();
                                                roleDTO.MenuName = item.MenuName;
                                                roleDTO.MenuCode = item.MenuCode;
                                                roleDTO.IsEncypt = true;
                                                roleDTO.IsShow = true;
                                                roleDTO.IsShowAll = true;
                                                roleDTO.IsAdd = true;
                                                roleDTO.IsEdit = true;
                                                roleDTO.IsEditAll = true;
                                                roleDTO.IsDelete = true;
                                                roleDTO.IsDeleteAll = true;
                                                roleDTO.IsImport = true;
                                                roleDTO.IsExport = true;
                                                roleDTO.IsPrint = true;
                                                roleDTO.IsApprove = true;
                                                roleDTO.IsEnable = true;
                                                roleDTO.IsPermission = true;
                                                roleDTO.IsFirstExtend = true;
                                                roleDTO.IsSecondExtend = true;
                                                roleDTO.IsThirdExtend = true;
                                                roleDTO.IsFouthExtend = true;
                                                tblRoleViewModel2.Roles.Add(roleDTO);
                                                lstRoleAll.AllRoles.Add(roleDTO);
                                                lstRoleOutPut.Add(tblRoleViewModel2);
                                                break;
                                            default:
                                                if (lstRoleOutPut.Any(x => x.ParentCode == item.ParentCode))
                                                {
                                                    var result3 = lstMenuRole.Where(x => x.MenuCode == item.MenuCode).ToList();
                                                    if (result3.Count > 0)
                                                    {
                                                        RoleDTO roleDTO3 = new RoleDTO();                                                        
                                                        foreach (var item1 in result3)
                                                        {
                                                            roleDTO3 = accountCommon.JoinPermission(roleDTO3, item1);
                                                        }
                                                        roleDTO3.MenuName = item.MenuName;
                                                        roleDTO3.MenuCode = item.MenuCode;
                                                        if (!accountCommon.IsRoleFalse(checkRoleFalse, roleDTO3))
                                                        {
                                                            lstRoleOutPut.Where(x => x.ParentCode == item.ParentCode).FirstOrDefault().Roles.Add(roleDTO3);
                                                            lstRoleAll.AllRoles.Add(roleDTO3);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    TblRoleViewModel tblRoleViewModel3 = new TblRoleViewModel();
                                                    tblRoleViewModel3.ParentName = db.TblMenu.Where(x => x.MenuCode == item.ParentCode).FirstOrDefault().MenuName;
                                                    tblRoleViewModel3.ParentCode = item.ParentCode;
                                                    var result3 = lstMenuRole.Where(x => x.MenuCode == item.MenuCode).ToList();
                                                    if (result3.Count > 0)
                                                    {
                                                        RoleDTO roleDTO3 = new RoleDTO();                                                        
                                                        foreach (var item1 in result3)
                                                        {
                                                            roleDTO3 = accountCommon.JoinPermission(roleDTO3, item1);
                                                        }
                                                        roleDTO3.MenuName = item.MenuName;
                                                        roleDTO3.MenuCode = item.MenuCode;
                                                        if (!accountCommon.IsRoleFalse(checkRoleFalse, roleDTO3))
                                                        {
                                                            tblRoleViewModel3.Roles.Add(roleDTO3);
                                                            lstRoleAll.AllRoles.Add(roleDTO3);
                                                            lstRoleOutPut.Add(tblRoleViewModel3);
                                                        }
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                            lstRoleAll.RoleModule = lstRoleOutPut;
                            return lstRoleAll;
                        }
                    }
                    else
                    {
                        return AuthorityConstant.Error;
                    }
                }
                else
                {
                    return AuthorityConstant.Error;
                }
            }
            catch (Exception)
            {
                return AuthorityConstant.Error;
            }

        }

        /// <summary>
        ///  GetUsersToGrantAuthority method: Danh sách users để thêm mới vào nhóm quyền
        /// </summary>
        /// <param name="authorityId"></param>
        /// <returns>Danh sách users</returns>
        public object GetUsersToGrantAuthority(int authorityId, int userId, string orgCode)
        {
            try
            {
                // Lấy mã đơn vị
                var orgId = db.TblOrganization.Where(x => x.OrganizationCode == orgCode).FirstOrDefault().OrganizationId;
                // Lấy tất cả user của đơn vị
                var lstUser = db.TblUsers.Where(x => db.TblOrganizationUser.Where(c => c.OrganizationId == orgId && c.UserId == x.Id && x.IsLock == false && x.IsDelete == false).Select(c => c.UserId).Contains(x.Id)).ToList();
                // Lấy tất cả nhóm quyền của đơn vị
                var lstAuth = db.TblAuthority.Where(x => x.OrganizationId == orgId).ToList();
                // Lấy Id của nhóm quyền admin đơn vị
                var adminId = lstAuth.Where(x => x.AuthorityType == AccountConstant.Admin).FirstOrDefault().AuthorityId;
                // Check nhóm quyền có tồn tại trong đơn vị không
                if (lstAuth.Any(x => x.AuthorityId == authorityId))
                {
                    // Danh sách users đã có trong nhóm quyền của đơn vị
                    var usersGrantedAuthority = lstUser.Where(x => db.TblAuthorityUser.Where(c => c.AuthorityId == authorityId).Select(c => c.UserId).Contains(x.Id)).Select(x => new UserDTO()
                    {
                        UserId = x.Id,
                        UserName = x.UserName,
                        FullName = x.FullName,
                        IsGrantedAuthority = true
                    }).ToList();
                    // Danh sách users chưa có trong nhóm quyền của đơn vị
                    var userNotGrantedAuthority = lstUser.Where(x => !db.TblAuthorityUser.Where(c => c.AuthorityId == authorityId).Select(c => c.UserId).Contains(x.Id)).Select(x => new UserDTO()
                    {
                        UserId = x.Id,
                        UserName = x.UserName,
                        FullName = x.FullName,
                        IsGrantedAuthority = false
                    }).ToList();
                    // Danh sách users là admin của đơn vị
                    var lstAdminUser = lstUser.Where(x => db.TblAuthorityUser.Where(c => c.AuthorityId == adminId).Select(c => c.UserId).Contains(x.Id)).Select(x => new UserDTO()
                    {
                        UserId = x.Id,
                        UserName = x.UserName,
                        FullName = x.FullName,
                        IsGrantedAuthority = false
                    }).ToList();
                    usersGrantedAuthority = usersGrantedAuthority.Where(x => !lstAdminUser.Where(c => c.UserId == x.UserId).Select(c => c.UserId).Contains(x.UserId)).ToList();
                    userNotGrantedAuthority = userNotGrantedAuthority.Where(x => !lstAdminUser.Where(c => c.UserId == x.UserId).Select(c => c.UserId).Contains(x.UserId)).ToList();
                    List<TblUserViewModel> model = new List<TblUserViewModel>();
                    model.AddRange(usersGrantedAuthority);
                    model.AddRange(userNotGrantedAuthority);
                    return model;
                }
                else
                {
                    return new { Message = AuthorityConstant.ExceptionGetUsersToGrantAuthority };
                }
            }
            catch (Exception)
            {
                //return "Error: Không có nhóm quyền này";
                return AuthorityConstant.ExceptionGetUsersToGrantAuthority;
            }
        }

        /// <summary>
        /// Function get list authority of org
        /// CreatedBy: HaiHM
        /// CreatedDate: 31/5/2019
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public object GetListAuthorityOfOrg(string orgCode)
        {
            try
            {
                var org = db.TblOrganization.Where(x => x.OrganizationCode == orgCode).FirstOrDefault();
                if (org != null)
                {
                    var obj = db.TblAuthority.Where(a => a.IsDelete == false && a.IsLock == false && a.OrganizationId == org.OrganizationId);
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
