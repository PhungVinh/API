using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OrganizationManagement.Constant;
using OrganizationManagement.DataAccess;
using OrganizationManagement.Repositories;
using OrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VinhDemo.Common;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VinhDemo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class OrganizationController : Controller
    {
        private IOrganizationRepository _iOrganizationRepository;

        public OrganizationController(IOrganizationRepository iOrganizationRepository)
        {
            _iOrganizationRepository = iOrganizationRepository;
        }

        #region list Organization @vinhnp
        /// <summary>
        /// get list Organization
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="TextSearch"></param>
        /// <param name="IsActive"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/GetOrganization")]
        [HttpGet]
        [Authorize]
        public object GetOrganization(string DateFrom, string DateTo, string TextSearch, int IsActive, int currPage, int recordperpage)
        {
            return _iOrganizationRepository.GetOrganization();
        }

        #endregion

        #region list Category df @vinhnp
        /// <summary>
        /// get list Category
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="TextSearch"></param>
        /// <param name="IsActive"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/GetOrganizationList")]
        [HttpGet]
        [Authorize]
        public object GetOrganizationList(string DateFrom, string DateTo, string TextSearch, int IsActive, int currPage, int recordperpage)
        {
            string arr = DateFrom + OrganizationConstant.StringSlipSearch
                         + DateTo + OrganizationConstant.StringSlipSearch
                        + TextSearch + OrganizationConstant.StringSlipSearch
                      + IsActive + OrganizationConstant.StringSlipSearch
                      + currPage + OrganizationConstant.StringSlipSearch
                      + recordperpage;

            return _iOrganizationRepository.GetOrganizationList(arr) ;

        }
        #endregion

        #region list Category Position @vinhnp
        /// <summary>
        /// get list Category
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="TextSearch"></param>
        /// <param name="IsActive"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/GetPosition")]
        [HttpGet]
        [Authorize]
        public object GetPosition(string DateFrom, string DateTo, string TextSearch, int IsActive, int currPage, int recordperpage)
        {
            string arr = DateFrom + OrganizationConstant.StringSlipSearch
                         + DateTo + OrganizationConstant.StringSlipSearch
                        + TextSearch + OrganizationConstant.StringSlipSearch
                      + IsActive + OrganizationConstant.StringSlipSearch
                      + currPage + OrganizationConstant.StringSlipSearch
                      + recordperpage;

            return _iOrganizationRepository.GetPosition(arr);

        }
        #endregion


        #region Get list log user @vinhnp
        /// <summary>
        /// GetListLogUser
        /// </summary>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/GetListLogUser")]
        [HttpGet]
        [Authorize]
        public object GetListLogUser ()
        {
            return _iOrganizationRepository.GetListLogUser();
        }


        #endregion

        //Vinhnp
        #region AddlogUser @vinhnp
        /// <summary>
        /// AddlogUser
        /// </summary>
        /// <param name="logU"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/AddlogUser")]
        [HttpPost]
        [Authorize]
        public IActionResult AddlogUser(TblLogUser logU)
        {
            int code = _iOrganizationRepository.AddlogUser(logU);
            if(code == 2)
            {
                return StatusCode(200, "Add OK");
            }
            else
            {
                return StatusCode(400, "Err");
            }
        }
        #endregion

        #region UpdatelogUser @vinhnp
        /// <summary>
        /// UpdatelogUser
        /// </summary>
        /// <param name="logU"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/UpdatelogUser")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdatelogUser (TblLogUser logU)
        {
            int code = _iOrganizationRepository.UpdateLogUser(logU);
            if (code == 2)
            {
                return StatusCode(200, "OK");
            }
            else
            {
                return StatusCode(400, "Err");
            }
        }
        #endregion

        #region DeletelogUser @vinhnp
        /// <summary>
        /// DeletelogUser
        /// </summary>
        /// <param name="logU"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/DeletelogUser")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeletelogUser (int id)
        {
            int code = _iOrganizationRepository.DeleteLogUser(id);
            if(code==2)
            {
                return StatusCode(200, "Delete ok");
            }
            else
            {
                return StatusCode(400, "Err");
            }
        }
        #endregion
        
        #region List TblLogUser theo parameter @vinhnp
        /// <summary> 
        /// GetLogUserList to OrganizationCode
        /// </summary>
        /// <param name="OrganizationCode"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/GetLogUserList")]
        [HttpGet]
        [Authorize]
        public object GetLogUserList (string OrganizationCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(OrganizationCode))
                {
                    OrganizationCode = OrganizationCode.Trim();
                }
                return _iOrganizationRepository.GetLogUserList(OrganizationCode);
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
        /// SearchLogUser 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/SearchLogUser")]
        [HttpGet]
        [Authorize]
        public object SearchLogUser(string textSearch, int currPage, int recordperpage)
        {
            var data = _iOrganizationRepository.SearchLogUser(textSearch, currPage, recordperpage);
            return data;
        }
        #endregion

        #region SearchCategory phân trang @vinhnp
        /// <summary>
        /// SearchCategory
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <returns></returns>
        [Route("~/api/OrganizationVinh/SearchCategory")]
        [HttpGet]
        [Authorize]
        public object SearchCategory(string textSearch, int currPage, int recordperpage)
        {
            var data = _iOrganizationRepository.SearchCategory(textSearch, currPage, recordperpage);
            return data;
        }
        #endregion
    }
}
