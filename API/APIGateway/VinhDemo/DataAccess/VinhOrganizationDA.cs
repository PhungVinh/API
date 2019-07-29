using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using OrganizationManagement.Common;
using OrganizationManagement.Constant;
//using OrganizationManagement.EmailServkice;
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
using VinhDemo.ViewModel;

namespace OrganizationManagement.DataAccess
{
    public class OrganizationDA : IOrganizationRepository
    {
        private OrganizationCommon organizationConmon = new OrganizationCommon();
        private SP_OrganizationDA orgSP = new SP_OrganizationDA();
        private IDistributedCache _distributedCache;
        //private IEmailService _emailService;
        //private EmailServiceEx _sendMail = new EmailServiceEx();
        //HaiHM add _environment
        private static IHostingEnvironment _environment;
        public OrganizationDA(IHostingEnvironment environment)
        {
            _environment = environment;
        }


        CRM_MASTERContext db = new CRM_MASTERContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MASTERContext>());

        /// <summary>
        /// list danh sach bảng Category
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public object GetOrganizationList(string strFilter)
        {
            try
            {
                List<TblCategory> lstCategory = db.TblCategory.Where(c => c.IsActive == true && c.IsDelete == false).ToList();

                return lstCategory;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        #region Get Position chưa phân trang

        public object GetPosition(string strFilter)
        {
            try
            {
                List<TblCategory> lstCategory = db.TblCategory.Where(c => c.IsActive == true && c.IsDelete == false).ToList();

                return lstCategory;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
        #endregion

        #region List danh sách bảng Organization chưa có phân trang
        /// <summary>
        /// List danh sách bảng Organization
        /// </summary>
        /// <returns></returns>


        public object GetOrganization()
        {
            try
            {
                List<TblOrganization> lstOrganization = db.TblOrganization.Where(c => c.IsActive == true && c.IsDelete == false).ToList();

                return lstOrganization;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        #region Lấy danh sách bảng Authority chưa có phân trang
        /// <summary>
        /// list Authority
        /// </summary>
        /// <returns></returns>
        public object GetAuthority()
        {
            try
            {
                List<TblAuthority> lstAuthority = db.TblAuthority.Where(x => x.IsLock == false && x.IsDelete == false).ToList();
                return lstAuthority;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        #region Lấy danh sách Log user chưa phân trang
        public object GetListLogUser()
        {
            try
            {
                List<TblLogUser> lstLogUser = db.TblLogUser.ToList();
                return lstLogUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        private static HttpClient client = new HttpClient();

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
        
        #region Thêm mới bảng ghi trong TblLogUser @vinhnp
        /// <summary>
        /// AddlogUser 
        /// </summary>
        /// <param name="logUser"></param>
        /// <returns></returns>
        public int AddlogUser(TblLogUser logUser)
        {

            int abc;
            TblLogUser LogU = db.TblLogUser.Where(v => v.Id == logUser.Id).FirstOrDefault();

            if (LogU != null)
            {
                abc = 1;
            }
            else
            {
                TblLogUser addLog = new TblLogUser();
                addLog.ActionName = logUser.ActionName;
                addLog.ActionType = logUser.ActionType;
                addLog.OrganizationCode = logUser.OrganizationCode;
                addLog.ActionDate = logUser.ActionDate;
                addLog.Username = logUser.Username;
                db.TblLogUser.Add(addLog);
                db.SaveChanges();

                abc = 2;

            }

            return abc;


        }
        #endregion

        #region Sửa mới bảng ghi trong TblLogUser @vinhnp
        /// <summary>
        /// UpdateLogUser
        /// </summary>
        /// <param name="logUser"></param>
        /// <returns></returns>
        public int UpdateLogUser(TblLogUser logUser)
        {
            int result;
            TblLogUser LogUChk = db.TblLogUser.Where(v => v.Id == logUser.Id).FirstOrDefault();

            if (LogUChk != null)
            {
                LogUChk.ActionName = logUser.ActionName;
                LogUChk.ActionDate = logUser.ActionDate;
                LogUChk.Username = logUser.Username;
                LogUChk.ActionType = logUser.ActionType;
                LogUChk.OrganizationCode = logUser.OrganizationCode;
                db.Entry(LogUChk).State = EntityState.Modified;
                db.SaveChanges();
                result = 2;
            }
            else
            {
                result = 1;
            }
            return result;
        }
        #endregion

        #region delete TblLogUser @vinhp
        /// <summary>
        /// DeleteLogUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public int DeleteLogUser(int id)
        {
            int result;
            TblLogUser LogUChk = db.TblLogUser.Where(v => v.Id == id).FirstOrDefault();

            if (LogUChk != null)
            {
                db.TblLogUser.Remove(LogUChk);
                db.SaveChanges();
                result = 2;
            }
            else
            {
                result = 1;
            }
            return result;
        }
        #endregion

        #region GetLogUserList @vinhnp
        /// <summary>
        /// GetLogUserList 
        /// </summary>
        /// <param name="OrganizationCode"></param>
        /// <returns></returns>
        public object GetLogUserList(string OrganizationCode)
        {
            try
            {
                List<TblLogUser> lstLogUser = db.TblLogUser.Where(c => c.OrganizationCode.Contains(OrganizationCode)).ToList();

                return lstLogUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        #region SearchLogUser phân trang @vinhnp
        /// <summary>
        /// SearchLogUser theo OrganizationCode hoặc Username
        /// </summary>
        /// <param name="textSeach"></param>
        /// <param name="currentPage"></param>
        /// <param name="recordPerPage"></param>
        /// <returns></returns>
        public object SearchLogUser(string textSeach, int currentPage, int recordPerPage)
        {
            List<List<dynamic>> obj = orgSP.SearchLogUser(textSeach, currentPage, recordPerPage);
            List<LogUserVM> lstLogUser = obj[0].OfType<LogUserVM>().ToList();
            PagePaging pagePaging = obj[1].OfType<PagePaging>().FirstOrDefault();
            return new { data = lstLogUser, pagePaging = pagePaging };
        }
        #endregion

        #region SearchLogUser phân trang @vinhnp
        /// <summary>
        /// SearchCategory theo CategoryCode
        /// </summary>
        /// <param name="textSeach"></param>
        /// <param name="currentPage"></param>
        /// <param name="recordPerPage"></param>
        /// <returns></returns>
        public object SearchCategory (string textSearch, int currentPage, int recordPerPage)
        {
            List<List<dynamic>> obj = orgSP.SearchCategory(textSearch, currentPage, recordPerPage);
            List<CategoryVM> lstCategory = obj[0].OfType<CategoryVM>().ToList();
            PagePaging pagePaging = obj[1].OfType<PagePaging>().FirstOrDefault();
            return new { data = lstCategory, pagePaging = pagePaging };
        }
        #endregion

        #region SearchUserVinhDemo phân trang bảng tblUserVinhDemo @vinhnp
        /// <summary>
        /// SearchUserVinhDemo
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="currentPage"></param>
        /// <param name="recordPerPage"></param>
        /// <returns></returns>
        public object SearchUserVinhDemo (string  textSearch, int currentPage, int recordPerPage)
        {
            List<List<dynamic>> obj = orgSP.SearchUserVinhDemo(textSearch, currentPage, recordPerPage);
            List<UserVinhDemoVM> lstuserVinhDemo = obj[0].OfType<UserVinhDemoVM>().ToList();
            PagePaging pagePaging = obj[1].OfType<PagePaging>().FirstOrDefault();
            return new { data = lstuserVinhDemo, pagePaging = pagePaging };
        }
        #endregion
    }
}
