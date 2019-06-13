using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.EmailService;
using AccountManagement.Repositories;
using AccountManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AccountManagement.Models.DataAccess
{
    public class AccountDA : IAccountRepository
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        private AccountCommon accountCommon;
        private SP_Account sp = new SP_Account();
        private EmailServiceEx _emailService = new EmailServiceEx();
        private IDistributedCache _distributedCache;
        private IConfiguration _config;
        private static IHostingEnvironment _environment;

        public AccountDA(IDistributedCache distributedCache, IConfiguration config, IHostingEnvironment environmen)
        {
            _distributedCache = distributedCache;
            accountCommon = new AccountCommon();
            _config = config;
            _environment = environmen;
        }

        public AccountDA() { }

        /// <summary>
        /// Function LoadDistributedCache
        /// CreatedBy: HaiHM
        /// CreatedDate: 12/04/2019
        /// </summary>
        /// <param name="distributedCache"></param>
        public void LoadDistributedCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Function GetStringCache - lấy dữ liệu theo key
        /// CreatedBy: HaiHM
        /// CreatedDate: 12/04/2019
        /// </summary>
        /// <param name="cacheKey">Key</param>
        /// <returns></returns>
        public string GetStringCache(string cacheKey)
        {
            return _distributedCache.GetString(cacheKey);
        }

        public async Task<string> GetStringAsync(string key)
        {
            var val = await _distributedCache.GetStringAsync(key);
            return val;
        }

        /// <summary>
        /// Function SetStringCache - Cập nhận object vào cache
        /// CreatedBy: HaiHM
        /// CreatedDate: 12/04/2019 
        /// </summary>
        /// <param name="cacheKey">key</param>
        /// <param name="obj">object</param>
        public void SetStringCache(string cacheKey, Object obj)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            _distributedCache.SetString(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(obj), options.SetSlidingExpiration(TimeSpan.FromMinutes(1)));

        }

        #region User
        /// <summary>
        /// Function get user login
        /// CreatedBy: System
        /// Modified: HaiHM
        /// ModifiedDate: 26/4/2019
        /// ModifiedConten: add check login and update LoginFail when login error
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="pw">password</param>
        /// <returns></returns>
        public TblUsers GetUsersLogin(string userName, string pw)
        {
            //string pw1 = "";
            //string pwSalf = "";
            //bool aa = accountCommon.ValidatePassword(pw1, pwSalf);

            try
            {
                //db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());


                TblUsers user = db.TblUsers.Where(a => a.UserName.Replace(" ", string.Empty).ToLower() == userName).FirstOrDefault();
                if (user != null)
                {
                    //Check Admin Org
                    var authorityType = from a in db.TblAuthority
                                        where (from c in db.TblAuthorityUser where c.UserId == user.Id && a.IsDelete == false && a.IsLock == false select c.AuthorityId).Contains(a.AuthorityId)
                                        select a;
                    List<TblAuthority> lstauT = authorityType.ToList();
                    if (lstauT.Count > 0)
                    {
                        TblAuthority auT = lstauT.FirstOrDefault();
                        if (auT.AuthorityType != null)
                        {
                            if (auT.AuthorityType == AccountConstant.Admin)
                            {
                                DateTime checkDate = user.ExpirationDate ?? DateTime.MinValue;
                                DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());
                                DateTime dbTime = DateTime.Parse(checkDate.ToShortDateString());
                                if (DateTime.Compare(dbTime, now) <= 0)
                                {
                                    user.IsLock = true;
                                    db.Entry(user).State = EntityState.Modified;
                                    db.SaveChanges();
                                    return user;
                                }
                            }
                        }
                    }
                    //End Check Admin Org
                    if (user.IsLock == true)
                        return user;
                    TblUsers userFind = db.TblUsers.Where(a => a.IsDelete == false && a.UserName.ToLower() == userName.ToLower() && accountCommon.ValidatePassword(pw, a.Password)).FirstOrDefault();
                    if (userFind == null)
                    {
                        //LoginFail += 1;
                        int count = Int32.Parse(_config[AccountConstant.CountLoginFail]);
                        user.LoginFail += 1;
                        if (user.LoginFail == count)
                        {
                            user.IsLock = true;
                        }
                        //user.LoginFail = user.LoginFail >= count ? 0 : user.LoginFail;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        user = null;
                    }
                    else
                    {
                        userFind.LoginFail = 0;
                        db.Entry(userFind).State = EntityState.Modified;
                        db.SaveChanges();
                        return userFind;
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Function Get User by Username
        /// CreatedBy: System
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public TblUsers GetUsersCustom(string userName, string orgCode)
        {
            try
            {
                TblUsers result = new TblUsers();
                TblUsers tblUsers = (TblUsers)db.TblUsers.Where(a => a.IsDelete == false
                    && a.UserName.Replace(" ", string.Empty).ToLower() == userName.Replace(" ", string.Empty).ToLower()
                    ).FirstOrDefault();
                TblOrganization org = GetOrganizationId(tblUsers.Id);
                tblUsers.CategoryCodeRole = org.OrganizationName;
                TblOrganization department = db.TblOrganization.Where(o => o.IsDelete == false && o.OrganizationCode == tblUsers.CategoryCodeDepartment).FirstOrDefault();
                if (department != null)
                {
                    tblUsers.CategoryCodeDepartment = department.OrganizationName;
                }
                //Position
                TblCategory category = db.TblCategory.Where(c => c.CategoryCode == tblUsers.Position && c.IsDelete == false).FirstOrDefault();
                if (category != null)
                {
                    tblUsers.Position = category.CategoryName;
                }
                if (tblUsers != null)
                {
                    result = new TblUsers();
                    result.Id = tblUsers.Id;
                    result.UserName = tblUsers.UserName;
                    result.FullName = tblUsers.FullName;
                    result.Email = tblUsers.Email;
                    result.PhoneNumber = tblUsers.PhoneNumber;
                    result.CreateBy = tblUsers.CreateBy;
                    result.CreateDate = tblUsers.CreateDate;
                    result.UpdateBy = tblUsers.UpdateBy;
                    result.UpdateDate = tblUsers.UpdateDate;
                    result.IsLock = tblUsers.IsLock;
                    result.Avatar = tblUsers.Avatar;
                    result.Address = tblUsers.Address;
                    result.LastLogin = tblUsers.LastLogin;
                    result.Position = tblUsers.Position;
                    result.Position = tblUsers.Position;
                    result.Gender = tblUsers.Gender;
                    result.BirthDay = tblUsers.BirthDay;
                    result.CategoryCodeDepartment = tblUsers.CategoryCodeDepartment;
                    result.CategoryCodeRole = tblUsers.CategoryCodeRole;
                }
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        /// <summary>
        /// Function get user infor
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public TblUsers GetUsers(string userName)
        {
            try
            {
                TblUsers tblUsers = (TblUsers)db.TblUsers.Where(a => a.IsDelete == false
                    && a.UserName.Replace(" ", string.Empty).ToLower() == userName.Replace(" ", string.Empty).ToLower()
                    ).FirstOrDefault();
                
                return tblUsers;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }


        /// <summary>
        /// Tạo mới thông tin thành viên
        /// CreatedBy: HaiHM
        /// CreatedDate: 12/04/2019
        /// </summary>
        /// <param name="model">object</param>
        /// <returns></returns>
        public int AddUser(UserAndOrgViewModel model)
        {
            int save = 0;
            string pass = "";
            string email = "";
            string phoneNumber = "";
            string fullname = "";
            string username = "";

            if (!string.IsNullOrEmpty(model.tblUsers.UserName))
            {
                username = model.tblUsers.UserName.Replace(" ", string.Empty).ToLower();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.Email))
            {
                email = model.tblUsers.Email.Replace(" ", string.Empty).ToLower();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.PhoneNumber))
            {
                phoneNumber = model.tblUsers.PhoneNumber.Replace(" ", string.Empty).ToLower();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.FullName))
            {
                fullname = model.tblUsers.FullName.TrimEnd().TrimStart();
            }
            try
            {
                TblUsers userChk = db.TblUsers.Where(a => a.IsDelete == false && a.UserName.Replace(" ", string.Empty).ToLower() == username).FirstOrDefault();
                // Check Email- email modified and not modify
                if (userChk != null)
                {
                    //Duplicate username
                    return save = AccountConstant.AddUserDuplicateUsername;
                }
                if (db.TblUsers.Where(u => u.IsDelete == false && u.Email.Replace(" ", string.Empty).ToLower() == email).Count() > 0)
                {
                    // Duplicate Email
                    return save = AccountConstant.AddUserDuplicateEmail;
                }
                if (db.TblUsers.Where(u => u.IsDelete == false && u.PhoneNumber.Replace(" ", string.Empty).ToLower() == phoneNumber).Count() > 0)
                {
                    // Duplicate PhoneNumber
                    return save = AccountConstant.AddUserDuplicatePhoneNumber;
                }
                using (var ts = new TransactionScope())
                {
                    if (userChk == null)
                    {
                        TblUsers tblUsers = new TblUsers();
                        pass = accountCommon.CreateRandomPassword();
                        string passHash = accountCommon.HashPassword(pass);
                        tblUsers.UserName = username;
                        tblUsers.Password = passHash;
                        tblUsers.FullName = fullname;
                        tblUsers.Email = email;
                        tblUsers.PhoneNumber = phoneNumber;
                        tblUsers.CreateBy = model.tblUsers.CreateBy;
                        tblUsers.CreateDate = DateTime.Now;
                        tblUsers.UpdateBy = model.tblUsers.UpdateBy;
                        tblUsers.UpdateDate = DateTime.Now;
                        tblUsers.IsDelete = false;
                        tblUsers.IsLock = model.tblUsers.IsLock;
                        tblUsers.Avatar = model.tblUsers.Avatar;
                        tblUsers.Address = model.tblUsers.Address;
                        tblUsers.LastLogin = null;
                        tblUsers.Position = model.tblUsers.Position;
                        tblUsers.EmailConfirmed = false;
                        tblUsers.Gender = model.tblUsers.Gender;
                        tblUsers.BirthDay = model.tblUsers.BirthDay;
                        tblUsers.HistoryPassword = passHash + AccountConstant.PasswordPolicySplit; // Ex: 21232f297a57a5a743894a0e4a801fc3=
                        tblUsers.ExpirationDate = DateTime.Now.AddDays(Int32.Parse(_config[AccountConstant.PasswordPolicyExpDateCreated])); // + 10 days
                        tblUsers.CategoryCodeDepartment = model.tblUsers.CategoryCodeDepartment;
                        tblUsers.CategoryCodeRole = model.tblUsers.CategoryCodeRole;

                        db.TblUsers.Add(tblUsers);
                        db.SaveChanges();

                        save = AccountConstant.AddUserSuccess;
                        int id = tblUsers.Id;
                        // Add table Reference
                        if (model.tblOrganization != null)
                        {
                            AddOrgUser(model.tblOrganization.OrganizationId, id);
                        }
                        if (model.tblAuthority != null)
                        {
                            if (model.tblAuthority.Count > 0)
                                AddAuthorityUser(model.tblAuthority, id);
                        }

                        // TransactionScope Complete
                        ts.Complete();
                    }
                }
                if (save == AccountConstant.AddUserSuccess)
                {
                    // Chua co link
                    SendMail(model.tblUsers.Email, model.tblUsers.FullName, model.tblUsers.UserName, pass, AccountConstant.ApiLinkCreated);
                }
                return save;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Function send mail
        /// CreatedBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <param name="emailTo">Reciever</param>
        /// <param name="user">Infor</param>
        /// <param name="pass">Infor</param>
        /// <param name="link">Infor</param>
        private void SendMail(string emailTo, string fullname, string user, string pass, string link)
        {
            TblRecieveEmail mail = new TblRecieveEmail();
            mail.Subject = accountCommon.SubjectCreate();
            mail.To = emailTo;
            mail.EmailContents = accountCommon.HtmlMailCreatedAccount(fullname, user, pass, link);
            //_emailService.SendEmailAsync(mail);
            _emailService.SendEmail(mail);
        }

        /// <summary>
        /// Function send mail reset
        /// CreatedBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="user"></param>
        /// <param name="link"></param>
        private void SendMailReset(string emailTo, TblUsers user, string link)
        {
            TblRecieveEmail mail = new TblRecieveEmail();
            mail.Subject = AccountConstant.SubjectSendMailReset;
            mail.To = emailTo;
            mail.EmailContents = accountCommon.HtmlMailResetAccount(user.FullName, link);
            //_emailService.SendEmailAsync(mail);
            _emailService.SendEmail(mail);
        }

        /// <summary>
        /// Function Add user in authoritys
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <param name="authoritys"></param>
        /// <param name="userId"></param>
        private void AddAuthorityUser(List<TblAuthority> authoritys, int userId)
        {
            try
            {
                // Delete old record here
                List<TblAuthorityUser> deleteAU = db.TblAuthorityUser.Where(au => au.UserId == userId).ToList<TblAuthorityUser>();
                using (var ts = new TransactionScope())
                {
                    foreach (var item in deleteAU)
                    {
                        db.TblAuthorityUser.Remove(item);
                        db.SaveChanges();
                    }
                    // TransactionScope complete
                    ts.Complete();
                }

                using (var ts = new TransactionScope())
                {
                    foreach (var item in authoritys)
                    {
                        TblAuthorityUser ru = new TblAuthorityUser();
                        ru.AuthorityId = item.AuthorityId;
                        ru.UserId = userId; // id new created
                        db.TblAuthorityUser.Add(ru);
                        db.SaveChanges();
                    }
                    // TransactionScope complete
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Function Add organization for user
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <param name="organizationId">ID organization </param>
        /// <param name="userId">ID user</param>
        private void AddOrgUser(int organizationId, int userId)
        {
            try
            {
                // Delete old record here
                List<TblOrganizationUser> deleteOU = db.TblOrganizationUser.Where(ouser => ouser.UserId == userId).ToList<TblOrganizationUser>();
                if (deleteOU != null)
                {
                    if (deleteOU.Count >= 1)
                    {
                        using (var ts = new TransactionScope())
                        {
                            foreach (var item in deleteOU)
                            {
                                db.TblOrganizationUser.Remove(item);
                                db.SaveChanges();
                            }
                            // TransactionScope complete
                            ts.Complete();
                        }
                    }
                }

                TblOrganizationUser ou = new TblOrganizationUser();
                ou.OrganizationId = organizationId;
                ou.UserId = userId; // id new created
                db.TblOrganizationUser.Add(ou);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Function Edit User
        /// CreatedBy: HaiHM
        /// CreatedDate: 15/04/2019
        /// </summary>
        /// <param name="model">object</param>
        /// <returns></returns>
        public int EditUser(UserAndOrgViewModel model)
        {
            int edit = 0;
            string email = "";
            string phoneNumber = "";
            string fullname = "";

            if (!string.IsNullOrEmpty(model.tblUsers.Email))
            {
                email = model.tblUsers.Email.Replace(" ", string.Empty).ToLower();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.PhoneNumber))
            {
                phoneNumber = model.tblUsers.PhoneNumber.Replace(" ", string.Empty).ToLower();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.FullName))
            {
                fullname = model.tblUsers.FullName.TrimEnd().TrimStart();
            }
            try
            {
                TblUsers userChk = db.TblUsers.Where(a => a.UserName == model.tblUsers.UserName && a.Id == model.tblUsers.Id).FirstOrDefault();
                if (userChk != null)
                {
                    // Check Email- email modified and not modify
                    if (userChk.Email.Replace(" ", string.Empty).ToLower() != email)
                    {
                        if (db.TblUsers.Where(u => u.IsDelete == false && u.Email.Replace(" ", string.Empty).ToLower() == email).Count() > 0)
                        {
                            // Duplicate Email
                            return edit = AccountConstant.EditUserDuplicateEmail;
                        }
                    }
                    // Check PhoneNumber- PhoneNumber modified and not modify
                    if (userChk.PhoneNumber.Replace(" ", string.Empty).ToLower() != phoneNumber)
                    {
                        if (db.TblUsers.Where(u => u.IsDelete == false && u.PhoneNumber.Replace(" ", string.Empty).ToLower() == phoneNumber).Count() > 0)
                        {
                            // Duplicate PhoneNumber
                            return edit = AccountConstant.EditUserDuplicatePhoneNumber;
                        }
                    }
                    using (var ts = new TransactionScope())
                    {
                        userChk.FullName = fullname;
                        userChk.Email = email;
                        userChk.PhoneNumber = phoneNumber;
                        userChk.UpdateBy = model.tblUsers.UpdateBy;
                        userChk.UpdateDate = DateTime.Now;
                        //userChk.IsDelete = model.tblUsers.IsDelete;
                        userChk.IsLock = model.tblUsers.IsLock;
                        userChk.Avatar = model.tblUsers.Avatar;
                        userChk.Address = model.tblUsers.Address;
                        //userChk.LastLogin = model.tblUsers.LastLogin;
                        userChk.Position = model.tblUsers.Position;
                        //userChk.EmailConfirmed = model.tblUsers.EmailConfirmed;
                        userChk.Gender = model.tblUsers.Gender;
                        userChk.BirthDay = model.tblUsers.BirthDay;
                        userChk.CategoryCodeDepartment = model.tblUsers.CategoryCodeDepartment;
                        //userChk.CategoryCodeRole = model.tblUsers.CategoryCodeRole;

                        db.Entry(userChk).State = EntityState.Modified;
                        db.SaveChanges();
                        edit = AccountConstant.EditUserSuccess;

                        // Add table Reference
                        if (model.tblOrganization != null)
                        {
                            AddOrgUser(model.tblOrganization.OrganizationId, model.tblUsers.Id);
                        }
                        if (model.tblAuthority != null)
                        {
                            if (model.tblAuthority.Count > 0)
                            {
                                AddAuthorityUser(model.tblAuthority, userChk.Id);
                            }
                        }
                        // TransactionScope Complete
                        ts.Complete();
                    }
                }
                return edit;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Function get org by userId
        /// CreatedBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TblOrganization GetOrganizationId(int userId)
        {
            try
            {
                TblOrganizationUser tblOrganizationUser = db.TblOrganizationUser.Where(a => a.UserId == userId).FirstOrDefault();
                if (tblOrganizationUser != null)
                {
                    TblOrganization tblOrganization = db.TblOrganization.Where(a => a.IsActive == true && a.IsDelete == false &&
                                a.OrganizationId == tblOrganizationUser.OrganizationId).FirstOrDefault();
                    return tblOrganization;
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

        #region HaiHM
        /// <summary>
        /// Function Delete user
        /// CreatedBy: HaiHM
        /// CreatedDate: 15/04/2019
        /// </summary>
        /// <param name="userID">id of user</param>
        /// <returns></returns>
        public int DeleteUser(int userID)
        {
            try
            {
                TblUsers userChk = db.TblUsers.Where(u => u.IsDelete == false && u.Id == userID).FirstOrDefault();
                if (userChk != null)
                {
                    userChk.IsDelete = true; // change status here
                    db.Entry(userChk).State = EntityState.Modified;
                    db.SaveChanges();
                    return AccountConstant.DeleteUserSuccess;
                }
                return AccountConstant.DeleteUserFail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AccountConstant.DeleteUserFail;
            }
        }

        /// <summary>
        /// Function get user by id
        /// CreatedBy: HaiHM
        /// CreatedDate: 17/04/2019
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns></returns>
        public TblUsers GetUserById(int id)
        {
            // check permission here
            //var user = (TblUsers)sp.GetUserById(id);
            try
            {
                TblUsers tblUsers = db.TblUsers.Where(u => u.Id == id && u.IsDelete == false).FirstOrDefault();
                return tblUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function get all aurhority of user
        /// CreatedBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TblAuthority> ListAuthorityOfUser(int userId)
        {
            try
            {
                var lstAuthority = from a in db.TblAuthority
                                   where (from c in db.TblAuthorityUser where c.UserId == userId && a.IsDelete == false && a.IsLock == false select c.AuthorityId).Contains(a.AuthorityId)
                                   select a;
                List<TblAuthority> lst = lstAuthority.ToList<TblAuthority>();
                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function Reset password user
        /// CreatedBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int ResetPassUser(TblUsers user)
        {
            // check org => get mail config =>link Reset
            try
            {
                TblUsers userChk = db.TblUsers.Where(u => u.Id == user.Id
                                && u.IsDelete == false).FirstOrDefault();

                if (userChk != null)
                {
                    // gentoken
                    string token = GenerateJSONWebTokenReset(userChk);
                    // send mail
                    SendMailReset(userChk.Email, userChk, AccountConstant.ApiLinkReset + "?token=" + token); // Chua co link
                    return AccountConstant.ResetPassSuccess;
                }
                else
                {
                    return AccountConstant.ResetPassFail;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return AccountConstant.ResetPassFail;
            }

        }

        /// <summary>
        /// Function Lock thành viên
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <returns></returns>
        public bool LockUser(TblUsers user)
        {
            try
            {
                TblUsers userChk = db.TblUsers.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userChk != null)
                {
                    userChk.IsLock = true; // change lock here
                    user.UpdateDate = DateTime.Now;
                    db.Entry(userChk).State = EntityState.Modified;
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
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Function Active thành viên
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <returns></returns>
        public bool ActiveUser(TblUsers user)
        {
            try
            {
                TblUsers userChk = db.TblUsers.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userChk != null)
                {
                    userChk.IsLock = false; // change lock here
                    user.UpdateDate = DateTime.Now;
                    db.Entry(userChk).State = EntityState.Modified;
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
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Function Tìm kiếm thành viên
        /// CreatedBy: HaiHM
        /// CreatedDate: 22/04/2019
        /// </summary>
        /// <param strFilter="object filter"></param>
        /// <returns></returns>
        public object SearchUser(string strFilter)
        {
            try
            {
                List<List<dynamic>> obj = sp.SearchUser(strFilter);
                var response = new { data = obj[0], paging = obj[1] };
                //Active comment for Unitest
                //int r = obj[0].Count();
                //return r;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new { code = 400, data = ex.Message };
            }
        }

        /// <summary>
        /// chức năng lấy danh sách chức vụ theo CategoryTypeCode
        /// CreatedBy: HaiHM
        /// CreatedDate: 23/04/2019
        /// </summary>
        /// <param name="CategoryTypeCode">Type get</param>
        /// <returns></returns>
        public object GetAllCategory(string CategoryTypeCode)
        {
            try
            {
                List<List<dynamic>> obj = sp.GetAllCategory(CategoryTypeCode);
                var response = new { data = obj[0] };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new { code = 400, data = ex.Message };
            }
        }

        /// <summary>
        /// Function Change password
        /// CreatedBy: HaiHM
        /// CreatedDate: 24/04/2019
        /// </summary>
        /// <param name="viewModel">object new password</param>
        /// <param name="user">object</param>
        /// <returns></returns>
        public int ChangePassword(ChangePassViewModel viewModel, TblUsers user)
        {
            int result = 0;

            // compare 2 password
            if (viewModel.Pass1 != viewModel.Pass2 && !string.IsNullOrEmpty(viewModel.Pass1))
            {
                // 2 new password can not same
                return AccountConstant.NotSame;
            }
            try
            {
                if (user != null)
                {
                    if (user.EmailConfirmed == false)
                    {
                        result = CheckPasswordChange(viewModel.Pass1, user.HistoryPassword, user);
                    }
                    else
                    {
                        if (accountCommon.ValidatePassword(viewModel.oldPassword, user.Password))
                        {
                            result = CheckPasswordChange(viewModel.Pass1, user.HistoryPassword, user);
                        }
                        else
                        {
                            // input password error
                            return AccountConstant.InputPasswordError;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return result;
            }
        }

        /// <summary>
        /// Function check password
        /// CreatedBy: HaiHM
        /// CreatedDate: 24/04/2019 
        /// </summary>
        /// <param name="pass">new password</param>
        /// <param name="ListOldPassword">List old password </param>
        /// <returns></returns>
        private int CheckPasswordChange(string pass, string ListOldPassword, TblUsers user)
        {
            int result = AccountConstant.ChangePasswordSuccess;
            int runFor = 0;
            if (!string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(ListOldPassword))
            {
                string[] authorsList = ListOldPassword.Split(AccountConstant.PasswordPolicySplit);
                for (runFor = 0; runFor < authorsList.Count() - 1; runFor++)
                {
                    if (accountCommon.ValidatePassword(pass, authorsList[runFor]))
                    {
                        // duplicate 3 old password
                        return AccountConstant.ChangePasswordDuplicateOldPass;
                    }
                }

                int count = Int32.Parse(_config[AccountConstant.PasswordPolicyCountPasswordHistory]);
                // Using Bcrypt
                pass = accountCommon.HashPassword(pass);

                if (authorsList.Count() > count)
                {
                    var strPass = "";
                    for (runFor = 1; runFor < authorsList.Count() - 1; runFor++)
                    {
                        strPass += authorsList[runFor] + AccountConstant.PasswordPolicySplit;
                    }
                    user.HistoryPassword = strPass + pass + AccountConstant.PasswordPolicySplit;
                    // add 180 days
                    user.ExpirationDate = DateTime.Now.AddDays(Int32.Parse(_config[AccountConstant.PasswordPolicyPasDuration]));
                    user.Password = pass;
                    user.EmailConfirmed = true;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    user.HistoryPassword += pass + AccountConstant.PasswordPolicySplit;
                    // add 180 days
                    user.ExpirationDate = DateTime.Now.AddDays(Int32.Parse(_config[AccountConstant.PasswordPolicyPasDuration]));
                    user.Password = pass;
                    user.EmailConfirmed = true;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// function : Account update this.Account.Self (cập nhật thông tin của bản thân)
        /// CreatedBy: HaiHM
        /// CreatedDate: 24/04/2019
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdateUser(TblUsers user)
        {
            int update = 0;
            try
            {
                TblUsers userChk = db.TblUsers.Where(a => a.IsDelete == false && a.UserName == user.UserName && user.Id == user.Id).FirstOrDefault();
                // Check Email- email modified and not modify
                if (user.Email != userChk.Email)
                {
                    if (db.TblUsers.Where(u => u.IsDelete == false && user.Email == u.Email).Count() > 0)
                    {
                        // Duplicate Email
                        return update = AccountConstant.EditUserDuplicateEmail;
                    }
                }
                else if (user.PhoneNumber != userChk.PhoneNumber)  // Check PhoneNumber- PhoneNumber modified and not modify
                {
                    if (db.TblUsers.Where(u => u.IsDelete == false && user.PhoneNumber == u.PhoneNumber).Count() > 0)
                    {
                        // Duplicate PhoneNumber
                        return update = AccountConstant.EditUserDuplicatePhoneNumber;
                    }
                }
                if (userChk != null)
                {
                    userChk.FullName = user.FullName.TrimStart().TrimEnd();
                    userChk.Email = user.Email.Replace(" ", string.Empty);
                    userChk.PhoneNumber = user.PhoneNumber.Replace(" ", string.Empty);
                    userChk.UpdateBy = user.UserName;
                    userChk.UpdateDate = DateTime.Now;
                    userChk.Avatar = user.Avatar;
                    userChk.Address = user.Address;
                    userChk.BirthDay = user.BirthDay;
                    userChk.Gender = user.Gender;

                    db.Entry(userChk).State = EntityState.Modified;
                    db.SaveChanges();
                    update = AccountConstant.EditUserSuccess;
                }

                return update;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Function get all department of Organization
        /// CreatedBy: HaiHM
        /// CreatedDate: 23/04/2019
        /// </summary>
        /// <param name="OrganizationCode">OrganizationCode of Organization</param>
        /// <returns></returns>
        public object GetDepartment(string OrganizationCode)
        {
            try
            {
                List<List<dynamic>> obj = sp.GetDepartment(OrganizationCode);
                var response = new { data = obj[0] };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new { code = 400, data = ex.Message };
            }
        }

        #endregion

        #endregion
        #region Menu
        /// <summary>
        /// Function Get List Menu
        /// CreatedBy: System
        /// </summary>
        /// <returns></returns>
        public object GetMenuList(int userId)
        {
            try
            {
                List<tblMenuTree> lstMenuTree = GetMenuChild(userId, AccountConstant.MENU_PARENT_CODE);
                return lstMenuTree;
            }
            catch (Exception ex)
            {
                return new { code = 400, message = ex.Message };
            }
        }

        /// <summary>
        /// Funtion Generate JSON Web Token Reset password
        /// CreatedBy: HaiHM
        /// CreatedDate: 23/04/2019
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJSONWebTokenReset(TblUsers userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[AccountConstant.JwtKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //get department
            TblOrganization tblOrganization = GetOrganizationId(userInfo.Id);
            string orgCode = tblOrganization != null ? tblOrganization.OrganizationCode : "MP";

            var claims = new List<Claim>();
            claims.Add(new Claim(AccountConstant.sub, userInfo.UserName));
            claims.Add(new Claim(AccountConstant.userId, userInfo.Id.ToString()));
            claims.Add(new Claim(AccountConstant.orgCode, orgCode));
            claims.Add(new Claim(AccountConstant.ExpLogin, DateTime.Now.AddDays(AccountConstant.AddDaysLinkReset).ToString()));
            //Add Role
            string lstRole = GetRolePermission(userInfo.Id, AccountConstant.ADMIN_USER);
            if (!string.IsNullOrEmpty(lstRole))
            {
                string[] arrRole = lstRole.Split(AccountConstant.StringSlipSearch);
                if (arrRole.Count() > 1)
                {
                    for (int run = 0; run < arrRole.Count() - 1; run++)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, arrRole[run]));
                    }
                }
            }

            claims.Add(new Claim(ClaimTypes.NameIdentifier, userInfo.UserName));

            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: _config[AccountConstant.JwtIssuer],
                audience: _config[AccountConstant.JwtIssuer],
                //new Claim[] { claim, claim1, claimDepartment, claimExpLogin, claimRole },
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(AccountConstant.AddMinuteExpLogin)),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Function get MenuTree
        /// CreatedBy: System
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public List<tblMenuTree> GetMenuChild(int userId, string parentCode)
        {
            List<tblMenuTree> lstMenuTree = new List<tblMenuTree>();
            List<TblMenu> lstMenu = db.TblMenu.Where(a => a.ParentCode == parentCode).OrderBy(a => a.MenuIndex).ToList();
            if (lstMenu.Count > 0)
            {
                foreach (TblMenu item in lstMenu)
                {
                    bool checkHasParent = false;
                    List<List<dynamic>> obj = sp.JoinRolePermission(userId, item.MenuCode);
                    TblRoleCheckViewModel roleParent = obj[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                    tblMenuTree tblMenuTreeParent = new tblMenuTree();

                    if (Check(roleParent))
                    {
                        checkHasParent = true;
                        tblMenuTreeParent.menuId = item.Id;
                        tblMenuTreeParent.code = item.MenuCode;
                        tblMenuTreeParent.parentCode = item.ParentCode;
                        tblMenuTreeParent.state = item.MenuState;
                        tblMenuTreeParent.mainState = item.MenuMainState;
                        tblMenuTreeParent.name = item.MenuName;
                        tblMenuTreeParent.shortLabel = item.MenuShortLable;
                        tblMenuTreeParent.icon = item.MenuIcon;
                        tblMenuTreeParent.type = item.MenuType;
                        tblMenuTreeParent.target = item.MenuTarget;
                        tblMenuTreeParent.badge = item.MenuBadge;
                        tblMenuTreeParent.IsShowSidebar = item.IsShowSidebar;
                        
                        lstMenuTree.Add(tblMenuTreeParent);

                    }

                    List<TblMenu> lstMenuChil = db.TblMenu.Where(a => a.ParentCode == item.MenuCode).OrderBy(a => a.MenuIndex).ToList();
                    foreach (var itemChild in lstMenuChil)
                    {
                        List<List<dynamic>> objChild = sp.JoinRolePermission(userId, itemChild.MenuCode);
                        TblRoleCheckViewModel roleChild = objChild[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                        if (Check(roleChild) && (roleChild.IsShow.Value || roleChild.IsShowAll.Value))
                        {
                            if (!checkHasParent)
                            {
                                tblMenuTreeParent.menuId = item.Id;
                                tblMenuTreeParent.code = item.MenuCode;
                                tblMenuTreeParent.parentCode = item.ParentCode;
                                tblMenuTreeParent.state = item.MenuState;
                                tblMenuTreeParent.mainState = item.MenuMainState;
                                tblMenuTreeParent.name = item.MenuName;
                                tblMenuTreeParent.shortLabel = item.MenuShortLable;
                                tblMenuTreeParent.icon = item.MenuIcon;
                                tblMenuTreeParent.type = item.MenuType;
                                tblMenuTreeParent.target = item.MenuTarget;
                                tblMenuTreeParent.badge = item.MenuBadge;
                                tblMenuTreeParent.IsShowSidebar = item.IsShowSidebar;
                                lstMenuTree.Add(tblMenuTreeParent);
                                checkHasParent = true;
                            }

                            tblMenuTree tblMenuTree = new tblMenuTree();
                            tblMenuTree.menuId = itemChild.Id;
                            tblMenuTree.code = itemChild.MenuCode;
                            tblMenuTree.parentCode = item.MenuCode;
                            tblMenuTree.state = itemChild.MenuState;
                            tblMenuTree.mainState = item.MenuCode;
                            tblMenuTree.name = itemChild.MenuName;
                            tblMenuTree.shortLabel = itemChild.MenuShortLable;
                            tblMenuTree.icon = itemChild.MenuIcon;
                            tblMenuTree.type = itemChild.MenuType;
                            tblMenuTree.target = itemChild.MenuTarget;
                            tblMenuTree.badge = itemChild.MenuBadge;
                            tblMenuTree.IsShowSidebar = itemChild.IsShowSidebar;
                            tblMenuTreeParent.children.Add(tblMenuTree);
                        }
                    }
                }
            }
            return lstMenuTree;
        }

        private bool Check(TblRoleCheckViewModel item)
        {
            int countNull = 0;
            countNull = !item.IsEncypt.Value ? countNull : countNull + 1;
            countNull = !item.IsShow.Value ? countNull : countNull + 1;
            countNull = !item.IsShowAll.Value ? countNull : countNull + 1;
            countNull = !item.IsAdd.Value ? countNull : countNull + 1;
            countNull = !item.IsApprove.Value ? countNull : countNull + 1;
            countNull = !item.IsDelete.Value ? countNull : countNull + 1;
            countNull = !item.IsDeleteAll.Value ? countNull : countNull + 1;
            countNull = !item.IsEdit.Value ? countNull : countNull + 1;
            countNull = !item.IsEditAll.Value ? countNull : countNull + 1;
            countNull = !item.IsEnable.Value ? countNull : countNull + 1;
            countNull = !item.IsImport.Value ? countNull : countNull + 1;
            countNull = !item.IsExport.Value ? countNull : countNull + 1;
            countNull = !item.IsPrint.Value ? countNull : countNull + 1;
            countNull = !item.IsPermission.Value ? countNull : countNull + 1;
            countNull = !item.IsFirstExtend.Value ? countNull : countNull + 1;
            countNull = !item.IsSecondExtend.Value ? countNull : countNull + 1;
            countNull = !item.IsThirdExtend.Value ? countNull : countNull + 1;
            countNull = !item.IsFouthExtend.Value ? countNull : countNull + 1;

            if (countNull > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Function get Menu of RoleId
        /// CreatedBy: System
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public object GetTblMenuSPs(int roleId)
        {
            return db.GetMenuByRole(roleId);
        }
        #endregion

        /// <summary>
        /// Function get all Menu parent
        /// CreatedBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <returns></returns>
        public object GetTblMenuParent()
        {
            return sp.GetTblMenuParent();
        }

        /// <summary>
        /// Function check permission
        /// CreatedBy: HaiHM
        /// CreatedDate: 11/5/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        public string GetRolePermission(int userId, string MenuCode)
        {
            try
            {
                string role = "";
                List<List<dynamic>> obj = sp.JoinRolePermission(userId, MenuCode);
                TblRoleCheckViewModel roleChild = obj[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();

                // Add role = true in string (role)
                if (roleChild.IsEncypt.Value)
                {
                    role += AccountConstant.CanEncypt + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsShowAll.Value)
                {
                    role += AccountConstant.CanShowAll + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsShow.Value)
                {
                    role += AccountConstant.CanShow + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsAdd.Value)
                {
                    role += AccountConstant.CanAdd + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsEditAll.Value)
                {
                    role += AccountConstant.CanEditAll + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsEdit.Value)
                {
                    role += AccountConstant.CanEdit + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsDeleteAll.Value)
                {
                    role += AccountConstant.CanDeleteAll + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsDelete.Value)
                {
                    role += AccountConstant.CanDelete + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsImport.Value)
                {
                    role += AccountConstant.CanImport + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsExport.Value)
                {
                    role += AccountConstant.CanExport + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsPrint.Value)
                {
                    role += AccountConstant.CanPrint + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsApprove.Value)
                {
                    role += AccountConstant.CanApprove + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsEnable.Value)
                {
                    role += AccountConstant.CanEnable + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsPermission.Value)
                {
                    role += AccountConstant.CanPermission + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsFirstExtend.Value)
                {
                    role += AccountConstant.CanFirstExtend + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsSecondExtend.Value)
                {
                    role += AccountConstant.CanSecondExtend + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsThirdExtend.Value)
                {
                    role += AccountConstant.CanThirdExtend + AccountConstant.StringSlipSearch;
                }
                if (roleChild.IsFouthExtend.Value)
                {
                    role += AccountConstant.CanFouthExtend + AccountConstant.StringSlipSearch;
                }

                return role;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function check service pack of organization
        /// CreatedBy: HaiHM
        /// CreatedDate: 13/5/2019
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public bool CheckServicePack(TblOrganization org)
        {
            bool result = false;
            int countCurrAccount = 0;
            int? maxUser = 0;
            // list service pack of organization
            try
            {
                List<TblOrganizationServicePack> lst = db.TblOrganizationServicePack.Where(o => o.OrganizationId == org.OrganizationId).ToList();
                if (lst != null)
                {
                    foreach (var item in lst)
                    {
                        TblServicePack sp = db.TblServicePack.Where(s => s.Id == item.ServicePackId).FirstOrDefault();
                        if (sp != null)
                        {
                            maxUser += sp.MaxUser;
                        }
                    }
                    var lstUser = from a in db.TblOrganizationUser
                                  where a.OrganizationId == org.OrganizationId
                                  select a.UserId;

                    countCurrAccount = db.TblUsers.Where(u => lstUser.Contains(u.Id) && u.IsDelete == false && u.IsLock == false).Count(); // count number user active

                    if (countCurrAccount >= maxUser)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return result;
        }

        /// <summary>
        /// function get all permission
        /// HaiHM
        /// 13/05/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetAllPermission(int userId)
        {
            int countNull = 0;
            try
            {
                var lstAuthority = from a in db.TblAuthorityUser
                                   where a.UserId == userId
                                   select a.AuthorityId;
                if (lstAuthority != null)
                {
                    List<TblRole> lstRole = db.TblRole.Where(r => lstAuthority.Contains(r.AuthorityId)).ToList();
                    List<TblRole> lstRoleSelect = new List<TblRole>();
                    if (lstRole != null)
                    {
                        if (lstRole.Count > 0)
                        {
                            foreach (var item in lstRole)
                            {
                                countNull = 0;
                                countNull = item.IsShow == true ? countNull : countNull + 1;
                                countNull = item.IsShowAll == true ? countNull : countNull + 1;
                                countNull = item.IsAdd == true ? countNull : countNull + 1;
                                countNull = item.IsApprove == true ? countNull : countNull + 1;
                                countNull = item.IsDelete == true ? countNull : countNull + 1;
                                countNull = item.IsDeleteAll == true ? countNull : countNull + 1;
                                countNull = item.IsEdit == true ? countNull : countNull + 1;
                                countNull = item.IsEditAll == true ? countNull : countNull + 1;
                                countNull = item.IsEnable == true ? countNull : countNull + 1;
                                countNull = item.IsImport == true ? countNull : countNull + 1;
                                countNull = item.IsExport == true ? countNull : countNull + 1;
                                countNull = item.IsPrint == true ? countNull : countNull + 1;
                                countNull = item.IsPermission == true ? countNull : countNull + 1;
                                countNull = item.IsFirstExtend == true ? countNull : countNull + 1;
                                countNull = item.IsSecondExtend == true ? countNull : countNull + 1;
                                countNull = item.IsThirdExtend == true ? countNull : countNull + 1;
                                countNull = item.IsFouthExtend == true ? countNull : countNull + 1;

                                if (countNull != AccountConstant.NumberRoles)
                                    lstRoleSelect.Add(item);
                            }
                        }
                    }
                    return lstRoleSelect;
                }
            }
            catch (Exception ex)
            {
                return new { code = 400, message = ex.Message };
            }
            return null;
        }

        /// <summary>
        /// Function Check Role
        /// CreatedBy: HaiHM
        /// CreatedDate: 22/05/2019
        /// </summary>
        /// <param name="userIdLogin"></param>
        /// <param name="userIdChange"></param>
        /// <returns></returns>
        public bool CheckPermission(int userIdLogin, int userIdChange)
        {
            bool result = false;
            try
            {
                TblUsers userLogin = GetUserById(userIdLogin);
                TblUsers userChange = GetUserById(userIdChange);
                if (userLogin != null && userChange != null)
                {
                    if (userLogin.UserName == userChange.CreateBy)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }


        /// <summary>
        /// Function convert ImageToBase64
        /// CreateBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> ImageToBase64Async(string path)
        {
            string p = _environment.WebRootPath + path;
            string base64String = null;
            return await Task.Run(() =>
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(p);
                base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            });
        }

        /// <summary>
        /// Function get all infor ỏg
        /// CreatedBy: HaiHM
        /// CreatedDated: 25/5/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetInforOrganization(int userId)
        {
            try
            {
                TblOrganization orgResult = new TblOrganization();

                var lstOrg = from a in db.TblOrganization
                             where (from c in db.TblOrganizationUser where c.UserId == userId select c.OrganizationId).Contains(a.OrganizationId)
                             select a;
                List<TblOrganization> lstauT = lstOrg.ToList();
                if (lstauT.Count() > 0)
                {
                    TblOrganization org = lstauT.FirstOrDefault();
                    if (org != null)
                    {
                        orgResult = org;
                    }
                }

                var lstSP = from a in db.TblServicePack
                            where (from c in db.TblOrganizationServicePack where c.OrganizationId == orgResult.OrganizationId select c.ServicePackId).Contains(a.Id)
                            select a;
                List<TblServicePack> listServicePack = lstSP.ToList();

                var obj = new { Organization = orgResult, servicePack = listServicePack };

                return obj;
            }
            catch (Exception ex)
            {
                var obj = new { code = 400, message = ex.Message };
                return obj;
            }

        }

        /// <summary>
        /// Function update infor organization
        /// CreatedBy: HaiHM
        /// CreatedDate: 27/5/2019
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public int UpdateInforOrganization(TblOrganization organization, int userId, string username)
        {
            int updateOrg = AccountConstant.UpdateOrgFail;
            string email = "";
            string taxCode = "";
            string name = "";
            try
            {
                TblOrganization org = GetOrganizationId(userId);
                if (organization != null)
                {
                    if (!string.IsNullOrEmpty(organization.OrganizationEmail))
                    {
                        email = organization.OrganizationEmail.Trim().ToLower();
                    }
                    if (!string.IsNullOrEmpty(organization.OrganizationTaxCode))
                    {
                        taxCode = organization.OrganizationTaxCode.Trim();
                    }
                    if (!string.IsNullOrEmpty(organization.OrganizationName))
                    {
                        name = organization.OrganizationName.Trim();
                    }
                    if (organization.OrganizationId == org.OrganizationId)
                    {
                        //Check Email
                        if (org.OrganizationEmail.ToLower() != email)
                        {
                            if (db.TblOrganization.Where(u => u.IsDelete == false &&
                                         u.OrganizationEmail.Trim().ToLower() == email).Count() > 0)
                            {
                                // Duplicate Email
                                return AccountConstant.UpdateOrgDuplicateEmail;
                            }
                        }
                        // Check OrganizationTaxCode
                        if (org.OrganizationTaxCode.Trim().ToLower() != taxCode)
                        {
                            if (db.TblOrganization.Where(u => u.IsDelete == false &&
                                        organization.OrganizationTaxCode.Trim().ToLower() == u.OrganizationTaxCode.Trim().ToLower()).Count() > 0)
                            {
                                // Duplicate TaxCode
                                return AccountConstant.UpdateOrgDuplicateTaxCode;
                            }
                        }
                        if (!string.IsNullOrEmpty(organization.OrganizationName))
                        {
                            org.OrganizationName = name;
                        }
                        if (!string.IsNullOrEmpty(organization.OrganizationEmail))
                        {
                            org.OrganizationEmail = organization.OrganizationEmail.Trim().ToLower();
                        }
                        org.OrganizationPhone = organization.OrganizationPhone.Trim();
                        if (!string.IsNullOrEmpty(organization.OrganizationTaxCode))
                        {
                            org.OrganizationTaxCode = organization.OrganizationTaxCode.Trim();
                        }
                        org.OrganizationAddress = organization.OrganizationAddress;
                        org.OrganizationNote = organization.OrganizationNote;
                        org.OrganizationRemark = organization.OrganizationRemark;
                        org.OrganizationHomePage = organization.OrganizationHomePage;

                        org.OrganizationLogo = organization.OrganizationLogo;
                        org.UpdateBy = username;
                        org.UpdateDate = DateTime.Now;
                        db.Entry(org).State = EntityState.Modified;
                        db.SaveChanges();
                        return AccountConstant.UpdateOrgSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return updateOrg;
            }
            return updateOrg;
        }

        /// <summary>
        /// Function get list authority by userId
        /// CreatedBy: HaiHM
        /// CreatedDate: 30/5/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetAuthorityByUserId(int userId)
        {
            try
            {
                TblOrganization organization = GetOrganizationId(userId);
                List<List<dynamic>> obj = sp.GetAuthorityById(userId, organization.OrganizationId);
                var response = new { data = obj[0]};
                return response;
                //List By org
            }catch(Exception ex)
            {
                var obj = new { code = 400, message = ex.Message };
                return obj;
            }
        }
    }
}
