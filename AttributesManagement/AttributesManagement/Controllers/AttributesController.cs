using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AttributesManagement.Models;
using AttributesManagement.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AttributesManagement.DataAccess;
using AttributesManagement.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using AttributesManagement.Common;

namespace AttributesManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private IAttributeRepository _attributeRepository;
        private string _organizationCode;
        public AttributesController(IHttpContextAccessor contextAccessor, IAttributeRepository attributeRepository, IDistributedCache distributedCache)
        {
            string token = contextAccessor.HttpContext.Request.Headers[AttributeConstant.Authorization];
            token = token.Length != 0 ? token.Replace(AttributeConstant.BearerReplace, string.Empty) : string.Empty;
            var arr = new JwtSecurityToken(token);
            string orgCode = arr.Claims.ToList()[2].Value;// + AttributeConstant.ConnectionAdd;
            _organizationCode = orgCode;
            attributeRepository.LoadContext(orgCode, distributedCache);
            _attributeRepository = attributeRepository;       
        }

        /// <summary>
        /// Add Attributes
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="Vocattributes"></param>
        /// <returns></returns>
        [Route("~/api/Attributes/AddAttribute")]
        [HttpPost]
        [Authorize]
        public IActionResult AddAttribute([FromBody] InfoAttribute Vocattributes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            object attributes = new TblVocattributes();
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            string userName = User.Claims.FirstOrDefault().Value;
            Vocattributes.CreateBy = userName;

            if (string.IsNullOrEmpty(Vocattributes.AttributeLabel) || string.IsNullOrEmpty(Vocattributes.AttributeType) || string.IsNullOrEmpty(Vocattributes.DataType))
            {
                List<FieldErrors> lsterror = new List<FieldErrors>();
                if(string.IsNullOrEmpty(Vocattributes.AttributeLabel))
                {
                    error.objectName = AttributeConstant.TypeAttribute;
                    error.field = AttributeConstant.entityName;
                    error.message = AttributeConstant.Message;
                    lsterror.Add(error);
                }
                if(string.IsNullOrEmpty(Vocattributes.AttributeType))
                {
                    error.objectName = AttributeConstant.TypeAttribute;
                    error.field = AttributeConstant.entityType;
                    error.message = AttributeConstant.Message;
                    lsterror.Add(error);
                }
                if(string.IsNullOrEmpty(Vocattributes.DataType))
                {
                    error.objectName = AttributeConstant.TypeAttribute;
                    error.field = AttributeConstant.entityDataType;
                    error.message = AttributeConstant.Message;
                    lsterror.Add(error);
                }
                rm.Title = AttributeConstant.Title;
                rm.Message = AttributeConstant.MessageError;
                rm.Status = AttributeConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }            
            object objAttributes = _attributeRepository.AddAttribute(Vocattributes);
            int code = Convert.ToInt32(objAttributes.GetType().GetProperty(AttributeConstant.Code).GetValue(objAttributes, null));
            int Id = Convert.ToInt32(objAttributes.GetType().GetProperty(AttributeConstant.Id).GetValue(objAttributes, null));
            if (code == 1)
            {
                if (Id > 0)
                {
                    attributes = _attributeRepository.GetObjectAttributes(Id);
                }
                //_attributeRepository.SetStringCache(_organizationCode + AttributeConstant.Attributes_GetListAttributes, _attributeRepository.GetListAttributes(Vocattributes.ModuleParent));
                //_attributeRepository.GetStringCache(AttributeConstant.Attributes_GetListAttributes);
                var responeNew = new { TblVocattributes = attributes };
                return StatusCode(201, Newtonsoft.Json.JsonConvert.SerializeObject(responeNew));
            }
            else if (code == 0)
            {
                response.title = AttributeConstant.titleDuplicate;
                response.entityName = AttributeConstant.entityName;
                response.errorKey = AttributeConstant.errorKey;
                response.status = AttributeConstant.statusError;
                response.message = AttributeConstant.MessageDulicateAttributename;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Update Attributes
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <param name="Vocattributes"></param>
        /// <returns></returns>
        [Route("~/api/Attributes/UpdateAttribute")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateAttribute([FromBody] InfoAttribute Vocattributes)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();       
            object attributes = new object();
            string userName = User.Claims.FirstOrDefault().Value;
            Vocattributes.UpDateBy = userName;
            if (string.IsNullOrEmpty(Vocattributes.AttributeLabel) || string.IsNullOrEmpty(Vocattributes.AttributeType) || string.IsNullOrEmpty(Vocattributes.DataType))
            {
                List<FieldErrors> lsterror = new List<FieldErrors>();
                if (string.IsNullOrEmpty(Vocattributes.AttributeLabel))
                {
                    error.objectName = AttributeConstant.TypeAttribute;
                    error.field = AttributeConstant.entityName;
                    error.message = AttributeConstant.Message;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(Vocattributes.AttributeType))
                {
                    error.objectName = AttributeConstant.TypeAttribute;
                    error.field = AttributeConstant.entityType;
                    error.message = AttributeConstant.Message;
                    lsterror.Add(error);
                }
                if (string.IsNullOrEmpty(Vocattributes.DataType))
                {
                    error.objectName = AttributeConstant.TypeAttribute;
                    error.field = AttributeConstant.entityDataType;
                    error.message = AttributeConstant.Message;
                    lsterror.Add(error);
                }
                rm.Title = AttributeConstant.Title;
                rm.Message = AttributeConstant.MessageError;
                rm.Status = AttributeConstant.statusError;
                var field = new { fieldErrors = lsterror };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object objAttributes = _attributeRepository.UpdateAttribute(Vocattributes);
            int code = Convert.ToInt32(objAttributes.GetType().GetProperty(AttributeConstant.Code).GetValue(objAttributes, null));
            int Id = Convert.ToInt32(objAttributes.GetType().GetProperty(AttributeConstant.Id).GetValue(objAttributes, null));
            if (code == 1)
            {
                //_attributeRepository.SetStringCache(_organizationCode + AttributeConstant.Attributes_GetListAttributes, _attributeRepository.GetListAttributes(Vocattributes.ModuleParent));
                //_attributeRepository.GetStringCache(AttributeConstant.Attributes_GetListAttributes);

                if (Id >0)
                {
                    attributes = _attributeRepository.GetObjectAttributes(Id);
                }
                var responeNew = new { TblVocattributes = attributes };
                return StatusCode(200, Newtonsoft.Json.JsonConvert.SerializeObject(responeNew));
            }
            else if(code == 0)
            {
                response.title = AttributeConstant.titleDuplicate;
                response.entityName = AttributeConstant.entityName;
                response.errorKey = AttributeConstant.errorKey;
                response.status = AttributeConstant.statusError;
                response.message = AttributeConstant.MessageDulicateAttributename;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// delete attribute
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        [Route("~/api/Attributes/DeleteAttribute")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteAttribute([FromQuery] int attributes)
        {
            ErrorObject response = new ErrorObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _attributeRepository.DeleteAttributes(attributes);
            if(code == 1)
            {
                return StatusCode(200);
            }
            else if(code == 0)
            {
                response.title = AttributeConstant.titleDelete;
                response.entityName = AttributeConstant.entityNameDelete;
                response.errorKey = AttributeConstant.errorKeyDelete;
                response.status = AttributeConstant.statusError;
                response.message = AttributeConstant.MessageDelete;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Get list Attributes
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetListAttributes")]
        [HttpGet]
        [Authorize]
        public object GetListAttributes(string moduleParent)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetListAttributes(moduleParent));
        }
        /// <summary>
        /// Get List Form
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Attributes/GetListFormCims")]
        [HttpGet]
        [Authorize]
        public object GetListForm(string MenuCode)
        {
            return new JsonResult(_attributeRepository.GetlistFormCims(MenuCode));
        }
        /// <summary>
        /// Add  infomation Form
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Attributes/AddFormCims")]
        [HttpPost]
        [Authorize]
        public IActionResult AddFormCims([FromBody] FormOptionValue attributesForm)
        {           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            string userName = User.Claims.FirstOrDefault().Value;
            attributesForm.tblCimsForm.CreateBy = userName;
            attributesForm.tblCimsForm.CreateDate = DateTime.Now;
            var lstAttribute = _attributeRepository.GetAllAttributeRequired(attributesForm.tblCimsForm.MenuCode);
            var requiredAttributes = lstAttribute.Where(x => !attributesForm.tblCimsattributeForm.Where(c => c.AttributeCode == x.AttributeCode).Select(c => c.AttributeCode).Contains(x.AttributeCode)).Select(x => new InfoAttribute()
            {
                AttributeLabel = x.AttributeLabel
            }).ToList();
            if (requiredAttributes.Count > 0)
            {
                return StatusCode(400, AttributesMessages.MS0004);
            }           
            var code = _attributeRepository.AddFormCims(attributesForm);
            if (code == 1)
            {
                return StatusCode(201, AttributesMessages.MS0003);
            }
            else
            {
                return StatusCode(400, AttributesMessages.MS00011);
            }
        }

        /// <summary>
        /// Update Form thông tin
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Attributes/UpdateFormCims")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateFormCims([FromBody] FormOptionValue attributesForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userName = User.Claims.FirstOrDefault().Value;
            attributesForm.tblCimsForm.UpdateBy = userName;
            attributesForm.tblCimsForm.UpdateDate = DateTime.Now;
            //var lstAttribute = _attributeRepository.GetAllAttributeRequired(attributesForm.tblCimsForm.MenuCode);
            //var requiredAttributes = lstAttribute.Where(x => !attributesForm.tblCimsattributeForm.Where(c => c.AttributeCode == x.AttributeCode).Select(c => c.AttributeCode).Contains(x.AttributeCode)).Select(x => new InfoAttribute()
            //{
            //    AttributeLabel = x.AttributeLabel
            //}).ToList();
            //if (requiredAttributes.Count > 0)
            //{
            //    return StatusCode(400, AttributesMessages.MS0004);
            //}
            var code = _attributeRepository.UpdateFormCims(attributesForm);
            if (code == 1)
            {
                return StatusCode(201, AttributesMessages.MS0003);
            }
            else
            {
                return StatusCode(400, AttributesMessages.MS00011);
            }
        }
        [Route("~/api/Attributes/AddFormCimsList")]
        [HttpPost]
        [Authorize]
        public IActionResult AddFormCimsList(UpdateFormDTO attributesForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            string userName = User.Claims.FirstOrDefault().Value;
            attributesForm.tblCimsForm.CreateBy = userName;
            attributesForm.tblCimsForm.CreateDate = DateTime.Now;
            var lstAttribute = _attributeRepository.GetAllAttributeRequired(attributesForm.tblCimsForm.MenuCode);
            var requiredAttributes = lstAttribute.Where(x => !attributesForm.tblCimsattributeForm.Where(c => c.AttributeCode == x.AttributeCode).Select(c => c.AttributeCode).Contains(x.AttributeCode)).Select(x => new InfoAttribute()
            {
                AttributeLabel = x.AttributeLabel
            }).ToList();
            if (requiredAttributes.Count > 0)
            {
                return StatusCode(400, AttributesMessages.MS0004);
            }
            var code = _attributeRepository.AddFormCims(attributesForm);
            if (code > 0)
            {
                return StatusCode(201, AttributesMessages.MS0003);
            }
            else
            {
                return StatusCode(400, AttributesMessages.MS00011);
            }
        }
        /// <summary>
        /// Update Form
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Attributes/UpdateFormCimsList")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateFormCimsList([FromBody] UpdateFormDTO attributesForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userName = User.Claims.FirstOrDefault().Value;
            attributesForm.tblCimsForm.UpdateBy = userName;
            attributesForm.tblCimsForm.UpdateDate = DateTime.Now;
            int code = _attributeRepository.UpdateFormCimsList(attributesForm);
            if (code > 0)
            {
                return StatusCode(200, AttributesMessages.MS0003);
            }
            return StatusCode(400, AttributesMessages.MS00011);
        }
        /// <summary>
        /// Add Attribute Constraint
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Attributes/AddAttributeConstraint")]
        [HttpPost]
        [Authorize]
        public IActionResult AddAttributeConstraint([FromBody] TblAttributeConstraint attributesConstraint)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            TblAttributeConstraint obj = new TblAttributeConstraint();
            string userName = User.Claims.FirstOrDefault().Value;
            attributesConstraint.CreateBy = userName;
            if (string.IsNullOrEmpty(attributesConstraint.Name) || string.IsNullOrEmpty(attributesConstraint.ContraintsType))
            {
                List<FieldErrors> lstError = new List<FieldErrors>();
                if(string.IsNullOrEmpty(attributesConstraint.Name))
                {
                    error.objectName = AttributeConstant.TypeAttributeConstraint;
                    error.field = AttributeConstant.entityNameConstraint;
                    error.message = AttributeConstant.Message;
                    lstError.Add(error);
                }
                if(string.IsNullOrEmpty(attributesConstraint.ContraintsType))
                {
                    error.objectName = AttributeConstant.TypeAttributeConstraint;
                    error.field = AttributeConstant.entityTypeConsatraint;
                    error.message = AttributeConstant.Message;
                    lstError.Add(error);
                }
                rm.Title = AttributeConstant.Title;
                rm.Message = AttributeConstant.MessageError;
                rm.Status = AttributeConstant.statusError;
                var field = new { fieldErrors = lstError };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object attributeConstraint = _attributeRepository.AddAttributeConstraint(attributesConstraint);
            int code = Convert.ToInt32(attributeConstraint.GetType().GetProperty(AttributeConstant.Code).GetValue(attributeConstraint, null));
            int Id = Convert.ToInt32(attributeConstraint.GetType().GetProperty(AttributeConstant.Id).GetValue(attributeConstraint, null));
            if (code ==1)
            {
                if(Id >0)
                {
                   obj = _attributeRepository.GetAttributesConstraintbyId(Id);
                }
                _attributeRepository.SetStringCache(_organizationCode + AttributeConstant.Attributes_GetListConstraint, _attributeRepository.GetAllConstraint(AttributeConstant.StringTextSearch,AttributeConstant.PageCurent,AttributeConstant.Recodperpage));
                _attributeRepository.GetStringCache(AttributeConstant.Attributes_GetListConstraint);
                var responeNew = new { TblAttributeConstraint = obj };
                return StatusCode(201, Newtonsoft.Json.JsonConvert.SerializeObject(responeNew));
            }
            else if(code == 0)
            {
                response.entityName = AttributeConstant.TypeAttributeConstraint;
                response.errorKey = AttributeConstant.errorKey;
                response.status = AttributeConstant.statusError;
                response.message = AttributeConstant.MessageDulicateAttributename;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }

            
        }
        /// <summary>
        /// Update Attribute Constraint
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("~/api/Attributes/UpdateAttributeConstraint")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateAttributeConstraint([FromBody] TblAttributeConstraint updateAttributeConstraint)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            TblAttributeConstraint objAttributeConstraint = new TblAttributeConstraint();
            string userName = User.Claims.FirstOrDefault().Value;
            updateAttributeConstraint.UpdateBy = userName;
            if (string.IsNullOrEmpty(updateAttributeConstraint.Name) || string.IsNullOrEmpty(updateAttributeConstraint.ContraintsType))
            {
                List<FieldErrors> lstError = new List<FieldErrors>();
                if (string.IsNullOrEmpty(updateAttributeConstraint.Name))
                {
                    error.objectName = AttributeConstant.TypeAttributeConstraint;
                    error.field = AttributeConstant.entityNameConstraint;
                    error.message = AttributeConstant.Message;
                    lstError.Add(error);
                }
                if (string.IsNullOrEmpty(updateAttributeConstraint.ContraintsType))
                {
                    error.objectName = AttributeConstant.TypeAttributeConstraint;
                    error.field = AttributeConstant.entityTypeConsatraint;
                    error.message = AttributeConstant.Message;
                    lstError.Add(error);
                }
                rm.Title = AttributeConstant.Title;
                rm.Message = AttributeConstant.MessageError;
                rm.Status = AttributeConstant.statusError;
                var field = new { fieldErrors = lstError };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object obj = _attributeRepository.UpdateAttributeConstraint(updateAttributeConstraint);
            int code = Convert.ToInt32(obj.GetType().GetProperty(AttributeConstant.Code).GetValue(obj, null));
            int Id = Convert.ToInt32(obj.GetType().GetProperty(AttributeConstant.Id).GetValue(obj, null));
            if (code == 1)
            {
                if (Id > 0)
                {
                    objAttributeConstraint = _attributeRepository.GetAttributesConstraintbyId(Id);
                }
                _attributeRepository.SetStringCache(_organizationCode + AttributeConstant.Attributes_GetListConstraint, _attributeRepository.GetAllConstraint(AttributeConstant.StringTextSearch, AttributeConstant.PageCurent, AttributeConstant.Recodperpage));
                _attributeRepository.GetStringCache(AttributeConstant.Attributes_GetListConstraint);
                var responeNew = new { TblAttributeConstraint = objAttributeConstraint };
                return StatusCode(201, Newtonsoft.Json.JsonConvert.SerializeObject(responeNew));
            }
            else if(code == 0)
            {
                response.entityName = AttributeConstant.TypeAttributeConstraint;
                response.errorKey = AttributeConstant.errorKey;
                response.status = AttributeConstant.statusError;
                response.message = AttributeConstant.MessageDulicateAttributename;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }
        /// <summary>
        /// delete attribute Constraint
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("~/api/Attributes/DeleteAttributesConstraint")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteAttributesConstraint([FromQuery] int Id)
        {
            ErrorObject response = new ErrorObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _attributeRepository.DeleteAttributesConstraint(Id);
            if (code == 1)
            {
                return StatusCode(200);
            }
            else if (code == 0)
            {
                response.title = AttributeConstant.titleConstraintDelete;
                response.entityName = AttributeConstant.entityNameConsatraint;
                response.errorKey = AttributeConstant.errorKeyConstraintDelete;
                response.status = AttributeConstant.statusError;
                response.message = AttributeConstant.MessageConstraintDelete;
                return StatusCode(400, response);
            }
            else
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// List type show in Attribute
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetListDataType")]
        [HttpGet]
        [Authorize]
        public object GetListDataType()
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetListDataType()));
        }
        /// <summary>
        /// Get list object in Attribute
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetListController")]
        [HttpGet]
        [Authorize]
        public object GetListController()
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetListController()));
        }
        /// <summary>
        /// Get list object in Constraint
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019 
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetListControllerObject")]
        [HttpGet]
        [Authorize]
        public object GetListControllerObject()
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetListControllerObject()));
        }
        /// <summary>
        /// Get list object in Constraint by categoryCode
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <param name="cateCode"></param>
        /// <returns></returns>
        [Route("~/api/Attributes/GetListConstraintByCateCode")]
        [HttpGet]
        [Authorize]
        public object GetListConstraintByCateCode([FromQuery] string cateCode)
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetListConstraintByCateCode(cateCode)));
        }

        /// <summary>
        /// Function using for search Constraints
        /// </summary>
        /// <param name="search">Full text search</param>
        /// <param name="index">page now</param>
        /// <param name="size">size of Page</param>
        /// <param name="sortType">Type sort</param>
        /// <returns></returns>
        [Route("~/api/Attributes/GetAllConstraints")]
        [HttpGet]
        [Authorize]
        public object GetAllConstraints(string TextSearch, int currPage, int recodperpage)
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetAllConstraint(TextSearch,currPage, recodperpage)));
        }
        /// <summary>
        /// Get list Attributes and Form
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetAllAttributeCims")]
        [HttpGet]
        [Authorize]
        public object GetAllAttributeCims(int formId)
        {
            return new JsonResult(_attributeRepository.GetAllAttributeCims(formId));
        }
        /// <summary>
        /// Get list all constraints
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetAllConstraints1")]
        [HttpGet]
        [Authorize]
        public object GetAllConstraints1()
        {
            return _attributeRepository.GetAllConstraint1();
        }
        /// <summary>
        /// Get list all parent Category
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetAllParentCategory")]
        [HttpGet]
        [Authorize]
        public object GetAllParentCategory()
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetAllParentCategory()));
        }
        /// <summary>
        /// Get list all children Category
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 10/05/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Attributes/GetAllChildCategory")]
        [HttpGet]
        [Authorize]
        public object GetAllChildCategory()
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetAllChildCategory()));
        }
        [Route("~/api/Attributes/SetContextFactory")]
        [HttpPost]
        [Authorize]
        public IActionResult SetContextFactory(ConnectionStrings connectionStrings)
        {
            try
            {
                _attributeRepository.SetContextFactory(connectionStrings);
                return StatusCode(201);
            }
            catch 
            {
                return StatusCode(400);
            }
        }
        #region vudt1
        /// <summary>
        /// Get form by child code
        /// CreatedBy: vudt1
        /// </summary>
        /// <param name="childCode"></param>
        /// <returns></returns>
        [Route("~/api/Attributes/GetAllAttributesCimsWithRowDetails")]
        [HttpGet]
        [Authorize]
        public IActionResult GetAllAttributesCimsWithRowDetails(string childCode)
        {
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.GetAllAttributesCimsWithRowDetails(childCode)));
        }
        [Route("~/api/Attributes/UpdateAttributeFormList")]
        [HttpPut]
        public IActionResult UpdateAttributeFormList(TblCimsattributeForm tblCimsattributeForm)
        {
            if (ModelState.IsValid)
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_attributeRepository.UpdateAttributeFormList(tblCimsattributeForm)));
            }
            return StatusCode(400);
        }
        #endregion
    }
    
}