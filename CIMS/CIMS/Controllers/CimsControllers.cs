using CIMS.Constant;
using CIMS.DTO;
using CIMS.Models;
using CIMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CIMS.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]



    public class CimsController : ControllerBase
    {


        private CimsRepository _cimsRepository;
        private string _organizationCode;
        public CimsController(IHttpContextAccessor contextAccessor, CimsRepository cimsRepository, IDistributedCache distributedCache)
        {
            string token = contextAccessor.HttpContext.Request.Headers[CimsConstant.Authorization];
            token = token.Length != 0 ? token.Replace(CimsConstant.BearerReplace, string.Empty) : string.Empty;
            var arr = new JwtSecurityToken(token);
            string orgCode = arr.Claims.ToList()[2].Value; //+ CimsConstant.ConnectionAdd;
            _organizationCode = orgCode;
            cimsRepository.LoadContext(orgCode, distributedCache);
            _cimsRepository = cimsRepository;
        }


        /// <summary>
        /// Get thong tin 1 khach hang
        /// </summary>
        /// <param name="RecordId"></param>

        /// <returns></returns>
        [Route("~/api/Cims/GetCustomerList_RecordId")]
        [HttpGet]
        [Authorize]
        public object GetCustomerList_RecordId(string RecordId)
        {
            return _cimsRepository.GetCustomerList_RecordId(RecordId);
        }
        
        /// <summary>
        /// Get danh sach CIMS
        /// </summary>
        /// <param name="ModuleParent"></param>
        /// <param name="currPage"></param>
        /// <param name="recodperpage"></param>
        /// <returns></returns>
        [Route("~/api/Cims/GetCimsvalue")]
        [HttpGet]
        [Authorize]
        public object GetCimsvalue(string ModuleParent, int currPage, int recodperpage)
        {
            return _cimsRepository.GetCimsvalue(ModuleParent, currPage, recodperpage);
        }


        [Route("~/api/Cims/DeleteCims")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteCims(string RecordId)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _cimsRepository.DeleteCims(RecordId);
            if (code == 1)
            {
                var obj = new { message = "Xóa thành công !" };
                return StatusCode(201, obj);
            }
            else
            {
                if (code == 0)
                {
                    var obj = new { message = CimsConstant.NoDelete };
                    return StatusCode(400, obj);
                }
                else
                {
                    var obj = new { message = CimsConstant.ErrDelete };
                    return StatusCode(400, obj);
                }

            }
            
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstCustomer"></param>
        /// <returns></returns>
        [Route("~/api/Cims/EditCimsValue")]
        [HttpPut]
        [Authorize]
        public IActionResult EditCimsValue([FromBody]List<TblCimsattributeValue> lstCustomer)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var recordId = lstCustomer.Find(attr => attr.AttributeCode == "RecordId");
            if (recordId == null)
            {
                return BadRequest(new
                {
                    field = "RecordId",
                    name = "RecordId",
                    message = "NotExist"
                });
            }
            lstCustomer.Remove(recordId);
            object code = _cimsRepository.EditCimsValue(lstCustomer, recordId.AttributeValue);
            if (code.GetType() == typeof(List<object>))
            {
                return BadRequest(code);
            }
            return Ok();
        }


        /// <summary>
        /// Add AddCimsValue
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Cims/AddCimsValue")]
        [HttpPost]
        [Authorize]
        public IActionResult AddCimsValue([FromBody] List<TblCimsattributeValue> cims)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object code = _cimsRepository.AddCimsValue(cims);
            if (code.GetType() == typeof(List<object>))
            {
                return BadRequest(code);
            }
            return Ok();
        }
        #region vudt
        [Route("~/api/Cims/GetListAttributesEncryption")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListAttributesEncryption(string parentCode)
        {
            var model = await _cimsRepository.GetListAttributesEncryption(parentCode);
            return StatusCode(model.StatusCode, model.Response);
        }
        [Route("~/api/Cims/ExecuteEncrpytion")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ExecuteEncrpytion([FromBody] List<TblVocattributes> lstAttributes)
        {
            var model = new AttributesDTO();
            model.tblVocattributes = lstAttributes;
            model.UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _cimsRepository.ExecuteEncrpytion(model);
            return StatusCode(result.StatusCode, result.Response);
        }


        #endregion


    }

}




