using AccountManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VOC.Common;
using VOC.Constant;
using VOC.DataAccess;
using VOC.Models;
using VOC.Repositories;
using VOC.ViewModel;

namespace VOC.Controllers
{
    [Route("api/[controller]")]
    [TokenExtensionAttribute]
    [Authorize]
    [ApiController]
    public class VOCProcessController : ControllerBase
    {
        private IVOCProcessRepository _iVOCProcessRepository;
        private string _organizationCode;
        private string userName;
        private int userId;
        public VOCProcessController(IHttpContextAccessor contextAccessor, IVOCProcessRepository VOCProcessRepository, IDistributedCache distributedCache)
        {
            string token = contextAccessor.HttpContext.Request.Headers[VOCConstant.Authorization];
            token = token.Length != 0 ? token.Replace(VOCConstant.BearerReplace, string.Empty) : string.Empty;
            var arr = new JwtSecurityToken(token);
            _organizationCode = arr.Claims.ToList()[2].Value;
            userId = Int32.Parse(arr.Claims.ToList()[1].Value);
            userName = arr.Claims.ToList()[0].Value;
            _iVOCProcessRepository = VOCProcessRepository;
            _iVOCProcessRepository.LoadContext(_organizationCode, distributedCache);
        }

        /// <summary>
        /// API thực hiện lấy danh sách người dùng của đơn vị
        /// @author: HaiHM
        /// @createdDate: 17/07/2019
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="vocProcessCode"></param>
        /// <param name="version"></param>
        /// <param name="stepCode"></param>
        /// <returns></returns>
        [Route("~/api/VOC/GetUserAssignee")]
        [HttpGet]
        [Authorize]
        public object AssigneeVOCProcess(string textSearch, string vocProcessCode, int version, string stepCode)
        {
            try
            {
                textSearch = string.IsNullOrEmpty(textSearch) ? "" : textSearch.Trim();
                vocProcessCode = string.IsNullOrEmpty(vocProcessCode) ? "" : vocProcessCode.Trim();
                stepCode = string.IsNullOrEmpty(stepCode) ? "" : stepCode.Trim();
                if (!string.IsNullOrEmpty(vocProcessCode))
                {
                    if (version <= 0)
                    {
                        ResponseMessage rm = new ResponseMessage();
                        FieldErrors error = new FieldErrors();
                        error.ObjectName = VOCProcessConstant.TypeVOC;
                        error.Field = VOCProcessConstant.FieldVersion;
                        error.Message = VOCProcessConstant.NeedVersion;
                        List<FieldErrors> lsterror = new List<FieldErrors>();
                        lsterror.Add(error);
                        rm.Type = VOCProcessConstant.TypeVOC;
                        rm.Title = VOCProcessConstant.TitleValidate;
                        rm.Message = VOCProcessConstant.ErrorValidate;
                        rm.Status = VOCProcessConstant.StatusFail;
                        var field = new { fieldErrors = lsterror };
                        rm.FieldError = field;
                        return StatusCode(400, rm);
                    }
                }
                var response = _iVOCProcessRepository.GetUserAssignee(textSearch, _organizationCode, vocProcessCode, version, stepCode);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }


