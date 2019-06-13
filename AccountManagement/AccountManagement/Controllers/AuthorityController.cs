using System;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.Models;
using AccountManagement.Repositories;
using AccountManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorityController : Controller
    {
        private IConfiguration _config;
        private IAuthorityRepository _authorityRepository;

        public AuthorityController(IConfiguration config, IAuthorityRepository authorityRepository)
        {
            _config = config;
            _authorityRepository = authorityRepository;
        }

        /// <summary>
        /// Function get list anh search authority with Pagging
        /// CreatedBy: HaiHm
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <param name="textSearch">target search</param>
        /// <param name="currPage">currPage</param>
        /// <param name="Record">Record</param>
        /// <returns></returns>
        [Route("~/api/Authority/SearchAuthority")]
        [HttpGet]
        [Authorize]
        public object GetAuthorityList(string textSearch, int currPage, int Record)
        {
            // GetOrg
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            TblOrganization org = new TblOrganization();
            if (!string.IsNullOrEmpty(orgCode))
            {
                org = _authorityRepository.GetOrganization(orgCode);
            }
            // GetUserId
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == AccountConstant.userId).FirstOrDefault().Value);
            if (!string.IsNullOrEmpty(textSearch))
            {
                textSearch = textSearch.TrimEnd().TrimStart();
            }
            
            string arr = textSearch + AccountConstant.StringSlipSearch
                   + currPage + AccountConstant.StringSlipSearch
                   + Record + AccountConstant.StringSlipSearch
                   + userId + AccountConstant.StringSlipSearch
                   + org.OrganizationId
                   ;
            return Json(_authorityRepository.GetAuthorityList(arr));
        }

        /// <summary>
        /// Function get list menu + role when create new authority
        /// CreatedBy: HaiHM
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Authority/SearchMenuAndRole")]
        [HttpGet]
        [Authorize]
        public object GetRoleList(string ParentCode)
        {
            var data = _authorityRepository.SearchMenuAndRole(ParentCode);
            return Json(data);
        }

        /// <summary>
        /// Function get list authority of org
        /// CreatedBy: HaiHM
        /// CreatedDate: 31/5/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Authority/GetListAuthorityOfOrg")]
        [HttpGet]
        [Authorize]
        public object GetListAuthorityOfOrg()
        {
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            return _authorityRepository.GetListAuthorityOfOrg(orgCode);
        }

        /// <summary>
        /// Funciton Create authority with role 
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="model">object has authority and role(menu)</param>
        /// <returns></returns>
        [Route("~/api/Authority/CreateAuthority")]
        [HttpPost]
        [Authorize]
        public IActionResult CreateAuthority([FromBody] AuthorityRoleViewModel model)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.tblAuthority == null)
            {
                rm.Type = AuthorityConstant.TypeAuthority;
                rm.Title = AuthorityConstant.TitleAddAuthority;
                rm.Message = AuthorityConstant.MessageAddAuthority;
                rm.Status = AuthorityConstant.statusError;
                var field = new { fieldErrors = rm.Title };
                
                return StatusCode(400, Json(rm));

            }
            else
            {
                string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
                TblOrganization org = _authorityRepository.GetOrganization(orgCode);
                if (org != null)
                {
                    model.tblAuthority.OrganizationId = org.OrganizationId;
                    model.tblAuthority.CreateBy = User.Claims.FirstOrDefault().Value;
                    if (String.IsNullOrEmpty(model.tblAuthority.OrganizationId.ToString()) || model.tblAuthority.OrganizationId == 0)
                    {
                        rm.Type = AuthorityConstant.TypeAuthority;
                        rm.Title = AuthorityConstant.TitleAddAuthorityNullOrg;
                        rm.Message = AuthorityConstant.MessageAddAuthorityNullOrg;
                        rm.Status = AuthorityConstant.statusError;
                        var field = new { fieldErrors = rm.Title };
                        
                        return StatusCode(400, Json(rm));
                    }
                    else
                    {
                        TblAuthority au = _authorityRepository.CreateAuthority(model);
                        if (au == null)
                        {
                            response.entityName = AuthorityConstant.TypeAuthority;
                            response.errorKey = AuthorityConstant.ErrorKeyAddError;
                            response.status = AuthorityConstant.statusError;
                            response.message = AuthorityConstant.MessageAddError;
                            return StatusCode(400, Json(response));
                        }
                        return StatusCode(201, Json(au));
                    }
                }
                return BadRequest();
            }
        }

        /// <summary>
        /// Function Update authority (update authority with role)
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="model">object has authority and role(menu)</param>
        /// <returns></returns>
        [Route("~/api/Authority/UpdateAuthority")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateAuthority([FromBody] AuthorityRoleViewModel model)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.tblAuthority == null)
            {
                rm.Type = AuthorityConstant.TypeAuthority;
                rm.Title = AuthorityConstant.TitleAddAuthority;
                rm.Message = AuthorityConstant.MessageAddAuthority;
                rm.Status = AuthorityConstant.statusError;
                var field = new { fieldErrors = rm.Title };
                
                return StatusCode(400, Json(rm));
            }
            else
            {
                string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
                TblOrganization org = _authorityRepository.GetOrganization(orgCode);
                if (org != null)
                {
                    model.tblAuthority.OrganizationId = org.OrganizationId;
                    if (String.IsNullOrEmpty(model.tblAuthority.OrganizationId.ToString()) || model.tblAuthority.OrganizationId == 0)
                    {
                        rm.Type = AuthorityConstant.TypeAuthority;
                        rm.Title = AuthorityConstant.TitleAddAuthorityNullOrg;
                        rm.Message = AuthorityConstant.MessageAddAuthorityNullOrg;
                        rm.Status = AuthorityConstant.statusError;
                        var field = new { fieldErrors = rm.Title };
                        
                        return StatusCode(400, Json(rm));
                    }
                    else
                    {
                        model.tblAuthority.UpdateBy = User.Claims.FirstOrDefault().Value;
                        TblAuthority au = _authorityRepository.UpdateAuthority(model);
                        if (au == null)
                        {
                            response.entityName = AuthorityConstant.TypeAuthority;
                            response.errorKey = AuthorityConstant.ErrorKeyAddError;
                            response.status = AuthorityConstant.statusError;
                            response.message = AuthorityConstant.MessageAddError;
                            return StatusCode(400, Json(response));
                        }
                        return StatusCode(200, Json(au));
                    }
                }
                return BadRequest();
            }

        }

        /// <summary>
        /// Function Delete authority by id
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="authorityId">id of authority</param>
        /// <returns> code = 1 : Success; code = 0 : Fail</returns>
        [Route("~/api/Authority/DeleteAuthority/{authorityId}")]
        [HttpDelete("{authorityId}")]
        [Authorize]
        public IActionResult DeleteAuthority(int authorityId)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (string.IsNullOrEmpty(authorityId.ToString()) || authorityId == 0)
            {
                rm.Type = AuthorityConstant.TypeAuthority;
                rm.Title = AuthorityConstant.TitleDeleteAuthority;
                rm.Message = AuthorityConstant.MessageDeleteAuthority;
                rm.Status = AuthorityConstant.statusError;
                var field = new { fieldErrors = rm.Title };
                

                return StatusCode(400, Json(rm));
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool delete = _authorityRepository.DeleteAuthority(authorityId);
            if (delete)
            {
                object obj = new { message = AuthorityConstant.DeleteSuccess };
                return StatusCode(200, obj);
            }
            else
            {
                response.entityName = AuthorityConstant.TypeAuthority;
                response.errorKey = AuthorityConstant.MessageDeleteAuthority;
                response.title = AuthorityConstant.TitleDeleteAuthority;
                response.status = AuthorityConstant.statusError;
                response.message = AuthorityConstant.MessageDeleteError;
                return StatusCode(400, Json(response));
            }
        }

        /// <summary>
        /// Function Copy 1 authority with role(menu)
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019 
        /// </summary>
        /// <param name="authorityId">id of authority</param>
        /// <returns>Authority with role of authority and Role not of authority</returns>
        [Route("~/api/Authority/CopyAuthority/{authorityId}")]
        [HttpGet("{authorityId}")]
        [Authorize]
        public IActionResult CopyAuthority(int authorityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            if (string.IsNullOrEmpty(authorityId.ToString()) || authorityId == 0)
            {
                rm.Type = AuthorityConstant.TypeAuthority;
                rm.Title = AuthorityConstant.TitleNullAuthorityId;
                rm.Message = AuthorityConstant.MessageNullAuthority;
                rm.Status = AuthorityConstant.statusError;
                var field = new { fieldErrors = rm.Title };
                

                return StatusCode(400, Json(rm));
            }
            return Json(_authorityRepository.CopyAuthority(authorityId));
        }

        /// <summary>
        /// Function Check duplicate name authority
        /// CreatedBy: HaiHM
        /// CreatedDate: 25/5/2019
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("~/api/Authority/CheckDupicate")]
        [HttpGet]
        [Authorize]
        public object CheckDupicate(int authorityId, string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string orgCode = User.Claims.Where(u => u.Type == AuthorityConstant.orgCode).FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(name))
            {
                if (_authorityRepository.CheckDuplicate(authorityId, name, orgCode))
                {
                    var obj = new { existed = false, message = AuthorityConstant.CheckDupicateOK };
                    return StatusCode(200, obj);
                }
                else
                {
                    var obj = new { existed = true, message = AuthorityConstant.MessageAddError };
                    return StatusCode(200, obj);
                }

            }
            else
            {
                var obj = new { existed = false, message = AuthorityConstant.CheckDupicateOK };
                return StatusCode(200, obj);
            }
        }

        #region vudt
        /// <summary>
        /// CreatedBy: VuDT
        /// Modified: HaiHM
        /// Content Modified: 
        /// Change Authorize(Roles = "Quản trị hệ thống") To  [Authorize]
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Authority/GetModuleList")]
        [HttpGet]
        [Authorize]
        public IActionResult GetModuleList()
        {
            var userId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == AccountConstant.userId).Value);
            return Json(_authorityRepository.GetModuleList(userId));
        }
        [Route("~/api/Authority/GrantAuthority")]
        [HttpPost]
        [Authorize]
        public IActionResult GrantAuthority(UserGrantDTO model)
        {
            return Ok( new { message = _authorityRepository.GrantAuthority(model.Users, model.AuthorityId) });
        }

        [Route("~/api/Authority/GetUsersToGrantAuthority/{authorityId}")]
        [HttpGet("{authorityId}")]
        [Authorize]
        public IActionResult GetUsersToGrantAuthority(int authorityId)
        {
            var userId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == AccountConstant.userId).Value);
            var orgCode = User.Claims.FirstOrDefault(x => x.Type == AuthorityConstant.orgCode).Value;
            return Json(_authorityRepository.GetUsersToGrantAuthority(authorityId, userId, orgCode));
        }

        [Route("~/api/Authority/AuthorityInformation")]
        [HttpGet]
        [Authorize]
        public IActionResult AuthorityInformation()
        {
            var userId = Int16.Parse(User.Claims.FirstOrDefault(x => x.Type == AccountConstant.userId).Value);            
            return Json(_authorityRepository.AuthorityInformation(userId));
        }
        #endregion
    }
}