using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using OrganizationManagement.Common;
using OrganizationManagement.Constant;
using OrganizationManagement.EmailService;
using OrganizationManagement.Models;
using OrganizationManagement.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace OrganizationManagement.DataAccess
{
    public class OrganizationDA : IOrganizationRepository
    {
        private OrganizationCommon organizationConmon = new OrganizationCommon();
        private SP_OrganizationDA orgSP = new SP_OrganizationDA();
        private IDistributedCache _distributedCache;
        private IEmailService _emailService;
        private EmailServiceEx _sendMail = new EmailServiceEx();
        //HaiHM add _environment
        private static IHostingEnvironment _environment;
        public OrganizationDA(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public void LoadDistributedCache(IDistributedCache distributedCache, IEmailService emailService)
        {
            _distributedCache = distributedCache;
            _emailService = emailService;
        }
        private OrganizationCommon common = new OrganizationCommon();

        CRM_MASTERContext db = new CRM_MASTERContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MASTERContext>());

        /// <summary>
        /// Phongtv 
        /// List goi dich vu
        /// </summary>
        /// <returns></returns>

        public List<TblServicePack> GetlistPack()
        {
            try
            {
                List<TblServicePack> lstPack = db.TblServicePack.Where(a => a.IsDelete.Value == false).ToList();
                return lstPack;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public object GetServicePack(int OrganizationId)
        {
            try
            {
                List<List<dynamic>> obj = orgSP.GetServicePack(OrganizationId);
                var response = new { data = obj[0] };
                return response;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        /// <summary>
        /// List Organization
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>

        public object GetOrganizationList(string strFilter)
        {
            try
            {
                List<List<dynamic>> obj = orgSP.GetOrg_Page(strFilter);
                //List<OrganizationSP> lstOrganization = new List<OrganizationSP>();
                //foreach (var item in obj[0])
                //{
                //    OrganizationSP organizationSP = new OrganizationSP();
                //    organizationSP = item as OrganizationSP;
                //    string urlImage = _environment.WebRootPath + OrganizationConstant.DirectoryUploads + organizationSP.OrganizationCode + "\\" + organizationSP.OrganizationCode + ".png";
                //    try
                //    {
                //        string imgBase64 = Convert.ToBase64String(File.ReadAllBytes(urlImage));
                //        organizationSP.OrganizationLogo = imgBase64;
                //    }
                //    catch (Exception ex)
                //    {
                //        organizationSP.OrganizationLogo = null;
                //    }


                //    lstOrganization.Add(organizationSP);
                //}
                var response = new
                {
                    data = obj[0],
                    paging = obj[1]
                };



                return response;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }





        /// <summary>
        /// Delete Organization
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <returns></returns>

        public int DeleteOrganization(int OrganizationId)
        {
            int result = 0;
            try
            {
                var lstOrgCheck = (from a in db.TblOrganizationUser
                                   join b in db.TblUsers on a.UserId equals b.Id
                                   where b.IsDelete == false && a.OrganizationId == OrganizationId
                                   select new
                                   {
                                       b.Id
                                   });

                if (lstOrgCheck.Count() > 1)
                {
                    result = 0;
                }
                else
                {
                    using (var ts = new TransactionScope())
                    {
                        TblOrganization OrgChk = db.TblOrganization.Where(a => a.OrganizationId == OrganizationId).FirstOrDefault();
                        if (OrgChk != null)
                        {
                            OrgChk.IsDelete = true;
                            db.Entry(OrgChk).State = EntityState.Modified;
                            db.SaveChanges();

                            TblConnectionConfig connect = db.TblConnectionConfig.Where(a => a.ConnectionKey == OrgChk.OrganizationCode + "Connection").FirstOrDefault();
                            if(connect != null)
                            {
                                db.TblConnectionConfig.Remove(connect);
                                db.SaveChanges();
                            }

                            string urlImage = _environment.WebRootPath + OrganizationConstant.DirectoryUploads + OrgChk.OrganizationCode + "\\" + OrgChk.OrganizationCode + ".png";
                            try
                            {
                                File.Delete(urlImage);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            DeleteUser(OrganizationId);

                            result = 1;
                        }
                        ts.Complete();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }
        /// <summary>
        ///  // Active Organization
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="IsCheckedOrg"></param>
        /// <returns></returns>

        public int ActiveOrganization(int OrganizationId, bool IsCheckedOrg)
        {
            int result = 0;
            try
            {
                TblOrganization OrgChk = db.TblOrganization.Where(a => a.OrganizationId == OrganizationId).FirstOrDefault();
                if (OrgChk != null)
                {
                    OrgChk.IsActive = IsCheckedOrg;
                    db.Entry(OrgChk).State = EntityState.Modified;
                    db.SaveChanges();
                    result = 1;
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }


        /// <summary>
        ///  Gui Mail
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public void SendMail(string name, string emailTo, string user, string pass, string link)
        {
            TblRecieveEmail mail = new TblRecieveEmail();
            mail.Subject = organizationConmon.subjectCreate();
            mail.To = emailTo;
            mail.EmailContents = organizationConmon.HtmlMailCreatedAccount(name, user, pass, link);
            _sendMail.SendEmail(mail);
        }

        /// <summary>
        /// add don vi
        /// </summary>
        /// <param name="Org"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public int AddOrganization(UpdateOrg model, string token)
        {
            int result = 0;
            string nameOrg = "";

            try
            {
                TblOrganization OrgChk = db.TblOrganization.Where(r => (r.OrganizationTaxCode == model.Org.OrganizationTaxCode && r.IsDelete == false) || (r.OrganizationCode == model.Org.OrganizationCode && r.IsDelete == false) || (r.OrganizationEmail == model.Org.OrganizationEmail && r.IsDelete == false)).FirstOrDefault();
                if (OrgChk == null)
                {
                    // Send mail
                    string pass = "";
                    string UserName = "";
                    //HaiHM comment Line under "Pass"
                    //string Pass = User.Password;
                    string UserEmail = "";
                    using (var ts = new TransactionScope())
                    {
                        TblOrganization OrgAdd = new TblOrganization();
                        OrgAdd.IsDelete = false;
                        OrgAdd.CreateDate = DateTime.Now;
                        OrgAdd.CreateBy = model.Org.CreateBy;
                        OrgAdd.IsActive = model.Org.IsActive;
                        OrgAdd.IsLock = model.Org.IsLock;
                        OrgAdd.UpdateBy = model.Org.UpdateBy;
                        OrgAdd.UpdateDate = DateTime.Now;
                        OrgAdd.OrganizationAddress = model.Org.OrganizationAddress;
                        OrgAdd.OrganizationCode = model.Org.OrganizationCode.TrimEnd().TrimStart();
                        OrgAdd.OrganizationEmail = model.Org.OrganizationEmail.TrimEnd().TrimStart();
                        OrgAdd.OrganizationFrom = model.Org.OrganizationFrom;
                        OrgAdd.OrganizationHomePage = model.Org.OrganizationHomePage;
                        OrgAdd.OrganizationName = model.Org.OrganizationName.TrimEnd().TrimStart();
                        OrgAdd.OrganizationParentCode = model.Org.OrganizationParentCode;
                        OrgAdd.OrganizationPhone = model.Org.OrganizationPhone;
                        OrgAdd.OrganizationRemark = model.Org.OrganizationRemark;
                        OrgAdd.OrganizationTaxCode = model.Org.OrganizationTaxCode.TrimEnd().TrimStart();
                        OrgAdd.OrganizationTo = model.Org.OrganizationTo;
                        OrgAdd.OrganizationLogo = model.Org.OrganizationLogo;
                        OrgAdd.OrganizationNote = model.Org.OrganizationNote;
                        OrgAdd.OrganizationSphereId = model.Org.OrganizationSphereId;
                        db.TblOrganization.Add(OrgAdd);
                        db.SaveChanges();

                        nameOrg = OrgAdd.OrganizationName;

                        // ADD admin
                        TblUsers User = new TblUsers();
                        List<TblUsers> lst = db.TblUsers.Where(u => u.UserName == model.Org.OrganizationCode + "_Admin").ToList();
                        if(lst != null)
                        {
                            if(lst.Count > 0)
                            {
                                foreach (var item in lst)
                                {
                                    TblUsers users = db.TblUsers.Where(u => u.Id == item.Id).FirstOrDefault();
                                    if(users != null)
                                    {
                                        users.IsDelete = true;
                                        db.Entry(users).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        User.UserName = model.Org.OrganizationCode + "_Admin";
                        //update isdelete
                        pass = "123@123a";
                       
                        User.Password = organizationConmon.HashPassword(pass);
                        User.CreateDate = DateTime.Now;
                        User.IsDelete = false;
                        User.IsLock = false;
                        User.CreateBy = model.Org.CreateBy;
                        User.Email = model.Org.OrganizationEmail;
                        User.EmailConfirmed = false;
                        User.HistoryPassword = organizationConmon.HashPassword(pass) + "=";
                        User.LoginFail = 0;
                        User.LastLogin = DateTime.Now;
                        User.ExpirationDate = DateTime.Now.AddDays(OrganizationConstant.ExpirationDate);
                        db.TblUsers.Add(User);
                        db.SaveChanges();
                        int UserID = User.Id;
                        int OrganizationId = OrgAdd.OrganizationId;

                        // Send mail
                        UserName = User.UserName;
                        //HaiHM comment Line under "Pass"
                        //string Pass = User.Password;
                        UserEmail = User.Email;

                        TblOrganizationUser OrgUser = new TblOrganizationUser
                        {
                            UserId = UserID,
                            OrganizationId = OrganizationId
                        };
                        db.TblOrganizationUser.Add(OrgUser);
                        db.SaveChanges();

                        //ADD goi dich vu

                        foreach (var item in model.tblServicePack)
                        {
                            TblOrganizationServicePack ru = new TblOrganizationServicePack();
                            ru.OrganizationId = OrganizationId;
                            ru.ServicePackId = item.Id; // id new created
                            db.TblOrganizationServicePack.Add(ru);
                            db.SaveChanges();
                        }



                        //HaiHM modified
                        AddAuthorityAndRoleForUserAdmin(OrganizationId, model.Org.CreateBy, UserID);

                        ts.Complete();
                    }
                    if (!string.IsNullOrEmpty(UserEmail))
                    {
                        SendMail(nameOrg, UserEmail, UserName, pass, "");
                        result = 1;
                        GenerateDB(model.Org.OrganizationCode, token);
                    }
                }
                else
                {
                    if (db.TblOrganization.Where(u => u.IsDelete == false && u.OrganizationCode == model.Org.OrganizationCode).Count() > 0)
                    {
                        // Duplicate AddDuplicateOrgCode
                        return result = 411;
                    }

                    if (db.TblOrganization.Where(u => u.IsDelete == false && u.OrganizationEmail == model.Org.OrganizationEmail).Count() > 0)
                    {
                        // Duplicate AddDuplicateEmail
                        return result = 412;
                    }

                    if (db.TblOrganization.Where(u => u.IsDelete == false && u.OrganizationTaxCode == model.Org.OrganizationTaxCode).Count() > 0)
                    {
                        // Duplicate AddDuplicateOrgTaxCode
                        return result = 413;
                    }


                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// Funtion get list Org
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/3/2019
        /// </summary>
        /// <returns></returns>
        public object GetListOrg()
        {
            try
            {


                var lstOrg = (from a in db.TblOrganization

                              where a.IsDelete == false && a.IsActive == true
                              select new
                              {
                                  a.OrganizationId,
                                  a.OrganizationCode,
                                  a.OrganizationName
                              });
                return lstOrg;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function set isDelete of user = true when delete Organization
        /// CreatedBy: HaiHM
        /// CreatedDate: 2019/6/3
        /// </summary>
        /// <param name="authoritys"></param>
        /// <param name="userId"></param>
        private void DeleteUser(int organizationId)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    // Update tblUsers
                    List<TblOrganizationUser> data = db.TblOrganizationUser.Where(x => x.OrganizationId == organizationId).ToList();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            TblUsers user = db.TblUsers.Where(u => u.IsDelete == false && u.Id == item.UserId).FirstOrDefault();
                            if (user != null)
                            {
                                user.IsDelete = true;
                                db.Entry(user).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Function using for add user in authority
        /// CreatedBy: HaiHM
        /// CreatedDate: 21/5/2019
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="createBy"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private void AddAuthorityAndRoleForUserAdmin(int OrganizationId, string createBy, int userId)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    // Add Authority Admin
                    int idAuthority = 0;
                    TblAuthority authority = new TblAuthority()
                    {
                        AuthorityDescription = OrganizationConstant.AuthorityDescription,
                        AuthorityName = OrganizationConstant.AuthorityName,
                        AuthorityType = OrganizationConstant.AuthorityType,
                        OrganizationId = OrganizationId,
                        CreateBy = createBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = createBy,
                        UpdateDate = DateTime.Now,
                        IsDelete = true,
                        IsLock = false
                    };

                    db.TblAuthority.Add(authority);
                    db.SaveChanges();

                    idAuthority = authority.AuthorityId;
                    //Add user In Authority Admin
                    if (idAuthority > 0)
                    {
                        TblAuthorityUser au = new TblAuthorityUser()
                        {
                            AuthorityId = idAuthority,
                            UserId = userId
                        };
                        db.TblAuthorityUser.Add(au);
                        db.SaveChanges();
                        // Add Role for user admin
                        AddRoleForUser(idAuthority);
                    }

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Function Add Role for User Admin
        /// CreatedBy: HaiHM
        /// CreatedDate: 22/5/2019
        /// </summary>
        /// <param name="authorityId"></param>
        private void AddRoleForUser(int authorityId)
        {
            try
            {
                //List<TblMenu> lstMenu = orgSP.LoadMenuWhenAddOrg();
                List<TblMenu> lstMenu = db.TblMenu.Where(m => m.MenuCode != OrganizationConstant.ADMIN_DEPART && m.IsDelete == false).ToList();
                foreach (var item in lstMenu)
                {
                    TblRole role = new TblRole();
                    role.AuthorityId = authorityId;
                    role.MenuCode = item.MenuCode;
                    role.IsEncypt = true;
                    role.IsShowAll = true;
                    role.IsShow = true;
                    role.IsAdd = true;
                    role.IsEditAll = true;
                    role.IsEdit = true;
                    role.IsDeleteAll = true;
                    role.IsDelete = true;
                    role.IsImport = true;
                    role.IsExport = true;
                    role.IsPrint = true;
                    role.IsApprove = true;
                    role.IsEnable = true;
                    role.IsPermission = true;
                    role.IsFirstExtend = true;
                    role.IsSecondExtend = true;
                    role.IsThirdExtend = true;
                    role.IsFouthExtend = true;
                    db.TblRole.Add(role);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Function GenerateDB
        /// CreatedBy: HiepPD1
        /// CreatedDate: 06/05/2019
        /// </summary>
        /// <param name="orgCode,token"></param>
        public void GenerateDB(string orgCode, string token)
        {
            // Create database 
            string script = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OrganizationConstant.DATABASE_SCRIPT_FILE));
            string dbName = OrganizationConstant.DATABASE_PREFIX + orgCode;
            string sqlCreateDB = OrganizationConstant.DATABASE_CHECK_CREATE + "'" + dbName + "'" + ")) ";
            sqlCreateDB += OrganizationConstant.DATABASE_DROP + dbName + " " + OrganizationConstant.DATABASE_CREATE + dbName + "";
            orgSP.ExecuteNonQuerySql(sqlCreateDB);
            script = script.Replace(OrganizationConstant.DATABASE_GO, OrganizationConstant.SPLIT_DATABASE);
            string[] arr = script.Split(OrganizationConstant.SPLIT_DATABASE);
            string connectionKey = orgCode + OrganizationConstant.CONNECTION_CONFIG;
            string connectionValue = OrganizationConstant.SQL_CONNECTION.Replace(OrganizationConstant.DATABASE_MASTER, dbName);
            AddConnectionConfig(connectionKey, connectionValue);

            for (int i = 0; i < arr.Length; i++)
            {
                string sqlQuery = "";
                if (i == 0)
                {
                    sqlQuery = OrganizationConstant.DATABASE_USE + dbName;
                    sqlQuery += arr[i];
                    orgSP.ExecuteNonQuerySql(sqlQuery);
                }
                else
                {
                    sqlQuery += arr[i];
                    orgSP.ExecuteNonQueryStoreProcedure(sqlQuery, dbName);
                }
            }
            //Add connection config
          
            //Set contextfactory
            //var objConn = new
            //{
            //    ConnectionKey = connectionKey,
            //    ConnectionValue = connectionValue
            //};
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(OrganizationConstant.BODY_TYPE));
            //client.DefaultRequestHeaders.Add(OrganizationConstant.AUTHORIZATION, token);
            //var response = client.PostAsJsonAsync(CommonFunction.API_URL + OrganizationConstant.API_CONTEXT, objConn).Result;
        }
        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Sua Don Vi
        /// </summary>
        /// <param name="Org"></param>
        /// <returns></returns>
        public int UpdateOrganization(UpdateOrg model)
        {
            int result = 0;
            try
            {
                TblOrganization OrgChk = db.TblOrganization.Where(r => r.OrganizationId == model.Org.OrganizationId && r.IsDelete == false).FirstOrDefault();


                using (var ts = new TransactionScope())
                {


                    if (OrgChk != null)
                    {
                        if (OrgChk.OrganizationCode != model.Org.OrganizationCode)
                        {

                            if (db.TblOrganization.Where(u => u.IsDelete == false && u.OrganizationCode == model.Org.OrganizationCode).Count() > 0)
                            {
                                // Duplicate DuplicateOrgCode
                                return result = 411;
                            }


                        }

                        if (OrgChk.OrganizationEmail != model.Org.OrganizationEmail)
                        {
                            if (db.TblOrganization.Where(u => u.IsDelete == false && u.OrganizationEmail == model.Org.OrganizationEmail).Count() > 0)
                            {
                                // Duplicate DuplicateEmail
                                return result = 412;
                            }
                        }

                        if (OrgChk.OrganizationTaxCode != model.Org.OrganizationTaxCode)
                        {
                            if (db.TblOrganization.Where(u => u.IsDelete == false && u.OrganizationTaxCode == model.Org.OrganizationTaxCode).Count() > 0)
                            {
                                // Duplicate DuplicateOrgTaxCode
                                return result = 413;
                            }
                        }


                        OrgChk.CreateBy = model.Org.CreateBy;
                        OrgChk.IsDelete = model.Org.IsDelete;
                        OrgChk.CreateDate = model.Org.CreateDate;
                        OrgChk.IsActive = model.Org.IsActive;
                        OrgChk.IsLock = model.Org.IsLock;
                        OrgChk.UpdateBy = model.Org.UpdateBy;
                        OrgChk.UpdateDate = DateTime.Now;
                        OrgChk.OrganizationAddress = model.Org.OrganizationAddress;
                        OrgChk.OrganizationCode = model.Org.OrganizationCode.TrimEnd().TrimStart();
                        OrgChk.OrganizationEmail = model.Org.OrganizationEmail.TrimEnd().TrimStart();
                        OrgChk.OrganizationFrom = model.Org.OrganizationFrom;
                        OrgChk.OrganizationHomePage = model.Org.OrganizationHomePage;
                        OrgChk.OrganizationName = model.Org.OrganizationName.TrimEnd().TrimStart();
                        OrgChk.OrganizationParentCode = model.Org.OrganizationParentCode;
                        OrgChk.OrganizationPhone = model.Org.OrganizationPhone;
                        OrgChk.OrganizationRemark = model.Org.OrganizationRemark;
                        OrgChk.OrganizationTaxCode = model.Org.OrganizationTaxCode.TrimEnd().TrimStart();
                        OrgChk.OrganizationTo = model.Org.OrganizationTo;
                        OrgChk.OrganizationLogo = model.Org.OrganizationLogo != null ? model.Org.OrganizationLogo : OrgChk.OrganizationLogo;
                        OrgChk.OrganizationNote = model.Org.OrganizationNote;
                        OrgChk.OrganizationSphereId = model.Org.OrganizationSphereId;
                        db.Entry(OrgChk).State = EntityState.Modified;
                        db.SaveChanges();

                        // Add goi dich vu

                        List<TblOrganizationServicePack> deleteAU = db.TblOrganizationServicePack.Where(au => au.OrganizationId == model.Org.OrganizationId).ToList<TblOrganizationServicePack>();
                        foreach (var item in deleteAU)
                        {
                            db.TblOrganizationServicePack.Remove(item);
                            db.SaveChanges();
                        }

                    }


                    foreach (var item in model.tblServicePack)
                    {
                        TblOrganizationServicePack ru = new TblOrganizationServicePack();
                        ru.OrganizationId = model.Org.OrganizationId;
                        ru.ServicePackId = item.Id; // id new created
                        db.TblOrganizationServicePack.Add(ru);
                        db.SaveChanges();
                    }


                    result = 2;
                    ts.Complete();
                }


                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// 
        // Delete User To Org
        /// </summary>
        /// <param name="OrgUser"></param>
        /// <returns></returns>
        public int DeleteUserOrg(TblOrganizationUser OrgUser)
        {
            int result = 0;
            try
            {
                TblOrganizationUser OrgChk = db.TblOrganizationUser.Where(r => r.OrganizationId == OrgUser.OrganizationId && r.UserId == OrgUser.UserId).FirstOrDefault();
                if (OrgChk != null)
                {
                    db.TblOrganizationUser.Remove(OrgChk);
                    db.SaveChanges();
                    result = 1;
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }

        /// <summary>
        /// HaiHM
        /// Get TblOrganization By OrganizationCode
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public TblOrganization GetOrganization(string orgCode)
        {
            try
            {
                TblOrganization org = db.TblOrganization.Where(o => String.Compare(o.OrganizationCode, orgCode, false) == 0).FirstOrDefault();
                return org;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        public int UpdateOrganizationLogo(TblOrganization Organization)
        {

            try
            {

                db.Entry(Organization).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }

        }

        #region Cache
        public string GetStringCache(string cacheKey)
        {
            return _distributedCache.GetString(cacheKey);
        }

        public void SetStringCache(string cacheKey, Object obj)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            _distributedCache.SetString(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(obj), options.SetSlidingExpiration(TimeSpan.FromMinutes(1)));

        }
        #endregion
        #region Connection Config
        /// <summary>
        /// HiepPD1
        /// Get all connection
        /// </summary>
        /// <returns></returns>
        public List<TblConnectionConfig> GetAllConnection()
        {
            List<TblConnectionConfig> lstConn = db.TblConnectionConfig.ToList();
            return lstConn;
        }
        /// <summary>
        /// HiepPD1
        /// Add new config connection 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddConnectionConfig(string key, string value)
        {
            TblConnectionConfig tblConnectionConfig = new TblConnectionConfig();
            tblConnectionConfig.ConnectionKey = key;
            tblConnectionConfig.ConnectionValue = value;
            db.TblConnectionConfig.Add(tblConnectionConfig);
            db.SaveChanges();
        }
        #endregion

        #region HaiHM get infor and update Organization

        /// <summary>
        /// Function convert ImageToBase64
        /// CreateBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> ImageToBase64Async(string path)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Function get all infor ỏg
        /// CreatedBy: HaiHM
        /// CreatedDated: 25/5/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<object> GetInforOrganization(int userId)
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
                        string str = OrganizationConstant.DirectoryUploads + org.OrganizationCode + OrganizationConstant.DirectorySlat + org.OrganizationCode + ".png";
                        if (File.Exists(_environment.WebRootPath + str))
                        {
                            org.OrganizationLogo = await ImageToBase64Async(str);
                        }
                        else
                        {
                            string defaultLogo = OrganizationConstant.DirectoryUploads + OrganizationConstant.LinkDefaulfLogo;
                            if (File.Exists(_environment.WebRootPath + defaultLogo))
                            {
                                org.OrganizationLogo = await ImageToBase64Async(defaultLogo);
                            }
                            else
                            {
                                org.OrganizationLogo = await ImageToBase64Async(OrganizationConstant.DirectoryUploads + OrganizationConstant.DirectorySlat + OrganizationConstant.LinkDefaulfLogo1);
                            }
                        }
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
        /// 

        /// <summary>
        /// Function Get Image From Resource
        /// CreateBy: DaiBH
        /// CreatedDate: 04/06/2019
        /// </summary>
        /// <param name="organizationCode">organizationCode</param>
        /// <param name="imageFile">imageFile</param>
        /// <returns></returns>
        public async Task<byte[]> GetImageAsync(string organizationCode, string imageFile)
        {
            string resourceDirectory = _environment.WebRootPath + Path.DirectorySeparatorChar + "Uploads" + Path.DirectorySeparatorChar;
            string imagePath = resourceDirectory + organizationCode + Path.DirectorySeparatorChar + imageFile;
            if (File.Exists(imagePath))
            {
                return await Task.Run(() =>
                {
                    return System.IO.File.ReadAllBytes(imagePath);
                });
            }
            throw new Exception("RESOURCE NOT FOUND");
        }

        public int UpdateInforOrganization(TblOrganization organization, int userId, string username)
        {
            int updateOrg = OrganizationConstant.UpdateOrgFail;
            string email = "";
            string taxCode = "";
            string name = "";
            try
            {
                TblOrganization org = db.TblOrganization.Where(o => o.OrganizationId == organization.OrganizationId && o.IsDelete == false).FirstOrDefault();  //GetOrganizationId(userId);
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
                                return OrganizationConstant.UpdateOrgDuplicateEmail;
                            }
                        }
                        // Check OrganizationTaxCode
                        if (org.OrganizationTaxCode.Trim().ToLower() != taxCode)
                        {
                            if (db.TblOrganization.Where(u => u.IsDelete == false &&
                                        organization.OrganizationTaxCode.Trim().ToLower() == u.OrganizationTaxCode.Trim().ToLower()).Count() > 0)
                            {
                                // Duplicate TaxCode
                                return OrganizationConstant.UpdateOrgDuplicateTaxCode;
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
                        return OrganizationConstant.UpdateOrgSuccess;
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

        public void UpdateLogoInforOrg(TblOrganization organization)
        {
            try {
                TblOrganization OrgSave = db.TblOrganization.Where(a => a.OrganizationId == organization.OrganizationId && a.IsDelete == false).FirstOrDefault();
                if(OrgSave !=null)
                {
                    OrgSave.OrganizationLogo = organization.OrganizationLogo;
                    db.Entry(OrgSave).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
