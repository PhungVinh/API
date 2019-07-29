using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AttributesManagement.Common;
using AttributesManagement.Constant;
using AttributesManagement.Models;
using AttributesManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AttributesManagement.Controllers
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
            string token = contextAccessor.HttpContext.Request.Headers[AttributeConstant.Authorization];
            token = token.Length != 0 ? token.Replace(AttributeConstant.BearerReplace, string.Empty) : string.Empty;
            var arr = new JwtSecurityToken(token);
            string orgCode = arr.Claims.ToList()[2].Value;// + AttributeConstant.ConnectionAdd;
            _organizationCode = orgCode;
            _categoryRepository = categoryRepository;
            _categoryRepository.LoadContext(orgCode, distributedCache);
        }
        /// <summary>
        /// Function using for search Category
        /// </summary>
        /// <param name="search">Full text search</param>
        /// <param name="index">page now</param>
        /// <param name="size">size of Page</param>
        /// <param name="sortType">Type sort</param>
        /// <returns></returns>
        [Route("~/api/Category/GetAllCategory")]
        [HttpGet]
        [Authorize]
        public object GetAllCategory(string TextSearch, int currPage, int recodperpage)
        {
           return new JsonResult(_categoryRepository.GetAllCategory(TextSearch, currPage, recodperpage));
        }
        /// <summary>
        /// add new category
        /// </summary>
        /// <param name="tblCategory"></param>
        /// <returns></returns>
        [Route("~/api/Category/AddCategory")]
        [HttpPost]
        [Authorize]
        public object AddCategory([FromBody] CategoryChildren tblCategory)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            object objCate = new TblCategory();
            string userName = User.Claims.FirstOrDefault().Value;
            tblCategory.CreateBy = userName;
            if(string.IsNullOrEmpty(tblCategory.CategoryName))
            {
                List<FieldErrors> lsterror = new List<FieldErrors>();
                if (string.IsNullOrEmpty(tblCategory.CategoryName))
                {
                    error.objectName = CategoryConstant.TypeCategory;
                    error.field = CategoryConstant.entityName;
                    error.message = CategoryConstant.Message;
                    lsterror.Add(error);
                }
                rm.Title = CategoryConstant.Title;
                rm.Message = CategoryConstant.MessageError;
                rm.Status = CategoryConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object objCategory = _categoryRepository.AddCategory(tblCategory);
            int code = Convert.ToInt32(objCategory.GetType().GetProperty(CategoryConstant.Code).GetValue(objCategory, null));
            string CateCode = objCategory.GetType().GetProperty(CategoryConstant.CategoryCode).GetValue(objCategory, null).ToString();
          
            
            if (code == 0)
            {
                response.title = CategoryConstant.titleDuplicate;
                response.entityName = CategoryConstant.entityName;
                response.errorKey = CategoryConstant.errorKey;
                response.status = CategoryConstant.statusError;
                response.message = CategoryConstant.MessageDulicateCategoryName;
                return StatusCode(400, response);
            }
            else if(code == 1)
            {
                if(!string.IsNullOrEmpty(CateCode))
                {
                    objCate = _categoryRepository.GetObjectCategory(CateCode);
                }
                var responeNew = new { TblCategory = objCate };
                return StatusCode(201, responeNew);
            }
            else
            {
                return StatusCode(400);
            }
        }
        /// <summary>
        /// edit category
        /// </summary>
        /// <param name="tblCategory"></param>
        /// <returns></returns>
        [Route("~/api/Category/UpdateCategory")]
        [HttpPut]
        [Authorize]
        public object UpdateCategory([FromBody] CategoryChildren tblCategory)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            object objCate = new TblCategory();
            string userName = User.Claims.FirstOrDefault().Value;
            tblCategory.UpdateBy = userName;
            tblCategory.CreateBy = userName;
            if (string.IsNullOrEmpty(tblCategory.CategoryName))
            {
                List<FieldErrors> lsterror = new List<FieldErrors>();
                if (string.IsNullOrEmpty(tblCategory.CategoryName))
                {
                    error.objectName = CategoryConstant.TypeCategory;
                    error.field = CategoryConstant.entityName;
                    error.message = CategoryConstant.Message;
                    lsterror.Add(error);
                }
                rm.Title = CategoryConstant.Title;
                rm.Message = CategoryConstant.MessageError;
                rm.Status = CategoryConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object objCategory = _categoryRepository.UpdateCategory(tblCategory);
            int code = Convert.ToInt32(objCategory.GetType().GetProperty(CategoryConstant.Code).GetValue(objCategory, null));
            string CateCode = objCategory.GetType().GetProperty(CategoryConstant.CategoryCode).GetValue(objCategory, null).ToString();
            if (code == 0)
            {
                response.title = CategoryConstant.titleDuplicate;
                response.entityName = CategoryConstant.entityName;
                response.errorKey = CategoryConstant.errorKey;
                response.status = CategoryConstant.statusError;
                response.message = CategoryConstant.MessageDulicateCategoryName;
                return StatusCode(400, response);
            }
            else if (code == 1)
            {
                if (!string.IsNullOrEmpty(CateCode))
                {
                    objCate = _categoryRepository.GetObjectCategory(CateCode);
                }
                var responeNew = new { TblCategory = objCate };
                return StatusCode(200, responeNew);
            }
            else
            {
                return StatusCode(400);
            }
        }
        /// <summary>
        /// Get all Parent Category
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Category/GetAllParentCategory")]
        [HttpGet]
        [Authorize]
        public object GetAllParentCategory()
        {
            return new JsonResult(_categoryRepository.GetAllParentCategory());
        }
        /// <summary>
        /// Get all Parent Category
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Category/GetObjectCategory")]
        [HttpGet]
        [Authorize]
        public object GetObjectCategory(string CategoryCode)
        {
            return new JsonResult(_categoryRepository.GetCategoryByCateCode(CategoryCode));
        }
        /// <summary>
        /// Delete Category
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Category/DeleteCategory")]
        [HttpDelete]
        [Authorize]
        public object DeleteCategory(string CategoryCode)
        {
            ErrorObject response = new ErrorObject();
            object objCate = new TblCategory();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object deleteCateCode = _categoryRepository.DeleteCategory(CategoryCode);
            int code = Convert.ToInt32(deleteCateCode.GetType().GetProperty(CategoryConstant.Code).GetValue(deleteCateCode, null));
            string CateCode = deleteCateCode.GetType().GetProperty(CategoryConstant.CategoryCode).GetValue(deleteCateCode, null).ToString();
            if (code == 1)
            {
                if(CateCode !="")
                {
                    objCate = _categoryRepository.GetObjectCategory(CateCode);
                }
                var responeNew = new { TblCategory = CategoryCode };
                return StatusCode(200, responeNew);
            }
            else if (code == 0)
            {
                response.title = CategoryConstant.titleDelete;
                response.entityName = CategoryConstant.entityNameDelete;
                response.errorKey = CategoryConstant.errorKeyDelete;
                response.status = CategoryConstant.statusError;
                response.message = CategoryConstant.MessageDelete;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }
        /// <summary>
        /// Check categoryCode be using
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        [Route("~/api/Category/checkCategoryCode")]
        [HttpGet]
        [Authorize]
        public object checkCategoryCode(string categoryCode)
        {
            ErrorObject response = new ErrorObject();
            object objCate = new TblCategory();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object deleteCateCode = _categoryRepository.checkCategoryCode(categoryCode);
            int code = Convert.ToInt32(deleteCateCode.GetType().GetProperty(CategoryConstant.Code).GetValue(deleteCateCode, null));
            string CateCode = deleteCateCode.GetType().GetProperty(CategoryConstant.CategoryCode).GetValue(deleteCateCode, null).ToString();
            if (code == 1)
            {
                response.message = CategoryConstant.MessageValidate;
                return StatusCode(200, response);
            }
            else if (code == 0)
            {
                response.title = CategoryConstant.titleDelete;
                response.entityName = CategoryConstant.entityNameDelete;
                response.errorKey = CategoryConstant.errorKeyDelete;
                response.status = CategoryConstant.statusError;
                response.message = CategoryConstant.MessageDelete;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }

    }
}