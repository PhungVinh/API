using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Encryption.Common;
using Encryption.Constant;
using Encryption.DTO;
using Encryption.Models;
using Encryption.Repositories;
using Encryption.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Quartz;

namespace Encryption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = EncryptionConstant.SuperAdmin)]
    public class EncryptionController : ControllerBase
    {
        private readonly IEncryptionRepository encryptionRepository;
        private readonly IDistributedCache distributedCache;
        //private readonly string orgCode;
        public EncryptionController(IHttpContextAccessor contextAccessor, IEncryptionRepository encryptionRepository, IDistributedCache distributedCache)
        {
            //orgCode = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == EncryptionConstant.OrgCode).Value;
            //encryptionRepository.LoadContext(orgCode, distributedCache);
            this.encryptionRepository = encryptionRepository;
        }

        #region vudt
        [Route(EncryptionConstant.API_GetListAttributesEncryption)]
        [HttpGet]
        public async Task<IActionResult> GetListAttributesEncryption(string orgCode)
        {
            encryptionRepository.LoadContext(orgCode, distributedCache);
            var result = await encryptionRepository.GetListAttributesEncryption(orgCode);
            return StatusCode(result.StatusCode, result.Response);
        }
        [Route(EncryptionConstant.API_ExecuteEncrpytion)]
        [HttpPost]
        public IActionResult ExecuteEncrpytion([FromBody] CRUDAttributeDTO lstAttributes)
        {
            if (ModelState.IsValid)
            {
                encryptionRepository.LoadContext(lstAttributes.OrgCode, distributedCache);
                var model = new AttributeModel();
                model.tblVocattributes = lstAttributes.Attributes;
                model.UserName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = encryptionRepository.UpdateEncrpytion(model, lstAttributes.OrgCode);
                return StatusCode(result.StatusCode, new { data = result.Response });
            }
            return StatusCode(400);
        }
        [Route(EncryptionConstant.API_GetListAttributesWithParentCode)]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListAttributesWithParentCode(string parentCode, string orgCode)
        {
            encryptionRepository.LoadContext(orgCode, distributedCache);
            var result = await encryptionRepository.GetListAttributesWithParentCode(parentCode, orgCode);
            return StatusCode(result.StatusCode, result.Response);
        }
        [Route(EncryptionConstant.API_GetListModule)]
        [HttpGet]
        public object GetListModule()
        {
            var result = encryptionRepository.GetListModule(EncryptionConstant.MASTER);
            return StatusCode(result.StatusCode, result.Response);
        }
        #endregion
    }
}