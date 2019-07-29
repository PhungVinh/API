using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.EmailService;
using AccountManagement.Models;
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
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AccountManagement.DataAccess
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
            string value = _distributedCache.GetString(cacheKey);
            return value;
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
            //_distributedCache.SetString(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(obj), options.SetSlidingExpiration(TimeSpan.FromMinutes(1)));
            _distributedCache.SetStringAsync(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(obj), options.SetSlidingExpiration(TimeSpan.FromMinutes(1)));

        }

        #region User

        /// <summary>
        /// Function check admin login at first time
        /// CreatedBy: HaiHM
        /// CreatedDate: 20/5/2019
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public bool CheckLoginAdminOrganization(string userName, string pw)
        {
            bool result = false;
            try
            {
                TblUsers user = db.TblUsers.Where(a => a.UserName.Trim().ToLower() == userName && a.EmailConfirmed == false).FirstOrDefault();
                if (user != null)
                {
                    //Check Admin Org
                    var authorityType = from a in db.TblAuthority
                                        where (from c in db.TblAuthorityUser
                                               where c.UserId == user.Id
                                                     && a.IsDelete == true
                                                     && a.IsLock == false
                                                     && a.AuthorityType == AccountConstant.Admin
                                               select c.AuthorityId).Contains(a.AuthorityId)
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
                                DateTime now = DateTime.Now;
                                if (now > checkDate && !user.EmailConfirmed.Value)
                                {
                                    using (var ts = new TransactionScope())
                                    {
                                        user.IsLock = true;
                                        db.Entry(user).State = EntityState.Modified;
                                        db.SaveChanges();

                                        // Lock and delete org
                                        //TblOrganization tblOrganization = new TblOrganization();

                                        //TblOrganizationUser tblOrganizationUser = db.TblOrganizationUser.Where(a => a.UserId == user.Id).FirstOrDefault();
                                        //if (tblOrganizationUser != null)
                                        //{
                                        //    tblOrganization = db.TblOrganization.Where(a => a.OrganizationId == tblOrganizationUser.OrganizationId).FirstOrDefault();

                                        //    tblOrganization.IsActive = false;
                                        //    tblOrganization.IsLock = true;
                                        //    tblOrganization.IsDelete = true;
                                        //    db.Entry(tblOrganization).State = EntityState.Modified;
                                        //    db.SaveChanges();
                                        //}

                                        result = true;
                                        ts.Complete();
                                    }
                                }
                            }
                        }
                    }
                    //End Check Admin Org
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

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
            bool checkSuperAdmin = false;
            int id = 0;
            string authorityName = "";
            try
            {
                TblUsers user = db.TblUsers.Where(a => a.UserName.Replace(" ", string.Empty).ToLower() == userName && a.IsDelete == false).FirstOrDefault();
                if (user != null)
                {
                    if (user.IsLock == true)
                        return user;
                    TblUsers userFind = db.TblUsers.Where(a => a.IsDelete == false && a.UserName.ToLower() == userName.ToLower() && accountCommon.ValidatePassword(pw, a.Password)).FirstOrDefault();
                    if (userFind == null)
                    {
                        var authorityId = db.TblAuthorityUser.Where(x => x.UserId == user.Id).FirstOrDefault();
                        if(authorityId != null)
                        {
                            id = authorityId.AuthorityId.Value;
                            authorityName = db.TblAuthority.Where(x => x.AuthorityId == id).FirstOrDefault().AuthorityName;
                        }
                        if (authorityName.Equals(AccountConstant.SuperAdmin))
                        {
                            checkSuperAdmin = true;
                        }
                        //LoginFail += 1;
                        int count = Int32.Parse(_config[AccountConstant.CountLoginFail]);
                        user.LoginFail += 1;
                        if (user.LoginFail > count && checkSuperAdmin == false)
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
        /// Function Get User by Username_ 
        /// Mặc định các giá trị sẽ hiển thị ở dạng rõ kể cả có cấu hình mã hóa hay không
        /// CreatedBy: System
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public TblUsers GetUsersCustom(string userName, string orgCode, int userIdLogin)
        {
            string keyEncrypt = "";
            TblCategory category = new TblCategory();
            try
            {
                TblUsers result = new TblUsers();
                TblUsers tblUsers = db.TblUsers.Where(a => a.IsDelete == false
                    && a.UserName == userName).FirstOrDefault();
                TblOrganization org = GetOrganizationId(tblUsers.Id);
                tblUsers.CategoryCodeRole = org.OrganizationName;
                //TblOrganization department = db.TblOrganization.Where(o => o.IsDelete == false && o.OrganizationCode != null && o.OrganizationCode == tblUsers.CategoryCodeDepartment).FirstOrDefault();
                //if (department != null)
                //{
                //    tblUsers.CategoryCodeDepartment = department.OrganizationName;
                //}
                //Position
                if (!string.IsNullOrEmpty(tblUsers.Position))
                {
                    category = GetAllCategory(AccountConstant.Position, orgCode, tblUsers.Position).FirstOrDefault();
                    if (category != null)
                    {
                        tblUsers.Position = category.CategoryName;
                    }
                }

                //Phòng ban
                if (!string.IsNullOrEmpty(tblUsers.CategoryCodeDepartment))
                {
                    category = GetAllCategory(AccountConstant.Department, orgCode, tblUsers.CategoryCodeDepartment).FirstOrDefault();
                    if (category != null)
                    {
                        tblUsers.CategoryCodeDepartment = category.CategoryName;
                    }
                }
                if (tblUsers != null)
                {
                    result = new TblUsers();
                    result.Id = tblUsers.Id;
                    result.UserName = tblUsers.UserName;
                    TblOrganization organization = GetOrganizationId(userIdLogin);
                    if (organization != null)
                    {
                        // Lấy khóa để giả mã cho các đơn vị
                        keyEncrypt = accountCommon.GenerateEncryptionKey(organization.OrganizationCode);
                    }
                    if (ListFieldEncrypted(AccountConstant.FieldEncryptedFullName,
                                AccountConstant.MENU_ADMIN_USER, userIdLogin) != null)
                    {
                        result.FullName = accountCommon.DecryptStringAES(tblUsers.FullName, keyEncrypt);
                    }
                    else
                    {
                        result.FullName = tblUsers.FullName;
                    }
                    if (ListFieldEncrypted(AccountConstant.FieldEncryptedAddress,
                               AccountConstant.MENU_ADMIN_USER, userIdLogin) != null)
                    {
                        result.Address = accountCommon.DecryptStringAES(tblUsers.Address, keyEncrypt);
                    }
                    else
                    {
                        result.Address = tblUsers.Address;
                    }

                    result.Email = tblUsers.Email;
                    result.PhoneNumber = tblUsers.PhoneNumber;
                    result.CreateBy = tblUsers.CreateBy;
                    result.CreateDate = tblUsers.CreateDate;
                    result.UpdateBy = tblUsers.UpdateBy;
                    result.UpdateDate = tblUsers.UpdateDate;
                    result.IsLock = tblUsers.IsLock;
                    //var base64 = tblUsers.Avatar;
                    //if (File.Exists(_environment.WebRootPath + tblUsers.Avatar))
                    //{
                    // base64 = ImageToBase64(tblUsers.Avatar);
                    //}
                    if (string.IsNullOrEmpty(tblUsers.Avatar))
                    {
                        if (tblUsers.Gender == 1)
                        {
                            result.Avatar = AccountConstant.LinkDefaulfLogoBoy;
                        }
                        else
                        {
                            result.Avatar = AccountConstant.LinkDefaulfLogoGirl;
                        }
                    }
                    else
                    {
                        result.Avatar = tblUsers.Avatar;
                    }

                    result.LastLogin = tblUsers.LastLogin;
                    result.Position = tblUsers.Position == "0" ? null : tblUsers.Position; ;
                    result.Gender = tblUsers.Gender;
                    result.BirthDay = tblUsers.BirthDay;
                    result.CategoryCodeDepartment = tblUsers.CategoryCodeDepartment == "0" ? null : tblUsers.CategoryCodeDepartment;
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
        public int AddUser(UserAndOrgViewModel model, int userIdLogin)
        {
            int save = 0;
            string pass = "";
            string email = "";
            string phoneNumber = "";
            string fullname = "";
            string username = "";
            string address = "";

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
                fullname = model.tblUsers.FullName.Trim();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.Address))
            {
                address = model.tblUsers.Address.Trim();
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

                        // Mã hóa luôn nếu trong db đã cấu hình mã hóa
                        //TblOrganization organization = GetOrganizationId(userIdLogin);
                        //if (organization != null)
                        //{
                        //    // Lấy khóa để giả mã cho các đơn vị
                        //    keyEncrypt = accountCommon.GenerateEncryptionKey(organization.OrganizationCode);
                        //}
                        //if (ListFieldEncrypted(AccountConstant.FieldEncryptedFullName, AccountConstant.MENU_ADMIN_USER, userIdLogin) != null)
                        //{
                        //    userChk.FullName = accountCommon.EncryptStringAES(fullname, keyEncrypt);
                        //}
                        //if (ListFieldEncrypted(AccountConstant.FieldEncryptedAddress, AccountConstant.MENU_ADMIN_USER, userIdLogin) != null)
                        //{
                        //    userChk.Address = accountCommon.EncryptStringAES(address, keyEncrypt);
                        //}

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
                        if (model.tblUsers.IsLock.HasValue)
                        {
                            tblUsers.IsLock = model.tblUsers.IsLock;
                        }
                        else
                        {
                            tblUsers.IsLock = false;
                        }
                        tblUsers.Avatar = model.tblUsers.Avatar;
                        tblUsers.Address = address;
                        tblUsers.LastLogin = null;
                        tblUsers.Position = model.tblUsers.Position;
                        tblUsers.EmailConfirmed = false;
                        tblUsers.Gender = model.tblUsers.Gender;
                        tblUsers.BirthDay = model.tblUsers.BirthDay;
                        // Ex: 21232f297a57a5a743894a0e4a801fc3=
                        tblUsers.HistoryPassword = passHash + AccountConstant.PasswordPolicySplit;
                        // + 30 days
                        tblUsers.ExpirationDate = DateTime.Now.AddDays(Int32.Parse(_config[AccountConstant.PasswordPolicyExpDateCreated]));
                        tblUsers.CategoryCodeDepartment = model.tblUsers.CategoryCodeDepartment;
                        tblUsers.CategoryCodeRole = model.tblUsers.CategoryCodeRole;
                        // when create user: SET Default avarta follow gender
                        if (tblUsers.Gender == 1)
                        {
                            tblUsers.Avatar = AccountConstant.LinkDefaulfLogoBoy;
                        }
                        else
                        {
                            tblUsers.Avatar = AccountConstant.LinkDefaulfLogoGirl;
                        }

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
            mail.Subject = AccountConstant.SubjectSendMailCreated;
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
        public int EditUser(UserAndOrgViewModel model, int userIdLogin)
        {
            int edit = 0;
            string email = "";
            string phoneNumber = "";
            string fullname = "";
            string address = "";
            string keyEncrypt = "";

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
                fullname = model.tblUsers.FullName.Trim();
            }
            if (!string.IsNullOrEmpty(model.tblUsers.Address))
            {
                address = model.tblUsers.FullName.Trim();
            }
            try
            {
                TblUsers userChk = db.TblUsers.Where(a => a.Id == model.tblUsers.Id).FirstOrDefault();
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
                    TblOrganization organization = GetOrganizationId(userIdLogin);
                    if (organization != null)
                    {
                        // Lấy khóa để giả mã cho các đơn vị
                        keyEncrypt = accountCommon.GenerateEncryptionKey(organization.OrganizationCode);
                    }
                    //Nếu trường FullName không bị mã hóa thì sẽ lưu như bình thường
                    List<EncryptionViewModel> lstCheckFullName = ListFieldEncrypted(AccountConstant.FieldEncryptedFullName,
                            AccountConstant.MENU_ADMIN_USER, userIdLogin);
                    //=> Lưu ở dạng rõ
                    if (lstCheckFullName == null)
                    {
                        userChk.FullName = fullname;
                    }
                    // Có quyền xem mã hóa trường FullName trong database đang bị mã hóa
                    //=> Lưu ở dạng mã hóa
                    if (lstCheckFullName != null && CheckRoleDecrypt(userIdLogin,
                                                                        AccountConstant.FieldEncryptedFullName,
                                                                        AccountConstant.MENU_ADMIN_USER))
                    {
                        userChk.FullName = accountCommon.EncryptStringAES(fullname, keyEncrypt);
                    }

                    //Nếu trường Address không bị mã hóa thì sẽ lưu như bình thường
                    //=> Lưu ở dạng rõ
                    List<EncryptionViewModel> lstCheckAddress = ListFieldEncrypted(AccountConstant.FieldEncryptedAddress,
                            AccountConstant.MENU_ADMIN_USER, userIdLogin);
                    if (lstCheckAddress == null)
                    {
                        userChk.Address = model.tblUsers.Address;
                    }
                    // Có quyền xem mã hóa trường Address trong database đang bị mã hóa
                    //=> Lưu ở dạng mã hóa
                    if (lstCheckAddress != null && CheckRoleDecrypt(userIdLogin,
                                                                        AccountConstant.FieldEncryptedAddress,
                                                                        AccountConstant.MENU_ADMIN_USER))
                    {
                        userChk.Address = accountCommon.EncryptStringAES(fullname, keyEncrypt);
                    }

                    using (var ts = new TransactionScope())
                    {
                        userChk.Email = email;
                        userChk.PhoneNumber = phoneNumber;
                        userChk.UpdateBy = model.tblUsers.UpdateBy;
                        userChk.UpdateDate = DateTime.Now;
                        //userChk.IsDelete = model.tblUsers.IsDelete;
                        //userChk.IsLock = model.tblUsers.IsLock;
                        if (model.tblUsers.IsLock.HasValue)
                        {
                            userChk.IsLock = model.tblUsers.IsLock;
                        }
                        else
                        {
                            userChk.IsLock = false;
                        }
                        userChk.Avatar = model.tblUsers.Avatar;
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
                            else
                            {
                                List<TblAuthorityUser> deleteAU = db.TblAuthorityUser.Where(au => au.UserId == userChk.Id).ToList<TblAuthorityUser>();
                                if (deleteAU != null)
                                {
                                    foreach (var item in deleteAU)
                                    {
                                        db.TblAuthorityUser.Remove(item);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            List<TblAuthorityUser> deleteAU = db.TblAuthorityUser.Where(au => au.UserId == userChk.Id).ToList<TblAuthorityUser>();
                            if (deleteAU != null)
                            {
                                foreach (var item in deleteAU)
                                {
                                    db.TblAuthorityUser.Remove(item);
                                    db.SaveChanges();
                                }
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
        public int DeleteUser(int userID, string username)
        {
            try
            {
                TblUsers userChk = db.TblUsers.Where(u => u.IsDelete == false && u.Id == userID).FirstOrDefault();
                if (userChk != null)
                {
                    userChk.UpdateBy = username;
                    userChk.UpdateDate = DateTime.Now;
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
        /// ModifiedDate: 10-06-2019
        /// ModifiedBody: Rút gọn link reset
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
                    // Active admin
                    var authorityType = from a in db.TblAuthority
                                        where (from c in db.TblAuthorityUser
                                               where c.UserId == user.Id
                                                     && a.IsDelete == true
                                                     && a.AuthorityType == AccountConstant.Admin
                                               select c.AuthorityId).Contains(a.AuthorityId)
                                        select a;
                    List<TblAuthority> lstauT = authorityType.ToList();
                    if (lstauT.Count() > 0)
                    {
                        TblAuthority auT = lstauT.FirstOrDefault();
                        if (auT.AuthorityType != null)
                        {
                            if (auT.AuthorityType == AccountConstant.Admin)
                            {
                                userChk.IsLock = false;
                            }
                        }
                    }
                    // Gen code reset
                    string code = accountCommon.CreateRandomStringNotSpecial();
                    userChk.CodeReset = accountCommon.HashPassword(user.UserName + code);
                    userChk.DateUpdatePassword = DateTime.Now.AddDays(AccountConstant.AddDaysLinkReset);
                    db.Entry(userChk).State = EntityState.Modified;
                    db.SaveChanges();
                    // gentoken
                    //string token = GenerateJSONWebTokenReset(userChk);
                    string token = AccountConstant.username + AccountConstant.PasswordPolicySplit + user.UserName
                                  + AccountConstant.And
                                  + AccountConstant.codeReset + AccountConstant.PasswordPolicySplit + code;


                    // send mail
                    SendMailReset(userChk.Email, userChk, AccountConstant.ApiLinkReset + token); // Chua co link
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
        /// Function Check Token Reset
        /// CreateBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// ModifiedDate: 10/6/2019
        /// ModifiedBody: Rút gọn link reset
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckTokenReset(string username, string codeReset)
        {
            try
            {
                TblUsers userCheck = GetUsers(username);

                if (userCheck != null && !string.IsNullOrEmpty(userCheck.CodeReset))
                {
                    if (accountCommon.ValidatePassword(username + codeReset, userCheck.CodeReset) && userCheck.DateUpdatePassword >= DateTime.Now)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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
        public async Task<object> SearchUser(int userId, string strFilter)
        {
            bool checkRole = false;
            string keyEncrypt = "";
            List<EncryptionViewModel> lstField = ListFieldEncrypted(AccountConstant.FieldMenuAdminUser, AccountConstant.MENU_ADMIN_USER, userId);
            try
            {
                List<List<dynamic>> obj = sp.SearchUser(strFilter);
                List<TblUserViewModel> lstUser = obj[0].OfType<TblUserViewModel>().ToList();
                if (lstUser != null)
                {
                    // kiểm tra xem có được hiển thị dữ liệu mã hóa hay không và có trường nào mã hóa nữa hay không
                    checkRole = CheckRoleDecrypt(userId, AccountConstant.FieldMenuAdminUser, AccountConstant.MENU_ADMIN_USER);
                    foreach (var item in lstUser)
                    {
                        //Nếu có được hiển thị dữ liệu mã hóa
                        if (checkRole)
                        {
                            // Không bị disable các trường mã hóa
                            lstField = null;
                            TblOrganization organization = GetOrganizationId(userId);
                            if (organization != null)
                            {
                                // Lấy khóa để giả mã cho các đơn vị
                                keyEncrypt = accountCommon.GenerateEncryptionKey(organization.OrganizationCode);
                            }
                            // duyệt từng field trong bảng mã hóa
                            foreach (var fieldEncrypted in ListFieldEncrypted(AccountConstant.FieldMenuAdminUser, AccountConstant.MENU_ADMIN_USER, userId))
                            {
                                // kiểm tra xem field nào cần được giải mã, duyệt lần lượt "FullName","Address"
                                switch (fieldEncrypted.Field)
                                {
                                    case AccountConstant.FieldEncryptedFullName:
                                        //giải mã FullName ở đây
                                        if (!string.IsNullOrEmpty(item.FullName))
                                        {
                                            if (item.CreateDate <= fieldEncrypted.UpdateDate)
                                            {
                                                item.FullName = accountCommon.DecryptStringAES(item.FullName, keyEncrypt);
                                            }
                                        }
                                        break;
                                    case AccountConstant.FieldEncryptedAddress:
                                        // giải mã Address ở đây
                                        if (!string.IsNullOrEmpty(item.Address))
                                        {
                                            if (item.CreateDate <= fieldEncrypted.UpdateDate)
                                            {
                                                item.Address = accountCommon.DecryptStringAES(item.Address, keyEncrypt);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        var base64 = item.Avatar;
                        if (File.Exists(_environment.WebRootPath + item.Avatar))
                        {
                            base64 = await ImageToBase64Async(item.Avatar);
                        }
                        item.Avatar = base64;
                    }
                    //foreach (var item in lstUser)
                    //{
                    //    // Có trường mã hóa và có quyền được giải mã

                    //    var base64 = item.Avatar;
                    //    if (File.Exists(_environment.WebRootPath + item.Avatar))
                    //    {
                    //        base64 = await ImageToBase64Async(item.Avatar);
                    //    }
                    //    item.Avatar = base64;
                    //}
                }
                var response = new { data = lstUser, paging = obj[1], fieldEncypt = CustomFieldEncypted(lstField) };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new { code = 400, data = ex.Message };
            }
        }

        private object CustomFieldEncypted(List<EncryptionViewModel> lstEncrypt)
        {
            if(lstEncrypt != null)
            {
                FieldAccountEncyptViewModel fevm = new FieldAccountEncyptViewModel();
                foreach (var item in lstEncrypt)
                {
                    if(item.Field == AccountConstant.FieldEncryptedFullName)
                    {
                        fevm.FullName = true;
                    }
                    if(item.Field == AccountConstant.FieldEncryptedAddress)
                    {
                        fevm.Address = true;
                    }
                }
                return fevm;
            }
            else
            {
                FieldAccountEncyptViewModel fevm = new FieldAccountEncyptViewModel();
                fevm.Address = false;
                fevm.FullName = false;
                return fevm;
            }

        }

        /// <summary>
        /// chức năng lấy danh sách chức vụ theo CategoryTypeCode 
        /// CreatedBy: HaiHM
        /// CreatedDate: 23/04/2019
        /// ModifiedDate: 14/6/2019
        /// ModifiedBody: lấy danh sách danh mục theo từng đơn vị đăng nhập
        /// </summary>
        /// <param name="CategoryTypeCode">Type get</param>
        /// <returns></returns>
        public List<TblCategory> GetAllCategory(string CategoryTypeCode, string orgCode, string CategoryCode)
        {
            List<TblCategory> lstCategory = new List<TblCategory>();
            List<TblCategory> lstCategoryByCategoryCode = new List<TblCategory>();
            bool checkValue = true;
            try
            {
                if(CategoryTypeCode.ToLower() == AccountConstant.Business.ToLower())
                {
                    lstCategory = db.TblCategory.Where(c => c.CategoryTypeCode.ToLower() == AccountConstant.Business.ToLower() && c.IsDelete == false && c.IsActive == true).ToList();
                    return lstCategory;
                }
                List<List<dynamic>> obj = sp.GetAllCategory(CategoryTypeCode, GetConnectionByOrgCode(orgCode));
                if(obj != null)
                {
                    if(obj[0] != null)
                    {
                        if(obj[0].Count() > 0)
                        {
                            lstCategory = obj[0].OfType<TblCategory>().ToList();
                            if (!string.IsNullOrEmpty(CategoryCode))
                            {
                                
                                foreach (var item in lstCategory)
                                {
                                    if(item.CategoryCode == CategoryCode)
                                    {
                                        checkValue = false;
                                        lstCategoryByCategoryCode.Add(item);
                                        return lstCategoryByCategoryCode;
                                    }
                                }
                                if (checkValue)
                                {
                                    return lstCategoryByCategoryCode;
                                }
                            }
                        }
                    }
                }
                //var response = new { data = obj[0] };
                return lstCategory;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
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
        public int UpdateUser(TblUsers user, bool checkUpdateImage, bool updatePass, bool checkResetSuccess, int userIdLogin)
        {
            int update = 0;
            string keyEncrypt = "";
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
                    //userChk.FullName = user.FullName;
                    //userChk.Address = user.Address;
                    TblOrganization organization = GetOrganizationId(user.Id);
                    if (organization != null)
                    {
                        // Lấy khóa để giả mã cho các đơn vị
                        keyEncrypt = accountCommon.GenerateEncryptionKey(organization.OrganizationCode);
                    }
                    //Nếu trường FullName không bị mã hóa thì sẽ lưu như bình thường
                    List<EncryptionViewModel> lstCheckFullName = ListFieldEncrypted(AccountConstant.FieldEncryptedFullName,
                            AccountConstant.MENU_ADMIN_USER, user.Id);
                    //=> Lưu ở dạng rõ
                    if (lstCheckFullName == null)
                    {
                        userChk.FullName = user.FullName;
                    }
                    // Có quyền xem mã hóa trường FullName trong database đang bị mã hóa
                    //=> Lưu ở dạng mã hóa
                    if (lstCheckFullName != null && CheckRoleDecrypt(user.Id,
                                                                        AccountConstant.FieldEncryptedFullName,
                                                                        AccountConstant.MENU_ADMIN_USER))
                    {
                        userChk.FullName = accountCommon.EncryptStringAES(user.FullName, keyEncrypt);
                    }

                    //Nếu trường Address không bị mã hóa thì sẽ lưu như bình thường
                    //=> Lưu ở dạng rõ
                    List<EncryptionViewModel> lstCheckAddress = ListFieldEncrypted(AccountConstant.FieldEncryptedAddress,
                            AccountConstant.MENU_ADMIN_USER, user.Id);
                    if (lstCheckAddress == null)
                    {
                        userChk.Address = user.Address;
                    }
                    // Có quyền xem mã hóa trường Address trong database đang bị mã hóa
                    //=> Lưu ở dạng mã hóa
                    if (lstCheckAddress != null && CheckRoleDecrypt(user.Id,
                                                                        AccountConstant.FieldEncryptedAddress,
                                                                        AccountConstant.MENU_ADMIN_USER))
                    {
                        userChk.Address = accountCommon.EncryptStringAES(user.Address, keyEncrypt);
                    }

                    userChk.Email = user.Email;
                    userChk.PhoneNumber = user.PhoneNumber;
                    userChk.UpdateBy = user.UserName;
                    userChk.UpdateDate = DateTime.Now;
                    if (updatePass)
                    {
                        userChk.Password = user.Password;
                        userChk.EmailConfirmed = user.EmailConfirmed;
                    }
                    if (checkResetSuccess)
                    {
                        user.DateUpdatePassword = user.DateUpdatePassword;
                        user.CodeReset = user.CodeReset;
                    }
                    if (checkUpdateImage)
                    {
                        userChk.Avatar = user.Avatar;
                    }
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
            List<tblMenuTree> lstMenuTree = GetMenuChild(userId, AccountConstant.MENU_PARENT_CODE);
            return lstMenuTree;
        }

        /// <summary>
        /// Funtion Generate JSON Web Token Reset password
        /// CreatedBy: HaiHM
        /// CreatedDate: 23/04/2019
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GenerateJSONWebTokenReset(TblUsers userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[AccountConstant.JwtKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //get department
            var claims = new List<Claim>();
            claims.Add(new Claim(AccountConstant.sub, userInfo.UserName));
            claims.Add(new Claim(AccountConstant.userId, userInfo.Id.ToString()));
            claims.Add(new Claim(AccountConstant.ExpLogin, DateTime.Now.AddDays(AccountConstant.AddDaysLinkReset).ToString()));

            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: _config[AccountConstant.JwtIssuer],
                audience: _config[AccountConstant.JwtIssuer],
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
            List<TblMenu> lstMenu = db.TblMenu.Where(a => a.ParentCode == parentCode && a.IsDelete == false && a.IsActive == true).OrderBy(a => a.MenuIndex).ToList();
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
                        tblMenuTreeParent.IsEncyption = item.IsEncyption;

                        lstMenuTree.Add(tblMenuTreeParent);

                    }

                    List<TblMenu> lstMenuChil = db.TblMenu.Where(a => a.ParentCode == item.MenuCode && a.IsDelete == false && a.IsActive == true).OrderBy(a => a.MenuIndex).ToList();
                    foreach (var itemChild in lstMenuChil)
                    {
                        List<List<dynamic>> objChild = sp.JoinRolePermission(userId, itemChild.MenuCode);
                        TblRoleCheckViewModel roleChild = objChild[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                        if (Check(roleChild) && (roleChild.IsShow.Value || roleChild.IsShowAll.Value || roleChild.IsAdd.Value))
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
                                tblMenuTreeParent.IsEncyption = item.IsEncyption;

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
                            tblMenuTreeParent.IsEncyption = item.IsEncyption;

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
        /// <param name="switchData"> switchData = true => Dùng cho màn hình cấu hình module</param>
        /// <returns></returns>
        public object GetTblMenuParent(bool switchData)
        {
            try
            {
                List<TblMenu> data = new List<TblMenu>();
                if (switchData)
                {
                    data = sp.GetTblMenuParent();
                }
                else
                {
                    data = db.TblMenu.Where(m => m.ParentCode == AccountConstant.MENU_PARENT_CODE && m.IsDelete == false && m.IsActive == true).ToList();
                }
                var obj = new object();
                List<Object> lstObject = new List<object>();
                foreach (var item in data)
                {
                    var tab = new object();
                    if (item.MenuCode == AccountConstant.MENU_CIMS_LIST)
                    {
                        tab = db.TblMenu.Where(m => m.MenuCode == AccountConstant.MENU_ATTR_FORM && m.IsDelete == false && m.IsActive == true).FirstOrDefault();
                        obj = new { data = item, tab = tab };
                        lstObject.Add(obj);
                    }
                    else if (item.MenuCode == "CIMS_DETAIL")
                    {
                        tab = db.TblMenu.Where(m => m.MenuCode == "ATTR_FORM_DETAIL").FirstOrDefault();
                        obj = new { data = item, tab = tab };
                        lstObject.Add(obj);
                    }
                    else
                    {
                        obj = new { data = item, tab = tab };
                        lstObject.Add(obj);
                    }

                }
                return lstObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Hàm lấy danh sách quyền
        /// @author: HaiHM
        /// @createdDate: 17/07/2019
        /// </summary>
        /// <returns></returns>
        private string StringRoles(TblRoleCheckViewModel role)
        {
            string result = "";
            if(role.MenuCode != AccountConstant.MENU_VOC)
            {
                role.MenuCode = "";
            }
            if(role != null)
            {
                // Add role = true in string (role)
                if (role.IsEncypt.Value)
                {
                    result += AccountConstant.CanEncypt + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsShowAll.Value)
                {
                    result += AccountConstant.CanShowAll + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsShow.Value)
                {
                    result += AccountConstant.CanShow + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsAdd.Value)
                {
                    result += AccountConstant.CanAdd + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsEditAll.Value)
                {
                    result += AccountConstant.CanEditAll + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsEdit.Value)
                {
                    result += AccountConstant.CanEdit + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsDeleteAll.Value)
                {
                    result += AccountConstant.CanDeleteAll + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsDelete.Value)
                {
                    result += AccountConstant.CanDelete + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsImport.Value)
                {
                    result += AccountConstant.CanImport + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsExport.Value)
                {
                    result += AccountConstant.CanExport + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsPrint.Value)
                {
                    result += AccountConstant.CanPrint + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsApprove.Value)
                {
                    result += AccountConstant.CanApprove + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsEnable.Value)
                {
                    result += AccountConstant.CanEnable + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsPermission.Value)
                {
                    result += AccountConstant.CanPermission + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsFirstExtend.Value)
                {
                    result += AccountConstant.CanFirstExtend + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsSecondExtend.Value)
                {
                    result += AccountConstant.CanSecondExtend + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsThirdExtend.Value)
                {
                    result += AccountConstant.CanThirdExtend + role.MenuCode + AccountConstant.StringSlipSearch;
                }
                if (role.IsFouthExtend.Value)
                {
                    result += AccountConstant.CanFouthExtend + role.MenuCode + AccountConstant.StringSlipSearch;
                }
            }

            return result;
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
                List<List<dynamic>> objVOC = sp.JoinRolePermission(userId, AccountConstant.MENU_VOC);
                TblRoleCheckViewModel roleChild = obj[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                TblRoleCheckViewModel roleChildVOC = objVOC[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                role += StringRoles(roleChild);
                role += StringRoles(roleChildVOC);

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
            List<TblRoleCheckViewModel> obj = new List<TblRoleCheckViewModel>();
            try
            {
                //start
                List<tblMenuTree> lstMenuTree = GetMenuChild(userId, AccountConstant.MENU_PARENT_CODE);
                if (lstMenuTree != null)
                {
                    foreach (var item in lstMenuTree)
                    {
                        var data = sp.JoinRolePermission(userId, item.code);
                        TblRoleCheckViewModel roleParent = data[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                        if (item.children.Count == 0)
                        {
                            obj.Add(roleParent);
                        }
                        if (item.children.Count > 0)
                        {
                            foreach (var itemChild in item.children)
                            {
                                var dataChild = sp.JoinRolePermission(userId, itemChild.code);
                                TblRoleCheckViewModel roleChild = dataChild[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
                                if (Check(roleChild))
                                {
                                    obj.Add(roleChild);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new { code = 400, message = ex.Message };
            }
            return obj;

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
        /// Function convert ImageToBase64 async
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
        /// Function convert ImageToBase64
        /// CreateBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ImageToBase64(string path)
        {
            string base64String = null;
            try
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                return base64String = Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function Get Image From Resource
        /// CreateBy: DaiBH
        /// CreatedDate: 04/06/2019
        /// </summary>
        /// <param name="organizationCode">organizationCode</param>
        /// <param name="imageFile">imageFile</param>
        /// <returns></returns>
        public byte[] GetImageAsync(string organizationCode, string imageFile)
        {
            string resourceDirectory = _environment.WebRootPath + Path.DirectorySeparatorChar + "Uploads" + Path.DirectorySeparatorChar;
            string imagePath = resourceDirectory + organizationCode + Path.DirectorySeparatorChar + imageFile;
            if (File.Exists(imagePath))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                return imageBytes;
            }
            throw new Exception("RESOURCE NOT FOUND");
        }

        /// <summary>
        /// Function get list authority by userId
        /// CreatedBy: HaiHM
        /// CreatedDate: 30/5/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetAuthorityByUserId(int userId, int organizationId)
        {
            try
            {
                List<List<dynamic>> obj = sp.GetAuthorityById(userId, organizationId);
                var response = new { data = obj[0] };
                return response;
                //List By org
            }
            catch (Exception ex)
            {
                var obj = new { code = 400, message = ex.Message };
                return obj;
            }
        }

        /// <summary>
        /// function kiểm tra xem có được giải mã hay không
        /// - đầu tiên phải kiểm tra có quyền xem (khi mã hóa hay ko)
        /// -> Nếu có quyền xem mã hóa => Kiểm tra xem trường đó đã mã hóa hay không
        ///     -> Nếu mã hóa rồi ==> giải mã trả lại
        /// </summary>
        /// <returns></returns>
        public bool CheckRoleDecrypt(int userId, string Field, string ParentCode)
        {
            bool result = false;
            // kiểm tra quyền
            var dataChild = sp.JoinRolePermission(userId, AccountConstant.MENU_ADMIN_USER);
            TblRoleCheckViewModel role = dataChild[0].OfType<TblRoleCheckViewModel>().FirstOrDefault();
            if (role != null)
            {
                // Có giá trị trường encypt trong bảng tblRole
                if (role.IsEncypt.HasValue)
                {
                    // có quyền xem dữ liệu mã hóa
                    if (role.IsEncypt.Value)
                    {
                        // đếm xem có trường mã hóa theo module (ParentCode) hay không
                        if (ListFieldEncrypted(Field, ParentCode, userId) != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Hàm lấy danh sách field đã được mã hóa theo module và field
        /// @author: HaiHM
        /// @created: 12/06/2019
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="ParentCode"></param>
        /// <returns></returns>
        private List<EncryptionViewModel> ListFieldEncrypted(string Field, string ParentCode, int userId)
        {
            List<EncryptionViewModel> lst = new List<EncryptionViewModel>();
            try
            {
                // lấy ra danh sách các field được mã hóa
                TblOrganization organization = GetOrganizationId(userId);
                if (organization != null)
                {
                    var data = sp.ListtFieldEncrypted(Field, ParentCode, GetConnectionByOrgCode(organization.OrganizationCode));
                    lst = data[0].OfType<EncryptionViewModel>().ToList<EncryptionViewModel>();
                    if (lst != null)
                    {
                        if (lst.Count() > 0)
                        {
                            return lst.Select(x => new EncryptionViewModel()
                            {
                                ParentCode = x.ParentCode,
                                Field = x.Field,
                                UpdateDate = x.UpdateDate
                            }).ToList();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return lst;
        }

        /// <summary>
        /// Function get connection của đơn vị
        /// HaiHM
        /// 11/06/2019
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        private string GetConnectionByOrgCode(string orgCode)
        {
            string connection = "";
            List<List<dynamic>> obj = sp.GetAllConnection();
            List<ConnectionStrings> lstUser = obj[0].OfType<ConnectionStrings>().ToList();
            if (lstUser.Count() > 0)
            {
                foreach (var item in lstUser)
                {
                    if (item.ConnectionKey.Contains(orgCode + AccountConstant.ConnectionKey))
                    {
                        connection = item.ConnectionValue;
                        break;
                    }
                }
            }
            return connection;
        }
    }
}
