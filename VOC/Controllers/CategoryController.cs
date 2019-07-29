
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using VOC.Constant;
using VOC.Repositories;

namespace VOC.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IcategoryRepository _categoryRepository;
        private string _organizationCode;

        public CategoryController(IHttpContextAccessor contextAccessor, IcategoryRepository categoryRepository, IDistributedCache distributedCache)
        {
            string token = contextAccessor.HttpContext.Request.Headers[VOCConstant.Authorization];
            token = token.Length != 0 ? token.Replace(VOCConstant.BearerReplace, string.Empty) : string.Empty;
            var arr = new JwtSecurityToken(token);
            string orgCode = arr.Claims.ToList()[2].Value;// + AttributeConstant.ConnectionAdd;
            _organizationCode = orgCode;
            _categoryRepository = categoryRepository;
            _categoryRepository.LoadContext(orgCode, distributedCache);
        }

        [Route("~/api/VOC/GetAllCategory")]
        [HttpGet]
        [Authorize]
        public object GetAllCategory(string TextSearch, int currPage, int recodperpage)
        {
            return new JsonResult(_categoryRepository.GetAllCategory(TextSearch, currPage, recodperpage));
        }
    }
}
