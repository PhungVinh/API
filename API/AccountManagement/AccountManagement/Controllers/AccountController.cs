using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.DataAccess;
using AccountManagement.Models;
using AccountManagement.Repositories;
using AccountManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TokenExtensionAttribute]
    public class AccountController : Controller
    {
        private IConfiguration _config;
        private readonly IDistributedCache _distributedCache;
        private IOptions<Jwt> _settings;
        //private AccountDA accountDA;
        private AccountCommon accountCommon;
        private IAccountRepository _accountRepository;
        private readonly IAuthorityRepository _authorityRepository;
        private static IHostingEnvironment _environment;
        private LogUserLogDA _log;
        private IHttpContextAccessor _contextAccessor;

        public AccountController(IConfiguration config, IDistributedCache distributedCache, IOptions<Jwt> settings,
            IAccountRepository accountRepository, IHostingEnvironment environmen, IAuthorityRepository authorityRepository, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _config = config;
            _distributedCache = distributedCache;
            _settings = settings;
            //accountDA = new AccountDA(_distributedCache, _config);
            accountCommon = new AccountCommon();
            _accountRepository = accountRepository;
            _environment = environmen;
            _authorityRepository = authorityRepository;
            _log = new LogUserLogDA();
        }

        [AllowAnonymous]
        [Route("~/api/Account/Login")]
        [HttpPost]
        public object Login([FromBody]TblUserLogin login)
        {
          
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                rm.Type = AccountConstant.TypeLogin;
                rm.Title = AccountConstant.TitleLoginNullParameter;
                rm.Message = AccountConstant.MessageLoginNullParameterFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };

                return StatusCode(400, rm);
            }

            if (_accountRepository.CheckLoginAdminOrganization(login.UserName, login.Password))
            {
                response.errorKey = AccountConstant.ErrorLoginAdmin;
                response.title = AccountConstant.TitleLoginAdminErr;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageLoginNotFound;
                return StatusCode(400, response);
            }

            var user = _accountRepository.GetUsersLogin(login.UserName, login.Password);
            if (user != null)
            {

                if (user.IsDelete.Value)
                {
                    // Account hasDelete
                    response.errorKey = AccountConstant.ErrorLoginDelete;
                    response.title = AccountConstant.TitleLoginDelete;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageLoginNotFound;
                    return StatusCode(400, response);
                }
                if (user.IsLock.Value)
                {
                    // Account hasLock
                    response.errorKey = AccountConstant.ErrorLoginLock;
                    response.title = AccountConstant.TitleLoginLock;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageLoginLock;
                    return StatusCode(400, response);
                }
                DateTime checkDate = user.ExpirationDate ?? DateTime.MinValue;
                DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());
                DateTime dbTime = DateTime.Parse(checkDate.ToShortDateString());
                TblOrganization tblOrganization = _accountRepository.GetOrganizationId(user.Id);
                if (tblOrganization == null)
                {
                    response.errorKey = AccountConstant.ErrorLoginLockOrgLockOrDelete;
                    response.title = AccountConstant.TitleLoginOrgLockOrDelete;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageLoginNotFound;
                    return StatusCode(400, response);
                }
                if (DateTime.Compare(dbTime, now) <= 0 || !user.EmailConfirmed.Value)
                {
                    // ok
                    var tokenString = GenerateJSONWebToken(user);
                    var responseNew = new { mustChangePassword = true, token = tokenString };

                    return Json(responseNew);
                }
                else
                {
                    // Login Success
                    // Get Infor client
                    //var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                    //string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    //if (Request.Headers.ContainsKey(AccountConstant.XForwardedFor))
                    //    remoteIpAddress = Request.Headers[AccountConstant.XForwardedFor];
                    //var userAgent = Request.Headers[AccountConstant.UserAgent];
                    //string orgCode = tblOrganization != null ? tblOrganization.OrganizationCode : "MP";
                    //TblLogUser log = new TblLogUser
                    //{
                    //    Type = AccountConstant.TypeLoginS,
                    //    OrganizationCode = orgCode,
                    //    Module = AccountConstant.ModuleLogin,
                    //    ActionType = AccountConstant.ActionTypeLogin,
                    //    ActionName = AccountConstant.ActionNameLogin,
                    //    ActionResult = AccountConstant.LoginSuccess,
                    //    ActionDate = DateTime.Now,
                    //    Username = login.UserName,
                    //    LoginOperatingSystemName = osNameAndVersion,
                    //    LoginBrowserName = userAgent,
                    //    LoginIpaddress = remoteIpAddress,
                    //    LoginTimeStart = DateTime.Now
                    //};
                    //_log.AddLog(log);
                    // End getIP
                    var tokenString = GenerateJSONWebToken(user);
                    var responseNew = new { token = tokenString };
                    return Json(responseNew);
                }
            }
            else
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorLoginNotFound;
                response.title = AccountConstant.TitleLoginNotFound;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageLoginNotFound;
                return StatusCode(400, response);
            }
        }

        /// <summary>
        /// CreatedBy: System
        /// ModifiedBy: HaiHM
        /// ModifiedDate: 26/04/2019
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJSONWebToken(TblUsers userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[AccountConstant.JwtKey]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //get department
            TblOrganization tblOrganization = _accountRepository.GetOrganizationId(userInfo.Id);
            string orgCode = tblOrganization != null ? tblOrganization.OrganizationCode : "MP";
            var claimRole = _authorityRepository.GetAuthorityNameOfUser(userInfo.Id);

            var claims = new List<Claim>();
            claims.Add(new Claim(AccountConstant.sub, userInfo.UserName));
            claims.Add(new Claim(AccountConstant.userId, userInfo.Id.ToString()));
            claims.Add(new Claim(AccountConstant.orgCode, orgCode));
            claims.Add(new Claim(AccountConstant.ExpLogin, DateTime.Now.AddMinutes(AccountConstant.AddMinuteExpLogin).ToString()));
            //Add Role
            string lstRole = _accountRepository.GetRolePermission(userInfo.Id, AccountConstant.ADMIN_USER);
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
            claims.Add(new Claim(AccountConstant.Role, claimRole));

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
        /// CreatedBy: System
        /// ModifiedBy: HaiHM
        /// ModifiedDate: 26/04/2019
        /// Content Modified: check token is null
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetUser")]
        [HttpGet]
        [Authorize]
        public object GetUser()
        {
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            string username = User.Claims.FirstOrDefault().Value;
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            return _accountRepository.GetUsersCustom(username, orgCode, userId);
        }

        /// <summary>
        /// Function Get TblMenu SPs
        /// CreatedBy: HiepPD1
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [Route("~/api/Account/GetTblMenuSPs")]
        [HttpGet]
        [Authorize]
        public object GetTblMenuSPs([FromQuery]int roleId)
        {
            //var cacheKey = "api/Account/GetTblMenuSPs" + roleId;
            //var cacheValue = _accountRepository.GetStringCache(cacheKey);
            //if (!string.IsNullOrEmpty(cacheValue))
            //{
            //    return cacheValue;
            //}
            //else
            //{
            //    _accountRepository.SetStringCache(cacheKey, _accountRepository.GetTblMenuSPs(roleId));
            //}
            return _accountRepository.GetTblMenuSPs(roleId);
        }

        /// <summary>
        /// Function Get List Menu
        /// CreatedBy: HiepPD1
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetMenuList")]
        [HttpGet]
        [Authorize]
        public object GetMenuList()
        {
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            return _accountRepository.GetMenuList(userId);
        }

        /// <summary>
        /// Function Add User
        /// CreateBy: HaiHM
        /// CreatedDate: 12/04/2019
        /// </summary>
        /// <param name="UserAndOrgViewModel">object</param>
        /// <returns></returns>
        [Route("~/api/Account/AddUser")]
        [HttpPost]
        [Authorize(AccountConstant.PolicyCanAdd)]
        public IActionResult AddUser([FromBody] UserAndOrgViewModel model)
        {
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<FieldErrors> lsterror = new List<FieldErrors>();

            if (string.IsNullOrEmpty(model.tblUsers.UserName) || string.IsNullOrEmpty(model.tblUsers.Email) || string.IsNullOrEmpty(model.tblUsers.PhoneNumber))
            {
                if (string.IsNullOrEmpty(model.tblUsers.UserName))
                {
                    error = new FieldErrors();
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.TypeNullUserName;
                    error.message = AccountConstant.MsgTypeNullUserName;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(model.tblUsers.Email))
                {
                    error = new FieldErrors();
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.AddTypeNullEmail;
                    error.message = AccountConstant.MsgAddTypeNullEmail;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(model.tblUsers.PhoneNumber))
                {
                    error = new FieldErrors();
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.TypeNullPhoneNumber;
                    error.message = AccountConstant.MsgTypeNullPhoneNumber;
                    lsterror.Add(error);
                }
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleAddUserNull;
                rm.Message = AccountConstant.MessageAddUserNull;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, Json(rm));
            }
            if (model.tblAuthority != null)
            {
                if (model.tblAuthority.Count > AccountConstant.Min0)
                {
                    if (string.IsNullOrEmpty(model.tblAuthority.FirstOrDefault().AuthorityId.ToString()) || model.tblAuthority.FirstOrDefault().AuthorityId == 0)
                    {
                        rm.Type = AccountConstant.TypeUser;
                        rm.Status = AccountConstant.statusError;
                        rm.Title = AccountConstant.TitleAddUserNullRoleId;
                        rm.Message = AccountConstant.MessageAddUserNullRoleId;
                        var field = new { fieldErrors = rm.Title };

                        return StatusCode(400, Json(rm));
                    }
                }
            }
            string username = User.Claims.FirstOrDefault().Value;
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            TblOrganization org = _authorityRepository.GetOrganization(orgCode);
            if (org != null)
            {
                if (model.tblUsers.IsLock == true || _accountRepository.CheckServicePack(org))
                {
                    orgCode = orgCode.ToLower() + AccountConstant.GachDuoi;
                    string checkOrgCode = "";
                    if (model.tblUsers.UserName.Length > orgCode.Length)
                    {
                        checkOrgCode = model.tblUsers.UserName.Trim().Substring(0, orgCode.Length).ToLower();
                        if (!string.Equals(orgCode, checkOrgCode))
                        {
                            model.tblUsers.UserName = orgCode + model.tblUsers.UserName;
                        }
                    }
                    else
                    {
                        model.tblUsers.UserName = orgCode + model.tblUsers.UserName;
                    }

                    model.tblOrganization = org;
                    model.tblUsers.CreateBy = username;
                    model.tblUsers.UpdateBy = username;
                    int save = _accountRepository.AddUser(model, userId);
                    //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
                    //_accountRepository.GetStringCache(AccountConstant.GetUserList);

                    if (save == AccountConstant.AddUserSuccess)
                    {
                        object obj = new { message = AccountConstant.MessageAddSuccess };
                        return StatusCode(201, obj);
                    }
                    else if (save == AccountConstant.AddUserDuplicateUsername)
                    {
                        response.entityName = AccountConstant.TypeUser;
                        response.errorKey = AccountConstant.ErrorKeyDuplicateUsername;
                        response.status = AccountConstant.statusError;
                        response.message = AccountConstant.MessageAddDuplicateUsernames;
                        return StatusCode(400, Json(response));

                    }
                    else if (save == AccountConstant.AddUserDuplicateEmail)
                    {
                        response.entityName = AccountConstant.TypeUser;
                        response.errorKey = AccountConstant.ErrorKeyDuplicateEmail;
                        response.status = AccountConstant.statusError;
                        response.message = AccountConstant.MessageAddDuplicateEmail;
                        return StatusCode(400, Json(response));
                    }
                    else if (save == AccountConstant.AddUserDuplicatePhoneNumber)
                    {
                        response.entityName = AccountConstant.TypeUser;
                        response.errorKey = AccountConstant.ErrorKeyDuplicatePhoneNumber;
                        response.status = AccountConstant.statusError;
                        response.message = AccountConstant.MessageAddDuplicatePhoneNumber;
                        return StatusCode(400, Json(response));
                    }
                    else
                        return BadRequest();
                }
                else
                {
                    response.entityName = AccountConstant.TypeUser;
                    response.errorKey = AccountConstant.ErrorKeyMaxUser;
                    response.title = AccountConstant.TitleMaxUser;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageMaxUser;
                    return StatusCode(400, Json(response));
                }
            }
            return BadRequest();
        }

        #region HaiHM

        /// <summary>
        /// Function Edit User
        /// CreateBy: HaiHM
        /// CreatedDate: 12/04/2019
        /// </summary>
        /// <param name="UserAndOrgViewModel">object</param>
        /// <returns></returns>
        [Route("~/api/Account/EditUser")]
        [HttpPut]
        [Authorize(Policy = AccountConstant.PolicyEdit)]
        public IActionResult EditUser([FromBody] UserAndOrgViewModel model)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.tblUsers != null)
            {
                if (string.IsNullOrEmpty(model.tblUsers.UserName) || string.IsNullOrEmpty(model.tblUsers.Email) || string.IsNullOrEmpty(model.tblUsers.PhoneNumber))
                {
                    List<FieldErrors> lsterror = new List<FieldErrors>();
                    if (string.IsNullOrEmpty(model.tblUsers.UserName))
                    {
                        error = new FieldErrors();
                        error.objectName = AccountConstant.TypeUser;
                        error.field = AccountConstant.TypeNullUserName;
                        error.message = AccountConstant.MsgTypeNullUserName;
                        lsterror.Add(error);
                    }
                    if (string.IsNullOrEmpty(model.tblUsers.Email))
                    {
                        error = new FieldErrors();
                        error.objectName = AccountConstant.TypeUser;
                        error.field = AccountConstant.AddTypeNullEmail;
                        error.message = AccountConstant.MsgAddTypeNullEmail;
                        lsterror.Add(error);
                    }
                    if (string.IsNullOrEmpty(model.tblUsers.PhoneNumber))
                    {
                        error = new FieldErrors();
                        error.objectName = AccountConstant.TypeUser;
                        error.field = AccountConstant.TypeNullPhoneNumber;
                        error.message = AccountConstant.MsgTypeNullPhoneNumber;
                        lsterror.Add(error);
                    }
                    if (string.IsNullOrEmpty(model.tblUsers.Id.ToString()) || model.tblUsers.Id == 0)
                    {
                        error = new FieldErrors();
                        error.objectName = AccountConstant.TypeUser;
                        error.field = AccountConstant.TypeNullId;
                        error.message = AccountConstant.MsgTypeNullId;
                        lsterror.Add(error);
                    }

                    rm.Type = AccountConstant.TypeUser;
                    rm.Title = AccountConstant.TitleNullUserOrId;
                    rm.Message = AccountConstant.MessageNullUserOrId;
                    rm.Status = AccountConstant.statusError;
                    var field = new { fieldErrors = lsterror };

                    return StatusCode(400, Json(rm));
                }
            }
            else
            {
                return BadRequest();
            }
            if (model.tblAuthority != null)
            {
                if (model.tblAuthority.Count > AccountConstant.Min0)
                {
                    if (string.IsNullOrEmpty(model.tblAuthority.FirstOrDefault().AuthorityId.ToString()) || model.tblAuthority.FirstOrDefault().AuthorityId == 0)
                    {
                        rm.Type = AccountConstant.TypeUser;
                        rm.Status = AccountConstant.statusError;
                        rm.Title = AccountConstant.TitleAddUserNullRoleId;
                        rm.Message = AccountConstant.MessageAddUserNullRoleId;
                        var field = new { fieldErrors = rm.Title };

                        return StatusCode(400, Json(rm));
                    }
                }
            }
            //Use for Check Permission
            var lst = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            if (lst != null)
            {
                if (lst.Contains(AccountConstant.CanEdit) && !lst.Contains(AccountConstant.CanEditAll))
                {
                    if (!_accountRepository.CheckPermission(userId, model.tblUsers.Id))
                    {
                        return StatusCode(403);
                    }
                }
            }
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            string username = User.Claims.FirstOrDefault().Value;
            TblOrganization org = _authorityRepository.GetOrganization(orgCode);
            // org.CreateBy;
            model.tblUsers.UpdateBy = username;
            if (org != null)
            {
                if (model.tblUsers.IsLock == true || _accountRepository.CheckServicePack(org) ||
                        model.tblUsers.IsLock == _accountRepository.GetUserById(model.tblUsers.Id).IsLock)
                {
                    orgCode = orgCode.ToLower() + AccountConstant.GachDuoi;
                    string checkOrgCode = model.tblUsers.UserName.Trim().Substring(0, orgCode.Length).ToLower();
                    if (!string.Equals(orgCode, checkOrgCode))
                    {
                        model.tblUsers.UserName = orgCode + model.tblUsers.UserName;
                    }
                    model.tblOrganization = org;
                    model.tblUsers.UpdateBy = username;
                    int edit = _accountRepository.EditUser(model, userId);
                    //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
                    //_accountRepository.GetStringCache(AccountConstant.GetUserList);

                    if (edit == AccountConstant.EditUserSuccess)
                    {
                        //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
                        //_accountRepository.GetStringCache(AccountConstant.GetUserList);
                        object obj = new { message = AccountConstant.MessageEditSuccess };
                        return StatusCode(200, obj);
                    }
                    else if (edit == AccountConstant.EditUserDuplicateEmail)
                    {
                        response.entityName = AccountConstant.TypeUser;
                        response.errorKey = AccountConstant.ErrorKeyDuplicateEmail;
                        response.title = AccountConstant.TitleDuplicateEmail;
                        response.status = AccountConstant.statusError;
                        response.message = AccountConstant.MessageAddDuplicateEmail;
                        return StatusCode(400, Json(response));

                    }
                    else if (edit == AccountConstant.EditUserDuplicatePhoneNumber)
                    {
                        response.entityName = AccountConstant.TypeUser;
                        response.errorKey = AccountConstant.ErrorKeyDuplicatePhoneNumber;
                        response.title = AccountConstant.TitleDuplicatePhoneNumber;
                        response.status = AccountConstant.statusError;
                        response.message = AccountConstant.MessageAddDuplicatePhoneNumber;
                        return StatusCode(400, Json(response));
                    }
                    else
                        return BadRequest();
                }
                else
                {
                    response.entityName = AccountConstant.TypeUser;
                    response.errorKey = AccountConstant.ErrorKeyMaxUser;
                    response.title = AccountConstant.TitleMaxUser;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageMaxUser;
                    return StatusCode(400, Json(response));
                }

            }
            else
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleAddUserNullOrganizationId;
                rm.Message = AccountConstant.MessageAddUserNullOrganizationId;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };

                return StatusCode(400, Json(rm));
            }
        }

        /// <summary>
        /// Function ResetPass User
        /// CreateBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <param name="user"></param> object
        /// <returns></returns>
        [Route("~/api/Account/ResetPass")]
        [HttpPost]
        [Authorize(Policy = AccountConstant.PolicyEdit)]
        public IActionResult ResetPass([FromBody] TblUsers user)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Use for Check Permission
            var lst = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();
            if (lst != null)
            {
                if (lst.Contains(AccountConstant.CanEdit) && !lst.Contains(AccountConstant.CanEditAll))
                {
                    int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
                    if (!_accountRepository.CheckPermission(userId, user.Id))
                    {
                        return StatusCode(403);
                    }
                }
            }
            //End Use for Check Permission
            if (user == null || string.IsNullOrEmpty(user.Id.ToString()) || user.Id == AccountConstant.Min0)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullUserOrId;
                rm.Message = AccountConstant.MessageNullUserOrId;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };


                return StatusCode(400, Json(rm));
            }
            int result = _accountRepository.ResetPassUser(user);

            if (result == AccountConstant.ResetPassSuccess)
            {
                // Remove token
                string token = _contextAccessor.HttpContext.Request.Headers[AccountConstant.Authorization];
                token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                token = accountCommon.MD5Hash(token);
                string cacheValue = _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                cacheValue += token;
                _accountRepository.SetStringCache(AccountConstant.ListLogoutToken, cacheValue);
                _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);

                object obj = new { message = AccountConstant.MessageResetPassOk };
                return StatusCode(200, obj);
            }
            else
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyResetPass;
                response.title = AccountConstant.TitleResetPass;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageResetPassFail;
                return StatusCode(400, Json(response));
            }
        }

        /// <summary>
        /// Function check token in email
        /// CreateBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// ModifiedDate: 10/06/2019
        /// ModifiedBody: rút gọn nội dung token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("~/api/Account/CheckTokenResetPass")]
        [HttpGet]
        public IActionResult CheckTokenResetPass(string username, string codeReset)
        {
            //int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);

            // Delete token
            string tokenCheck = accountCommon.MD5Hash(username + codeReset);
            var blackList = _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
            if (blackList != null && tokenCheck != null)
            {
                if (blackList.Contains(tokenCheck))
                {
                    return BadRequest();
                }
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(codeReset))
            {
                rm.Type = AccountConstant.TypeNullToken;
                rm.Title = AccountConstant.TitleNullToken;
                rm.Message = AccountConstant.MessageNullToken;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };

                return StatusCode(400, Json(rm));
            }
            if (_accountRepository.CheckTokenReset(username, codeReset))
            {
                TblUsers user = _accountRepository.GetUsers(username);
                if (user != null)
                {
                    user.EmailConfirmed = false;
                    // xóa mật khẩu cũ
                    user.Password = accountCommon.HashPassword(accountCommon.CreateRandomPassword());
                    _accountRepository.UpdateUser(user, false, true, false, 0);
                    //string token = _accountRepository.GenerateJSONWebTokenReset(user);
                    var obj = new { token = true };
                    return Json(obj);
                }
                return BadRequest();
            }
            else
            {
                response.errorKey = AccountConstant.TypeInValidToken;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageTokenResetFail;
                return StatusCode(400, new JsonResult(response));
            }
        }

        

        /// <summary>
        /// Function change password Account
        /// CreateBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <param name="viewModel">object</param>
        /// <returns></returns>
        [Route("~/api/Account/ChangePassword")]
        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePassViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();

            if (string.IsNullOrEmpty(viewModel.Pass1) || string.IsNullOrEmpty(viewModel.Pass2))
            {
                List<FieldErrors> lsterror = new List<FieldErrors>();
                if (string.IsNullOrEmpty(viewModel.Pass1))
                {
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.FieldPass1;
                    error.message = AccountConstant.MessageTypeNullEmail;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(viewModel.Pass2))
                {
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.FieldPass2;
                    error.message = AccountConstant.MessageTypeNullTaxCode;
                    lsterror.Add(error);
                }
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullNewPassword;
                rm.Message = AccountConstant.MessageNullNewPasswordFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, Json(rm));
            }

            string token = HttpContext.Request.Headers[AccountConstant.Authorization];
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            TblUsers user = _accountRepository.GetUserById(userId);
            if (user == null)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleTokenInvalid;
                rm.Message = AccountConstant.MessageTokenResetFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };


                return StatusCode(400, Json(rm));
            }
            int changePass = _accountRepository.ChangePassword(viewModel, user);

            if (changePass == AccountConstant.ChangePasswordSuccess)
            {
                // cập nhật lại thông tin
                user.DateUpdatePassword = null;
                user.CodeReset = null;
                _accountRepository.UpdateUser(user, false, false, true, userId);
                //Destroy Token
                token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                token = accountCommon.MD5Hash(token);
                string cacheValue = _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                cacheValue += token;
                _accountRepository.SetStringCache(AccountConstant.ListLogoutToken, cacheValue);
                _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                // End Destroy
                string tokenNew = GenerateJSONWebToken(user);
                object obj = new { message = AccountConstant.MessageNewPasswordSuccess, token = tokenNew };
                return StatusCode(200, obj);
            }
            else if (changePass == AccountConstant.ChangePasswordDuplicateOldPass)
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyChangePassFail;
                response.title = AccountConstant.TitleChangePassFail;
                response.status = AccountConstant.statusSuccess;
                response.message = AccountConstant.MessageChangePassFail;
                return StatusCode(400, Json(response));
            }
            else if (changePass == AccountConstant.NotSame)
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyChangePassNotSame;
                response.title = AccountConstant.TitleChangePassNotSame;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageChangePassNotSame;
                return StatusCode(400, Json(response));
            }
            else
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyChangeInputPassFail;
                response.title = AccountConstant.TitleChangePassInputPassFail;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageChangePassInputPassFail;
                return StatusCode(400, Json(response));
            }
        }

        /// <summary>
        /// Function change password Account
        /// CreateBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <param name="viewModel">object</param>
        /// <returns></returns>
        [Route("~/api/Account/ChangePasswordReset")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ChangePasswordReset([FromBody] ChangePassViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();

            if (string.IsNullOrEmpty(viewModel.Pass1) 
                || string.IsNullOrEmpty(viewModel.Pass2)
                || string.IsNullOrEmpty(viewModel.username)
                || string.IsNullOrEmpty(viewModel.codeReset))
            {
                List<FieldErrors> lsterror = new List<FieldErrors>();
                if (string.IsNullOrEmpty(viewModel.Pass1))
                {
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.FieldPass1;
                    error.message = AccountConstant.MessageTypeNullEmail;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(viewModel.Pass2))
                {
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.FieldPass2;
                    error.message = AccountConstant.MessageTypeNullEmail;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(viewModel.username))
                {
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.username;
                    error.message = AccountConstant.MessageTypeNullEmail;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(viewModel.codeReset))
                {
                    error.objectName = AccountConstant.TypeUser;
                    error.field = AccountConstant.codeReset;
                    error.message = AccountConstant.MessageTypeNullEmail;
                    lsterror.Add(error);
                }
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullNewPassword;
                rm.Message = AccountConstant.MessageNullNewPasswordFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, Json(rm));
            }
            if(_accountRepository.CheckTokenReset(viewModel.username, viewModel.codeReset))
            {
                TblUsers user = _accountRepository.GetUsers(viewModel.username);
                if (user == null)
                {
                    rm.Type = AccountConstant.TypeUser;
                    rm.Title = AccountConstant.TitleTokenInvalid;
                    rm.Message = AccountConstant.MessageTokenResetFail;
                    rm.Status = AccountConstant.statusError;
                    var field = new { fieldErrors = rm.Title };

                    return StatusCode(400, Json(rm));
                }
                int changePass = _accountRepository.ChangePassword(viewModel, user);

                if (changePass == AccountConstant.ChangePasswordSuccess)
                {
                    // cập nhật lại thông tin
                    user.DateUpdatePassword = null;
                    user.CodeReset = null;
                    //int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
                    _accountRepository.UpdateUser(user, false, false, true, 0);
                    
                    object obj = new { message = AccountConstant.MessageNewPasswordSuccess};
                    return StatusCode(200, obj);
                }
                else if (changePass == AccountConstant.ChangePasswordDuplicateOldPass)
                {
                    response.entityName = AccountConstant.TypeUser;
                    response.errorKey = AccountConstant.ErrorKeyChangePassFail;
                    response.title = AccountConstant.TitleChangePassFail;
                    response.status = AccountConstant.statusSuccess;
                    response.message = AccountConstant.MessageChangePassFail;
                    return StatusCode(400, Json(response));
                }
                else if (changePass == AccountConstant.NotSame)
                {
                    response.entityName = AccountConstant.TypeUser;
                    response.errorKey = AccountConstant.ErrorKeyChangePassNotSame;
                    response.title = AccountConstant.TitleChangePassNotSame;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageChangePassNotSame;
                    return StatusCode(400, Json(response));
                }
                else
                {
                    response.entityName = AccountConstant.TypeUser;
                    response.errorKey = AccountConstant.ErrorKeyChangeInputPassFail;
                    response.title = AccountConstant.TitleChangePassInputPassFail;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageChangePassInputPassFail;
                    return StatusCode(400, Json(response));
                }
            }
            else
            {
                response.errorKey = AccountConstant.TypeInValidToken;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageTokenResetFail;
                return StatusCode(400, new JsonResult(response));
            }
        }

        /// <summary>
        /// Function change password Account
        /// CreateBy: HaiHM
        /// CreatedDate: 21/04/2019
        /// </summary>
        /// <param name="viewModel">object</param>
        /// <returns></returns>
        [Route("~/api/Account/ChangePasswordUser")]
        [HttpPost]
        [Authorize]
        public IActionResult ChangePasswordUser([FromBody] ChangePassViewModel model)
        {
            //Test
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(model.Pass1) || string.IsNullOrEmpty(model.Pass2) || string.IsNullOrEmpty(model.oldPassword))
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullNewPassword;
                rm.Message = AccountConstant.MessageNullNewPasswordFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };

                return StatusCode(400, Json(rm));
            }
            string token = HttpContext.Request.Headers[AccountConstant.Authorization];
            TblUsers user = DecodeTokenUser(token);
            if (user == null)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleTokenInvalid;
                rm.Message = AccountConstant.MessageTokenResetFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };


                return StatusCode(400, Json(rm));
            }
            int changePass = _accountRepository.ChangePassword(model, user);

            if (changePass == AccountConstant.ChangePasswordSuccess)
            {
                //Destroy Token
                token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                token = accountCommon.MD5Hash(token);
                string cacheValue = _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                cacheValue += token;
                _accountRepository.SetStringCache(AccountConstant.ListLogoutToken, cacheValue);
                _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                // End Destroy
                string tokenNew = GenerateJSONWebToken(user);
                object obj = new { message = AccountConstant.MessageNewPasswordSuccess, token = tokenNew };
                return StatusCode(200, obj);
            }
            else if (changePass == AccountConstant.ChangePasswordDuplicateOldPass)
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyChangePassFail;
                response.title = AccountConstant.TitleChangePassFail;
                response.status = AccountConstant.statusSuccess;
                response.message = AccountConstant.MessageChangePassFail;
                return StatusCode(400, Json(response));
            }
            else if (changePass == AccountConstant.NotSame)
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyChangePassNotSame;
                response.title = AccountConstant.TitleChangePassNotSame;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageChangePassNotSame;
                return StatusCode(400, Json(response));
            }
            else
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyChangeInputPassFail;
                response.title = AccountConstant.TitleChangePassInputPassFail;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageChangePassInputPassFail;
                return StatusCode(400, Json(response));
            }
        }

        /// <summary>
        /// Function lock User
        /// CreateBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <param name="user"></param> object
        /// <returns></returns>
        //[Authorize(Policy = AccountConstant.PolicyEdit)]
        [Route("~/api/Account/LockUser")]
        [HttpPatch]
        [Authorize]
        public IActionResult LockUser([FromBody] TblUsers user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            //Use for Check Permission
            //var lst = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();
            //if (lst != null)
            //{
            //    if (lst.Contains(AccountConstant.CanDelete))
            //    {
            //        int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            //        if (!_accountRepository.CheckPermission(userId, user.Id))
            //        {
            //            return StatusCode(403);
            //        }
            //    }
            //}
            // End Check Permission
            if (user == null || string.IsNullOrEmpty(user.Id.ToString()) || user.Id == 0)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullUserOrId;
                rm.Message = AccountConstant.MessageNullUserOrId;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };


                return StatusCode(400, Json(rm));
            }
            string username = User.Claims.FirstOrDefault().Value;
            user.UpdateBy = username;
            bool lockUser = _accountRepository.LockUser(user);
            //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
            //_accountRepository.GetStringCache(AccountConstant.GetUserList);

            if (lockUser)
            {
                object obj = new { message = AccountConstant.MessageLockUserSuccess };
                return StatusCode(200, obj);
            }
            else
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyLockUserFail;
                response.title = AccountConstant.TitleLockUserFail;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageLockUserFail;
                return StatusCode(400, Json(response));
            }
        }

        /// <summary>
        /// Function Active User
        /// CreateBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //[Authorize(Policy = AccountConstant.PolicyEdit)]
        [Route("~/api/Account/ActiveUser")]
        [HttpPatch]
        [Authorize]
        public IActionResult ActiveUser([FromBody] TblUsers user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            //Use for Check Permission
            //var lst = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();
            //if (lst != null)
            //{
            //    if (lst.Contains(AccountConstant.CanDelete))
            //    {
            //        int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            //        if (!_accountRepository.CheckPermission(userId, user.Id))
            //        {
            //            return StatusCode(403);
            //        }
            //    }
            //}
            // End Check Permission
            if (user == null || string.IsNullOrEmpty(user.Id.ToString()) || user.Id == 0)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullUserOrId;
                rm.Message = AccountConstant.MessageNullUserOrId;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };


                return StatusCode(400, Json(rm));
            }
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            string username = User.Claims.FirstOrDefault().Value;
            user.UpdateBy = username;
            TblOrganization org = _authorityRepository.GetOrganization(orgCode);
            if (org != null)
            {
                if (_accountRepository.CheckServicePack(org))
                {
                    bool activeUser = _accountRepository.ActiveUser(user);
                    //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
                    //_accountRepository.GetStringCache(AccountConstant.GetUserList);

                    if (activeUser)
                    {
                        object obj = new { message = AccountConstant.MessageActiveUserSuccess };
                        return StatusCode(200, obj);
                    }
                    else
                    {
                        response.entityName = AccountConstant.TypeUser;
                        response.errorKey = AccountConstant.ErrorKeyActiveUserFail;
                        response.title = AccountConstant.TitleActiveUserFail;
                        response.status = AccountConstant.statusSuccess;
                        response.message = AccountConstant.MessageActiveUserFail;
                        return StatusCode(400, Json(response));
                    }
                }
                else
                {
                    response.entityName = AccountConstant.TypeUser;
                    response.errorKey = AccountConstant.ErrorKeyMaxUser;
                    response.title = AccountConstant.TitleMaxUser;
                    response.status = AccountConstant.statusError;
                    response.message = AccountConstant.MessageMaxUser;
                    return StatusCode(400, Json(response));
                }
            }
            else
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleAddUserNullOrganizationId;
                rm.Message = AccountConstant.MessageAddUserNullOrganizationId;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };

                return StatusCode(400, Json(rm));
            }
        }

        /// <summary>
        /// Function Delete User
        /// CreateBy: HaiHM
        /// CreatedDate: 14/04/2019
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("~/api/Account/DeleteUser/{id}")]
        [HttpDelete("{id}")]
        [Authorize(Policy = AccountConstant.PolicyDelete)]
        public IActionResult DeleteUser(int id)
        {
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            string username = User.Claims.FirstOrDefault().Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Use for Check Permission
            var lst = User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList();
            if (lst != null)
            {
                if (lst.Contains(AccountConstant.CanDelete) && !lst.Contains(AccountConstant.CanDeleteAll))
                {
                    if (!_accountRepository.CheckPermission(userId, id))
                    {
                        return StatusCode(403);
                    }
                }
            }
            // End Check Permission
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (string.IsNullOrEmpty(id.ToString()) || id == 0)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullUserOrId;
                rm.Message = AccountConstant.MessageNullUserOrId;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };


                return StatusCode(400, Json(rm));
            }
            int delete = _accountRepository.DeleteUser(id, username);
            //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
            //_accountRepository.GetStringCache(AccountConstant.GetUserList);

            if (delete == AccountConstant.DeleteUserSuccess)
            {
                object obj = new { message = AccountConstant.MessageDeleteUserSuccess };
                return StatusCode(200, obj);
            }
            else
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyDeleteUserFail;
                response.title = AccountConstant.TitleDeleteUserFail;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageDeleteUserFail;
                return StatusCode(400, Json(response));
            }
        }

        /// <summary>
        /// Function using for search User
        /// CreateBy: HaiHM
        /// CreatedDate: 15/04/2019
        /// </summary>
        /// <param name="search">Full text search</param>
        /// <param name="index">page now</param>
        /// <param name="size">size of Page</param>
        /// <param name="sortType">Type sort</param>
        /// <returns></returns>
        [Route("~/api/Account/SearchUser")]
        [HttpGet]
        [Authorize(Policy = AccountConstant.PolicyShow)]
        public async Task<Object> SearchUser(string textSearch, string isActive, string orgCode, string currPage, string recordperpage)
        {
            try
            {
                int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
                TblUsers user = _accountRepository.GetUserById(userId);

                if (!string.IsNullOrEmpty(textSearch))
                {
                    textSearch = textSearch.Trim();
                }
                if (!string.IsNullOrEmpty(orgCode))
                {
                    orgCode = orgCode.Trim();
                }
                if (!string.IsNullOrEmpty(currPage))
                {
                    currPage = currPage.Trim();
                }
                if (!string.IsNullOrEmpty(recordperpage))
                {
                    recordperpage = recordperpage.Trim();
                }
                string arr = userId + AccountConstant.StringSlipSearch
                       + textSearch + AccountConstant.StringSlipSearch
                       + isActive + AccountConstant.StringSlipSearch
                       + orgCode + AccountConstant.StringSlipSearch
                       + currPage + AccountConstant.StringSlipSearch
                       + recordperpage;

                return await _accountRepository.SearchUser(userId, arr);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NoContent();
            }
        }

        /// <summary>
        ///  Function GetAllCategory (lấy dữ liệu bảng danh mục theo CategoryTypeCode) 
        /// CreateBy: HaiHM
        /// CreatedDate: 16/04/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetAllCategory")]
        [HttpGet]
        [Authorize]
        public object GetAllCategory(string CategoryTypeCode)
        {
            if (!string.IsNullOrEmpty(CategoryTypeCode))
            {
                string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
                var response = new { data = _accountRepository.GetAllCategory(CategoryTypeCode, orgCode, String.Empty) };
                return response;

            }
            return BadRequest();
        }

        /// <summary>
        /// Function using for Update User (Not admin and superAdmin)
        /// CreateBy: HaiHM
        /// CreatedDate: 16/04/2019
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("~/api/Account/UpdateUser")]
        [HttpPost]
        [Authorize]
        public IActionResult UpdateUser([FromBody]TblUsers user)
        {
            if (!ModelState.IsValid )
            {
                return BadRequest(ModelState);
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (user == null || string.IsNullOrEmpty(user.Id.ToString()) || user.Id == 0)
            {
                rm.Type = AccountConstant.TypeUser;
                rm.Title = AccountConstant.TitleNullUserOrId;
                rm.Message = AccountConstant.MessageUpdateUserFail;
                rm.Status = AccountConstant.statusError;
                var field = new { fieldErrors = rm.Title };

                return StatusCode(400, Json(rm));
            }
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            int updateSelf = _accountRepository.UpdateUser(user, false, false, false, userId);

            //_accountRepository.SetStringCache(AccountConstant.GetUserList, _accountRepository.SearchUser(AccountConstant.StringNullGetListUser));
            //_accountRepository.GetStringCache(AccountConstant.GetUserList);

            if (updateSelf == AccountConstant.EditUserSuccess)
            {
                object obj = new { message = AccountConstant.MessageUpdateUserSuccess };
                return StatusCode(200, obj);
            }
            else if (updateSelf == AccountConstant.EditUserDuplicateEmail)
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyDuplicateEmail;
                response.title = AccountConstant.TitleDuplicateEmail;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageAddDuplicateEmail;
                return StatusCode(400, Json(response));
            }
            else if (updateSelf == AccountConstant.EditUserDuplicatePhoneNumber)
            {
                response.entityName = AccountConstant.TypeUser;
                response.errorKey = AccountConstant.ErrorKeyDuplicatePhoneNumber;
                response.title = AccountConstant.TitleDuplicatePhoneNumber;
                response.status = AccountConstant.statusError;
                response.message = AccountConstant.MessageAddDuplicatePhoneNumber;
                return StatusCode(400, Json(response));
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Function using for get parent Menu -- ParentCode = CRM
        /// CreateBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetTblMenuParent")]
        [HttpGet]
        [Authorize]
        public async Task<object> GetTblMenuParent()
        {
            var obj = _accountRepository.GetTblMenuParent(false);
            _accountRepository.SetStringCache(AccountConstant.GetTblMenuParent, obj);
            return await _accountRepository.GetStringAsync(AccountConstant.GetTblMenuParent);
        }

        /// <summary>
        /// Function using for get parent Menu -- ParentCode = CRM
        /// CreateBy: HaiHM
        /// CreatedDate: 20/04/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetModuleParent")]
        [HttpGet]
        [Authorize]
        public async Task<object> GetModuleParent()
        {
            var obj = _accountRepository.GetTblMenuParent(true);
            _accountRepository.SetStringCache(AccountConstant.GetTblMenuParent, obj);
            return await _accountRepository.GetStringAsync(AccountConstant.GetTblMenuParent);
        }

        /// <summary>
        /// Function using for get list department
        /// CreateBy: HaiHM
        /// CreatedDate: 26/04/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetDepartment")]
        [HttpGet]
        [Authorize]
        public object GetDepartment()
        {
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(orgCode))
            {
                return Json(_accountRepository.GetDepartment(orgCode));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Function Decode token
        /// CreateBy: HaiHM
        /// CreatedDate: 26/04/2019
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private TblUsers DecodeTokenUser(string token)
        {
            if (token != null)
            {
                token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                try
                {
                    var arr = new JwtSecurityToken(token);
                    string userName = arr.Subject;
                    TblUsers user = _accountRepository.GetUsers(userName);
                    if (user != null)
                    {
                        return user;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Function upload Avatar
        /// CreateBy: HaiHM
        /// CreatedDate: 26/04/2019
        /// </summary>
        /// <param name="files">file</param>
        /// <returns></returns>
        [Route("~/api/Account/UploadFile")]
        [HttpPost]
        [Authorize]
        public IActionResult UploadFile(IFormFile files)
        {
            bool checkExtentionFile = false;
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            //JPG, PNG
            string fileName = accountCommon.ConvertStringToVNChar(files.FileName.Replace(" ", string.Empty));

            if (accountCommon.ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant()))
            {
                checkExtentionFile = true;
            }

            if (orgCode != null && checkExtentionFile)
            {
                if (files.Length > AccountConstant.MaximumFile || files.Length <= 0)
                {
                    rm.Type = AccountConstant.TypeUser;
                    rm.Title = AccountConstant.TitleMaximumFile;
                    rm.Message = AccountConstant.MaximumFileOver;
                    rm.Status = AccountConstant.statusError;
                    var field = new { fieldErrors = rm.Title };

                    return StatusCode(400, Json(rm));
                }
                else
                {
                    try
                    {
                        string str = null;
                        string username = User.Claims.FirstOrDefault().Value;
                        string fileNameCustom = username + Path.GetExtension(fileName);
                        if ((System.IO.File.Exists(_environment.WebRootPath + AccountConstant.DirectoryUploads + orgCode + AccountConstant.DirectorySlat + fileNameCustom)))
                        {
                            System.IO.File.Delete(_environment.WebRootPath + AccountConstant.DirectoryUploads + orgCode + AccountConstant.DirectorySlat + fileNameCustom);
                        }
                        // Get Directory Organization
                        if (!Directory.Exists(_environment.WebRootPath + AccountConstant.DirectoryUploads + orgCode + AccountConstant.DirectorySlat))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + AccountConstant.DirectoryUploads + orgCode + AccountConstant.DirectorySlat);
                        }
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + AccountConstant.DirectoryUploads + orgCode + AccountConstant.DirectorySlat + fileNameCustom))
                        {
                            files.CopyTo(filestream);
                            filestream.Flush();
                            str = AccountConstant.DirectoryUploads + orgCode + AccountConstant.DirectorySlat + fileNameCustom;
                        }
                        if (!string.IsNullOrEmpty(str))
                        {
                            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
                            TblUsers user = _accountRepository.GetUserById(userId);
                            //string base64 = await _accountRepository.ImageToBase64Async(str);
                            string avatar = orgCode + AccountConstant.DirectorySlat + fileNameCustom;
                            user.Avatar = avatar.Replace("\\", "/");
                            _accountRepository.UpdateUser(user, true, false, false, userId);
                            var obj = new { data = avatar };
                            return StatusCode(201, obj);
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(400, ex.Message);
                    }
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Function upload Avatar
        /// CreateBy: HaiHM
        /// CreatedDate: 26/04/2019
        /// </summary>
        /// <param name="files">file</param>
        /// <returns></returns>
        [Route("~/api/Account/DownloadFile")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            bool checkExtentionFile = false;
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            //JPG, PNG
            fileName = accountCommon.ConvertStringToVNChar(fileName.Replace("\\\\", "\\"));

            if (accountCommon.ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant())
                && System.IO.File.Exists(_environment.WebRootPath + fileName))
            {
                checkExtentionFile = true;
            }

            if (checkExtentionFile)
            {
                try
                {
                    var base64 = await _accountRepository.ImageToBase64Async(fileName);
                    var obj = new { data = base64 };
                    return StatusCode(200, obj);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Function Delete Token
        /// CreateBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/DeleteToken")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteToken()
        {
            string token = _contextAccessor.HttpContext.Request.Headers[AccountConstant.Authorization];
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                token = accountCommon.MD5Hash(token);
                string cacheValue = _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                cacheValue += token;
                _accountRepository.SetStringCache(AccountConstant.ListLogoutToken, cacheValue);
                _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);

                object obj = new { message = AccountConstant.MessageDeleteSuccess };
                return StatusCode(200, obj);
            }
            return BadRequest();
        }

        /// <summary>
        /// Function Reset Token
        /// CreateBy: HaiHM
        /// CreatedDate: 18/04/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/ResetToken")]
        [HttpGet]
        [Authorize]
        public IActionResult ResetToken()
        {
            // Destroy old token
            string token = _contextAccessor.HttpContext.Request.Headers[AccountConstant.Authorization];
            if (token != null)
            {
                token = token.Length != 0 ? token.Replace(AccountConstant.BearerReplace, string.Empty) : string.Empty;
                token = accountCommon.MD5Hash(token);
                string cacheValue = _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
                cacheValue += token;
                _accountRepository.SetStringCache(AccountConstant.ListLogoutToken, cacheValue);
                _accountRepository.GetStringCache(AccountConstant.ListLogoutToken);
            }

            string username = User.Claims.FirstOrDefault().Value;
            if (username != null)
            {
                TblUsers userInfo = _accountRepository.GetUsers(username);
                string tokenReset = GenerateJSONWebToken(userInfo);
                var responseNew = new { token = tokenReset };
                return Json(responseNew);
            }
            return BadRequest();
        }

        /// <summary>
        /// Function get all permission of user
        /// CreatedBy: HaiHM
        /// CreatedDate: 13/05/2019
        /// </summary>
        /// <param name="MenuCode"></param>
        /// <returns>list role of user</returns>
        [Route("~/api/Account/GetListPermission")]
        [HttpGet]
        [Authorize]
        public Object JoinRolePermission()
        {
            string userId = User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(userId))
            {
                return Json(_accountRepository.GetAllPermission(Int32.Parse(userId)));
            }
            return BadRequest();
        }

        /// <summary>
        /// Function get list authority by userid
        /// CreatedBy: HaiHM
        /// CreatedDate: 30/5/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Account/GetAuthorityByUserId/{id}")]
        [HttpGet("{id}")]
        [Authorize]
        public Object GetAuthorityByUserId(int id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
                    TblOrganization organization =  _authorityRepository.GetOrganization(orgCode);
                    return Json(_accountRepository.GetAuthorityByUserId(id, organization.OrganizationId));
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
            
        }

        /// <summary>
        /// API get images of organization
        /// CreatedBy: DaiBH
        /// CreatedDate: 04/06/2019
        /// ModifiedBy: HaiHM
        /// ModifiedDate: 06/06/2019
        /// ModifiedContent: Remove async
        /// </summary>
        /// <param name="organizationCode">organizationCode</param>
        /// <param name="imageFile">imageFile</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("~/api/Account/Images/{organizationCode}/{imageFile}")]
        [HttpGet("{organizationCode}/{imageFile}")]
        public ActionResult GetResourceImage(string organizationCode, string imageFile)
        {
            try
            {
                string[] imagePart = imageFile.Split(".");
                // check for image file has extension?
                if (imagePart.Length < 2)
                {
                    return NotFound();
                }
                // read file from resource
                byte[] data = _accountRepository.GetImageAsync(organizationCode, imageFile);
                MemoryStream stream = new MemoryStream(data);
                // process to return file with content type is image media
                string contentType = "image/png";
                switch (imagePart.Last().ToUpper())
                {
                    case "JPG":
                    case "JPEG":
                        contentType = "image/jpeg";
                        break;
                }
                return new FileStreamResult(stream, contentType);
            } catch (Exception ex)
            {
                // throw not found resource when requested file is not existing on systems
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion
    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }

}