        /// <summary>
        /// Hàm thực hiện tìm kiếm, lấy danh sách quy trình sự vụ
        /// @author: HaiHM
        /// @createdDate: 17/07/2019
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="isActive"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <returns></returns>
        [Route("~/api/VOC/SearchVOCProcess")]
        [HttpGet]
        [Authorize(Policy = VOCConstant.PolicyShow)]
        public object SearchVOCProcess(string textSearch, string isActive, int currPage, int recordperpage)
        {
            try
            {
                textSearch = string.IsNullOrEmpty(textSearch) ? "" : textSearch.Trim();
                isActive = string.IsNullOrEmpty(isActive) ? "" : isActive.Trim();
                string isShowAll = "1";
                int userId = Int32.Parse(User.Claims.Where(u => u.Type == VOCConstant.userId).FirstOrDefault().Value);

                var data = _iVOCProcessRepository.SearchVOCProcess(userId, _organizationCode, isShowAll, textSearch, isActive, currPage, recordperpage);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private List<FieldErrors> ValidateAddOrEdit(ObjectVOCProccess model)
        {
            ErrorObject errorObject = new ErrorObject();
            FieldErrors error = new FieldErrors();
            List<FieldErrors> lsterror = new List<FieldErrors>();
            // kiểm tra dữ liệu truyền lên
            if (string.IsNullOrEmpty(model.VOCProcessCode))
            {
                error = new FieldErrors();
                error.ObjectName = VOCProcessConstant.TypeVOC;
                error.Field = VOCProcessConstant.FieldVOCProcessCode;
                error.Message = VOCProcessConstant.NullCodeVOCProcess;
                lsterror.Add(error);
            }
            else
            {
                //Kiểm tra tính hợp lệ của mã quy trình
                //-1 Nhỏ hơn 3 ký tự
                model.VOCProcessCode = model.VOCProcessCode.Replace(" ", string.Empty);
                if (model.VOCProcessCode.Length < 3)
                {
                    error = new FieldErrors();
                    error.ObjectName = VOCProcessConstant.TypeVOC;
                    error.Field = VOCProcessConstant.FieldVOCProcessCode;
                    error.Message = VOCProcessConstant.MinVOCProcessCode;
                    lsterror.Add(error);
                }
                Regex rgx = new Regex(@"^[a-zA-Z0-9]+$");
                bool checkRegex = rgx.IsMatch(model.VOCProcessCode);
                if (!checkRegex)
                {
                    error = new FieldErrors();
                    error.ObjectName = VOCProcessConstant.TypeVOC;
                    error.Field = VOCProcessConstant.FieldVOCProcessCode;
                    error.Message = VOCProcessConstant.FormatVOCProcessCode;
                    lsterror.Add(error);
                }
            }
            if (string.IsNullOrEmpty(model.VOCProcessName))
            {
                error = new FieldErrors();
                error.ObjectName = VOCProcessConstant.TypeVOC;
                error.Field = VOCProcessConstant.FieldVOCProcessName;
                error.Message = VOCProcessConstant.NullNameProcessName;
                lsterror.Add(error);
            }
            if (string.IsNullOrEmpty(model.VOCProcessType))
            {
                error = new FieldErrors();
                error.ObjectName = VOCProcessConstant.TypeVOC;
                error.Field = VOCProcessConstant.FieldVOCProcessType;
                error.Message = VOCProcessConstant.NullTypeVOCProcess;
                lsterror.Add(error);
            }

            // Kiểm tra tính hợp lệ các bước của quy trình
            if (model.objectSteps != null)
            {
                //Lấy danh sách bước cha
                List<TblVocprocessSteps> lstParent = new List<TblVocprocessSteps>();
                int runCode = 1;

                foreach (var item in model.objectSteps)
                {

                    TblVocprocessSteps stepParent = new TblVocprocessSteps();
                    item.StepCode = VOCProcessConstant.STEP + runCode;
                    item.ParentCode = VOCProcessConstant.PARENT;

                    stepParent.VocprocessCode = item.VocprocessCode;
                    stepParent.StepCode = item.StepCode;
                    stepParent.ParentCode = item.ParentCode;
                    stepParent.StepName = item.StepName;
                    stepParent.FormId = item.FormId;
                    stepParent.ConditionId = item.ConditionId;
                    stepParent.OrderNum = item.OrderNum;
                    stepParent.Version = item.Version;
                    stepParent.FinishDate = item.FinishDate;
                    stepParent.IsFinish = item.IsFinish;
                    stepParent.InProgress = item.InProgress;
                    stepParent.DurationStepDay = item.DurationStepDay;
                    stepParent.DurationStepHour = item.DurationStepHour;
                    stepParent.DurationStepMinute = item.DurationStepMinute;

                    lstParent.Add(stepParent);
                    //Kiểm tra tất cả các bước con
                    List<TblVocprocessSteps> lstChild = item.stepChilds;
                    if (lstChild != null && lstChild.Count > 0)
                    {
                        //Thêm mã code cho các bước
                        int runCodeChild = 1;
                        foreach (var itemAddCode in lstChild)
                        {
                            itemAddCode.StepCode = VOCProcessConstant.STEP + runCode + "." + runCodeChild++;
                            itemAddCode.ParentCode = item.StepCode;
                            if (String.IsNullOrEmpty(itemAddCode.StepName))
                            {
                                error = new FieldErrors();
                                error.ObjectName = VOCProcessConstant.TypeVOC;
                                error.Field = itemAddCode.StepCode;
                                error.Message = VOCProcessConstant.NullStepName;
                                lsterror.Add(error);
                            }
                        }

                        foreach (var itemChild in lstChild)
                        {
                            foreach (var itemChildRound2 in lstChild)
                            {
                                if (itemChild.StepCode.Trim() != itemChildRound2.StepCode.Trim())
                                {
                                    if (!String.IsNullOrEmpty(itemChild.StepName) && !String.IsNullOrEmpty(itemChildRound2.StepName))
                                    {
                                        if (String.Compare(itemChild.StepName.Trim(), itemChildRound2.StepName.Trim(), true) == 0)
                                        {
                                            error = new FieldErrors();
                                            error.ObjectName = VOCProcessConstant.TypeVOC;
                                            error.Field = itemChildRound2.StepCode;
                                            error.Message = VOCProcessConstant.DuplicateValidate;
                                            lsterror.Add(error);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    runCode += 1;
                }

                if (lstParent != null && lstParent.Count > 0)
                {
                    //Kiểm tra tên tất cả các bước cha
                    foreach (var item in lstParent)
                    {
                        foreach (var itemRound2 in lstParent)
                        {
                            if (item.StepCode.Trim() != itemRound2.StepCode.Trim())
                            {
                                if (!String.IsNullOrEmpty(item.StepName) && !String.IsNullOrEmpty(itemRound2.StepName))
                                {
                                    if (String.Compare(item.StepName.Trim(), itemRound2.StepName.Trim(), true) == 0)
                                    {
                                        error = new FieldErrors();
                                        error.ObjectName = VOCProcessConstant.TypeVOC;
                                        error.Field = itemRound2.StepCode;
                                        error.Message = VOCProcessConstant.DuplicateValidate;
                                        lsterror.Add(error);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return lsterror;
        }

        /// <summary>
        /// Hàm thực hiện thêm mới quy trình của sự vụ
        /// @author: HaiHM
        /// @createdDate: 16/07/2019
        /// </summary>
        /// <param name="model">object dùng để thêm mới quy trình(bao gồm thông tin quy trình và danh sách user được assignee)</param>
        /// <returns></returns>
        [Route("~/api/VOC/AddVOCProcess")]
        [HttpPost]
        [Authorize]
        public IActionResult AddVOCProcess([FromBody]ObjectVOCProccess model)
        {
            ErrorObject errorObject = new ErrorObject();
            FieldErrors error = new FieldErrors();
            ResponseMessage rm = new ResponseMessage();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model != null)
            {
                List<FieldErrors> lsterror = ValidateAddOrEdit(model);
                if (lsterror.Count > 0)
                {
                    // response validate fail to client
                    rm.Type = VOCProcessConstant.TypeVOC;
                    rm.Title = VOCProcessConstant.TitleValidate;
                    rm.Message = VOCProcessConstant.ErrorValidate;
                    rm.Status = VOCProcessConstant.StatusFail;
                    var field = new { fieldErrors = lsterror };
                    rm.FieldError = field;
                    return StatusCode(400, rm);
                }
                else
                {
                    ErrorObject response = new ErrorObject();
                    // thêm vào database
                    int result = _iVOCProcessRepository.AddVOCProcess(model, userName);

                    if (result == VOCProcessConstant.AddDuplicateCode)
                    {
                        response.EntityName = VOCProcessConstant.TypeVOC;
                        response.ErrorKey = VOCProcessConstant.ErrorKeyAddDuplicateCode;
                        response.Status = VOCProcessConstant.StatusFail;
                        response.Message = VOCProcessConstant.MessageAddDuplicateCode;
                        return StatusCode(400, response);
                    }
                    else if (result == VOCProcessConstant.AddVOCProcessSuccess)
                    {
                        object obj = new { message = VOCProcessConstant.MessageAddSuccess };
                        return StatusCode(201, obj);
                    }
                    else
                    {
                        response = new ErrorObject();
                        response.EntityName = VOCProcessConstant.TypeVOC;
                        response.ErrorKey = VOCProcessConstant.ErrorKeyAddFail;
                        response.Status = VOCProcessConstant.StatusFail;
                        response.Message = VOCProcessConstant.MessageAddFail;
                        return StatusCode(400, response);
                    }
                }

            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Hàm thực hiện sửa quy trình
        /// @author: HaiHM
        /// @createdDate: 16/07/2019
        /// </summary>
        /// <param name="model">object dùng để sửa quy trình(bao gồm thông tin quy trình và danh sách user được assignee)</param>
        /// <returns></returns>
        [Route("~/api/VOC/EditVOCProcess")]
        [HttpPut]
        [Authorize]
        public IActionResult EditVOCProcess([FromBody]ObjectVOCProccess model)
        {
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            ErrorObject response = new ErrorObject();

            int resultEdit = 0;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model != null)
            {
                List<FieldErrors> lsterror = ValidateAddOrEdit(model);
                if (String.IsNullOrEmpty(model.VOCProcessCode))
                {
                    error = new FieldErrors();
                    error.ObjectName = VOCProcessConstant.TypeVOC;
                    error.Field = VOCProcessConstant.FieldVOCProcessCode;
                    error.Message = VOCProcessConstant.NeedVOCProcessCode;
                    lsterror.Add(error);
                }
                if (model.CurrentVersion == null)
                {
                    error = new FieldErrors();
                    error.ObjectName = VOCProcessConstant.TypeVOC;
                    error.Field = VOCProcessConstant.FieldVersion;
                    error.Message = VOCProcessConstant.NeedVersion;
                    lsterror.Add(error);
                }
                if (lsterror.Count > 0)
                {
                    // response validate fail to client
                    rm.Type = VOCProcessConstant.TypeVOC;
                    rm.Title = VOCProcessConstant.TitleValidate;
                    rm.Message = VOCProcessConstant.ErrorValidate;
                    rm.Status = VOCProcessConstant.StatusFail;
                    var field = new { fieldErrors = lsterror };
                    rm.FieldError = field;
                    return StatusCode(400, rm);
                }
                else
                {
                    resultEdit = _iVOCProcessRepository.EditVOCProcess(model, userName);
                    
                    if(resultEdit == VOCProcessConstant.EditVOCProcessSuccess)
                    {
                        var obj = new { code = 200, message = VOCProcessConstant.MSEditSuccess };
                        return StatusCode(200, obj);
                    }
                    else 
                    {
                        response = new ErrorObject();
                        response.EntityName = VOCProcessConstant.TypeVOC;
                        response.ErrorKey = VOCProcessConstant.ErrorKeyEditFail;
                        response.Status = VOCProcessConstant.StatusFail;
                        response.Message = VOCProcessConstant.MSEditFail;
                        return StatusCode(400, response);
                    }
                }
            }
            return BadRequest();

        }

        /// <summary>
        /// Hàm thực hiện xóa quy trình(chỉ xóa được khi chưa add quy trình vào sự vụ nào)
        /// @author: HaiHM
        /// @createdDate: 16/07/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/VOC/DeleteVOCProcess/{idVOCProcess}")]
        [HttpDelete("{idVOCProcess}")]
        [Authorize]
        public IActionResult DeleteVOCProcess(int idVOCProcess)
        {
            return BadRequest();
        }

        /// <summary>
        /// Hàm thực hiện copy quy trình theo version
        /// @author: HaiHM
        /// @createdDate: 17/07/2019
        /// </summary>
        /// <param name="idVOCProcess"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [Route("~/api/VOC/CopyVOCProcess")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CopyVOCProcess(string VOCProcessCode, int version)
        {
            try
            {
                if (String.IsNullOrEmpty(VOCProcessCode) || version < 1)
                {
                    var obj = new { data = "", code = 400 };
                    return StatusCode(400, obj);
                }
                else
                {
                    var data = await _iVOCProcessRepository.CopyVOCProcess(VOCProcessCode, version, _organizationCode);
                    return StatusCode(VOCProcessConstant.StatusCodeSuccess, data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("~/api/VOC/SearchVersion")]
        [HttpGet]
        [Authorize(Policy = VOCConstant.PolicyShow)]
        public async Task<object> SearchVersion(string userName, string VOCProcessCode, string IsActive, int currPage, int recordperpage)
        {
            try
            {
                ResponseMessage rm = new ResponseMessage();
                FieldErrors error = new FieldErrors();
                List<FieldErrors> lsterror = new List<FieldErrors>();
                userName = string.IsNullOrEmpty(userName) ? "" : userName.Trim();
                if (String.IsNullOrEmpty(VOCProcessCode))
                {
                    error = new FieldErrors();
                    error.ObjectName = VOCProcessConstant.TypeVOC;
                    error.Field = VOCProcessConstant.FieldVOCProcessCode;
                    error.Message = VOCProcessConstant.NeedVOCProcessCode;
                    lsterror.Add(error);
                }
                if (lsterror.Count > 0)
                {
                    // response validate fail to client
                    rm.Type = VOCProcessConstant.TypeVOC;
                    rm.Title = VOCProcessConstant.TitleValidate;
                    rm.Message = VOCProcessConstant.ErrorValidate;
                    rm.Status = VOCProcessConstant.StatusFail;
                    var field = new { fieldErrors = lsterror };
                    rm.FieldError = field;
                    return StatusCode(400, rm);
                }
                return _iVOCProcessRepository.SearchVersion(userName, VOCProcessCode, IsActive, currPage, recordperpage, _organizationCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [Route("~/api/VOC/VesionWhenAddVOC")]
        [HttpGet]
        [Authorize]
        public object VesionWhenAddVOC()
        {
            // Mã quy trình - Tên quy trình - Phiên bản
            return _iVOCProcessRepository.VesionWhenAddVOC();
        }

        [Route("~/api/VOC/GetListUserSearchVersion")]
        [HttpGet]
        [Authorize]
        public async Task<object> GetListUserSearchVersion(string VOCProcessCode)
        {
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            List<FieldErrors> lsterror = new List<FieldErrors>();
            if (string.IsNullOrEmpty(VOCProcessCode))
            {
                error = new FieldErrors();
                error.ObjectName = VOCProcessConstant.TypeVOC;
                error.Field = VOCProcessConstant.FieldVOCProcessCode;
                error.Message = VOCProcessConstant.NeedVOCProcessCode;
                lsterror.Add(error);
            }
            VOCProcessCode = string.IsNullOrEmpty(VOCProcessCode) ? "" : VOCProcessCode.Trim();
            var data = await _iVOCProcessRepository.GetListUserSearchVersion(VOCProcessCode, _organizationCode);
            return data;
        }

        [Route("~/api/VOC/SwitchStatus")]
        [HttpPut]
        [Authorize]
        public IActionResult SwitchStatus([FromBody]VOCProcessStepsViewModel step)
        {
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            List<FieldErrors> lsterror = new List<FieldErrors>();
            ErrorObject response = new ErrorObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(step != null)
            {
                if (String.IsNullOrEmpty(step.VOCProcessCode))
                {
                    response = new ErrorObject();
                    response.EntityName = VOCProcessConstant.TypeVOC;
                    response.ErrorKey = VOCProcessConstant.ErrorKeyMissCodeVOC;
                    response.Status = VOCProcessConstant.StatusFail;
                    response.Message = VOCProcessConstant.MSSwitchStatusMissCodeVOC;
                    return StatusCode(400, response);
                }
                if (step.IsActive.HasValue)
                {
                    int switchStatus = _iVOCProcessRepository.SwitchStatus(step);

                    if (switchStatus == VOCProcessConstant.SwitchStatusSuccess)
                    {
                        object obj = new { message = VOCProcessConstant.MSSwitchStatusSuccess };
                        return StatusCode(200, obj);
                    }
                    else if (switchStatus == VOCProcessConstant.SwitchStatusNotChange)
                    {
                        response = new ErrorObject();
                        response.EntityName = VOCProcessConstant.TypeVOC;
                        response.ErrorKey = VOCProcessConstant.ErrorKeyMSSwitchStatusNotChange;
                        response.Status = VOCProcessConstant.StatusFail;
                        response.Message = VOCProcessConstant.MSSwitchStatusNotChange;
                        return StatusCode(400, response);
                    }
                    else if (switchStatus == VOCProcessConstant.SwitchStatusNotFound)
                    {
                        response = new ErrorObject();
                        response.EntityName = VOCProcessConstant.TypeVOC;
                        response.ErrorKey = VOCProcessConstant.ErrorKeyMSSwitchStatusNotFound;
                        response.Status = VOCProcessConstant.StatusFail;
                        response.Message = VOCProcessConstant.MSSwitchStatusNotFound;
                        return StatusCode(400, response);
                    }
                    else
                    {
                        response = new ErrorObject();
                        response.EntityName = VOCProcessConstant.TypeVOC;
                        response.ErrorKey = VOCProcessConstant.ErrorKeySwitchStatusFail;
                        response.Status = VOCProcessConstant.StatusFail;
                        response.Message = VOCProcessConstant.MSSwitchStatusFail;
                        return StatusCode(400, response);
                    }
                }
                else
                {
                    error = new FieldErrors();
                    error.ObjectName = VOCProcessConstant.TypeVOC;
                    error.Field = VOCProcessConstant.FieldIsActive;
                    error.Message = VOCProcessConstant.MSNeedStatus;
                    lsterror.Add(error);

                    // response validate fail to client
                    rm.Type = VOCProcessConstant.TypeVOC;
                    rm.Title = VOCProcessConstant.TitleValidate;
                    rm.Message = VOCProcessConstant.ErrorValidate;
                    rm.Status = VOCProcessConstant.StatusFail;
                    var field = new { fieldErrors = lsterror };
                    rm.FieldError = field;
                    return StatusCode(400, rm);
                }
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
