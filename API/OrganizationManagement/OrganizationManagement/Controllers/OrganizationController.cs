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
using OrganizationManagement.EmailService;
using OrganizationManagement.Common;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace OrganizationManagement.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrganizationController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        private OrganizationDA organizationDA;
        private readonly IEmailService _emailService;
        private static IHostingEnvironment _environment;
        private IHttpContextAccessor httpContextAccessor;

        private IOrganizationRepository _iOrganizationRepository;
        public OrganizationController(IDistributedCache distributedCache, IOrganizationRepository organizationRepository, IHostingEnvironment environmen, IHttpContextAccessor contextAccessor)
        {
            _distributedCache = distributedCache;
            organizationRepository.LoadDistributedCache(_distributedCache, _emailService);
            _iOrganizationRepository = organizationRepository;
            _environment = environmen;
            httpContextAccessor = contextAccessor;
            //_iOrganizationRepository.TestBase64(@"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QIAAgd4KSEA033VdHRA333JnmCYgNqYJrf06vzy/IR6dGxOTLuSrz1944YVFbNMeN1y7rfjkZXeW21tP+3JxyUW3lVt67rJLvz2tiJlf72wCouuLUM4sbEcCBAgQIECgFgEJiFoY1ywkd0DcfA2doS0BsZEnPxS/w4cPF48++mgehqNLgdyYmJaAiMTDFVdcUW7XXXfdmupXX/W94k1v+GJx5luuGruddebVtfWaBESFcuPGjUv3OazL9rmz2rt8nzWsz/W5GGg/Bk499dRaBi4SEOMZn3766eLee++tzbiWgjpSyN69e4vrr7++uOOOO8oaXXDBBcfULD5WfevWrTPX9vvf/35x1VVXzbz/kHbMvVhqyuKxxx4r+/gHP/hBU6eopdyu+q3VuGQbX+Pxvve9r7jsssuKSELM8hh9v916663Fjh07Zjm03Ofzn/98sXv37pn379uOuTGxVgIikg9f+MIXyt95V189W/Lg1q/tLB7e8VRx9OgLxYEDzxfbHvxx8eADu0rWAwdWauNtKgHRy4/hrBOjth5SEAECBAgQ6LlA/H3ds2fPTwc1R7NakjtYyzr5Tw9u4vx1lPnhD3+4uPjii3ObVx5fR31qqUhNhaQEw1133VV86UtfKs4444ziiSeeKLd4HDx4sLjtttvKr/HYt29fcd99963+/MgjjxSRdIjn4xHlhXXsExdhscX3zzzzzNjja2pGJ4rpamw8+eSTZT+cddZZnXCaVImu+k2q74MPPlh85CMfKW0jARHvgTvvvLPYtetnF6Tx8/bt21ffK+m56vsr3m/xHkrvn/ga77f03knvt+p7KcpPCY7NmzcX8d6NMkbfn7FP9fhOd/6EyuXGxKRr10g+XHrppcXb3va24u67756Z5qYbthd33P4PxZEjR4u9PzlY/P3XHy2+vnVnsbJypDh0aLak0ywnq/Oau2qYnYBoYw2IOjFmwbcPAQIECBBYBoH4+xqDypWVvP+g5A7Wcq2bOH9umffff395URAD9ToeufWpow51lhEJg5ixsGXLliL++xoXRDFbJL7GIy5aIzbPPPPM8udXvepVZcLhvPPOK+65557yuPe85z2rF1lR3pvf/OayjEj8PP744+XxZ5999nHH19mOLpTV1diI2wH+7M/+rLjpppu6wDSxDl31m1Th+J0S8Z4eMcMkYv3cc889JtariZ/4Pr2/YvbRa17zmvL9FM/Hz3Fs/Bxfo+x4H95+++3lf+o/+tGPlq/F+y09og7xHoxESCREIhESdYj35+jxne78CZXLjYlx166RfAjDTZs2FXE9Pc/17TVfeXBsAmLfvkPF888fqY14njpNO6kExDQhrxMgQIAAgSUUiMFGTKOVgDi+83MHoH/8x39cvOMd7yh+67d+q5b7snPr07XwjouU97///avT81PiISVsYkp5PNIMkurrccHzxje+sXjXu9612qwor3psJH/iYui1r31tuc9o+V3zyKlPl2Mj/hue7HPa2OSxXfYb1+5ICkSCMz1iBlEkCU477bRjYj29h6rxf+WVV64mCOL5lMyI59P7rfpeitfjPXj++eeXt12kRzruO9/5TnmrVPxXP+qQEhDpvVhXArbJ/h9Xdm5MjF7IR/LhDW94Qznz4dlnny1POc/F/t/97f1jExB7ntpf3pZR12OeOk075yASEC+88EJhYyAGxIAYEANioL4YqDMBEf8Na+uRO1hsYgBaHajX4dJEG+uo13rLGF3jYTRBEBcysX5AXPhUL6DigiYuemL/D3zgA0WssxGPKC+SDfH6tddeW97W8bnPfa545Stfedzx661zV4/ramzERVfMckl9yK8egZixELF++eWXF5E4CN9wfv3rX1+e4OUvf3n52kUXXbR6wni/xHvj9NNPL2+jqL7f4ud3vvOdZRmf+cxnyvdSNYEQ77NILnz6058+JgERaxhEefEejOO/+MUvll9Hj6+n1YstJfc9Vb2Qj5kn3/jGN8o1H44c+flshXku9j/3mXvGJiCeeHxvrTDz1GnaiauGvfwYzsAw4KxvwMmSpRgQA2JADEQMSEBMHkLlDkCnDc7mfb1r9Zm3/nXu//GPf7y8//yWW24pb8dY9ofYyIuAofmNm3GSMwsljo3bKyIRsSyP3JioXsh/4hOfGLsY8TwX+5/91F1jExCP/+gntXbJPHWaduJaExBtfAynBISBsoslMSAGxIAYqD8GJCAkIKYNIrv6elwQpQUmu1rHRdUr92JpUfXs6nmG5jfufZHzXomFYOP9lhaE7Wo/1lmv3JiY5UJ+ln1SmyIBse3B3eXtFvv3rxTf++4Txf33PV5IQGzYUGe/H1OWBET9g04DeaZiQAyIATEgASEB0djgTcELE8i9WFpYRTt6In4d7ZgWq5UbE7N+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");

        }

        /// <summary>
        ///  Get Org    
        /// </summary>
        /// Phongtv
        /// <returns></returns>
        [Route("~/api/Organization/GetOrganizationList")]
        [HttpGet]

        public object GetOrganizationList(string DateFrom, string DateTo, string TextSearch, int IsActive, int currPage, int recordperpage)
        {
            string arr = DateFrom + OrganizationConstant.StringSlipSearch
                         + DateTo + OrganizationConstant.StringSlipSearch
                        + TextSearch + OrganizationConstant.StringSlipSearch
                      + IsActive + OrganizationConstant.StringSlipSearch
                      + currPage + OrganizationConstant.StringSlipSearch
                      + recordperpage;

            return _iOrganizationRepository.GetOrganizationList(arr);
        }



        [Route("~/api/Organization/GetServicePack")]
        [HttpGet]

        public object GetServicePack(int OrganizationId)
        {

            return _iOrganizationRepository.GetServicePack(OrganizationId);
        }




        /// <summary>
        ///   // Add Org  
        /// </summary>
        /// Phongtv
        /// <returns></returns>

        [Route("~/api/Organization/AddOrganization")]
        [HttpPost]
        [Authorize]
        public IActionResult AddOrganization(UpdateOrg model)
        {


            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();

            if (string.IsNullOrEmpty(model.Org.OrganizationTaxCode) || string.IsNullOrEmpty(model.Org.OrganizationEmail) || string.IsNullOrEmpty(model.Org.OrganizationCode))
            {
                List<FieldErrors> lstError = new List<FieldErrors>();
                if (string.IsNullOrEmpty(model.Org.OrganizationEmail))
                {
                    error.objectName = OrganizationConstant.TypeOrganization;
                    error.field = OrganizationConstant.entityEmail;
                    error.message = OrganizationConstant.Message;
                    lstError.Add(error);
                }
                if (string.IsNullOrEmpty(model.Org.OrganizationTaxCode))
                {
                    error.objectName = OrganizationConstant.TypeOrganization;
                    error.field = OrganizationConstant.entityTaxCode;
                    error.message = OrganizationConstant.Message;
                    lstError.Add(error);
                }
                if (string.IsNullOrEmpty(model.Org.OrganizationCode))
                {
                    error.objectName = OrganizationConstant.TypeOrganization;
                    error.field = OrganizationConstant.entityOrgCode;
                    error.message = OrganizationConstant.Message;
                    lstError.Add(error);
                }
                rm.Title = OrganizationConstant.Title;
                rm.Message = OrganizationConstant.MessageError;
                rm.Status = OrganizationConstant.statusError;
                var field = new { fieldErrors = lstError };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string token = httpContextAccessor.HttpContext.Request.Headers[OrganizationConstant.AUTHORIZATION];
            //token = token.Length != 0 ? token.Replace(OrganizationConstant.BEARER_REPLACE, string.Empty) : string.Empty;
            string username = User.Claims.FirstOrDefault().Value;
            model.Org.CreateBy = username;
            model.Org.UpdateBy = username;
            string imgbase64 = model.Org.OrganizationLogo;
            string Logo = model.Org.OrganizationCode + OrganizationConstant.DirectorySlatSave + model.Org.OrganizationCode + ".png";
            Byte[] bitmapData = null;
            model.Org.OrganizationLogo = Logo;
            if (imgbase64 != null)
            {
                try
                {
                    bitmapData = Convert.FromBase64String(imgbase64);
                }
                catch
                {
                    model.Org.OrganizationLogo = null;
                }
            }

            int code = _iOrganizationRepository.AddOrganization(model, token);
            if (code == 1)
            {
                var obj = new { message = OrganizationConstant.AddOK };
                if (bitmapData != null)
                {
                    try
                    {
                        string str = null;
                        // Get Directory Organization
                        if (!Directory.Exists(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat);
                        }
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat + model.Org.OrganizationCode + ".png"))
                        {
                            filestream.Write(bitmapData, 0, bitmapData.Length);
                            filestream.Flush();
                            str = OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat + model.Org.OrganizationCode + ".png";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return StatusCode(201, obj);

            }

            else
            {

                if (code == 411)
                {
                    var obj = new { message = OrganizationConstant.DuplicateOrgCode };
                    return StatusCode(400, obj);
                }

                if (code == 412)
                {
                    var obj = new { message = OrganizationConstant.DuplicateEmail };
                    return StatusCode(400, obj);
                }

                if (code == 413)
                {
                    var obj = new { message = OrganizationConstant.DuplicateOrgTaxCode };
                    return StatusCode(400, obj);
                }

                else
                {
                    return StatusCode(400);
                }
            }



        }


        /// <summary>
        ///   //Edit Org
        /// </summary>
        /// Phongtv
        /// <returns></returns>

        [Route("~/api/Organization/UpdateOrganization")]
        [HttpPut]
        [Authorize]
        public IActionResult EditOrganization(UpdateOrg model)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();


            if (string.IsNullOrEmpty(model.Org.OrganizationTaxCode) || string.IsNullOrEmpty(model.Org.OrganizationEmail) || string.IsNullOrEmpty(model.Org.OrganizationCode))
            {
                List<FieldErrors> lstError = new List<FieldErrors>();
                if (string.IsNullOrEmpty(model.Org.OrganizationEmail))
                {
                    error.objectName = OrganizationConstant.TypeOrganization;
                    error.field = OrganizationConstant.entityEmail;
                    error.message = OrganizationConstant.Message;
                    lstError.Add(error);
                }
                if (string.IsNullOrEmpty(model.Org.OrganizationTaxCode))
                {
                    error.objectName = OrganizationConstant.TypeOrganization;
                    error.field = OrganizationConstant.entityTaxCode;
                    error.message = OrganizationConstant.Message;
                    lstError.Add(error);
                }
                if (string.IsNullOrEmpty(model.Org.OrganizationCode))
                {
                    error.objectName = OrganizationConstant.TypeOrganization;
                    error.field = OrganizationConstant.entityOrgCode;
                    error.message = OrganizationConstant.Message;
                    lstError.Add(error);
                }
                rm.Title = OrganizationConstant.Title;
                rm.Message = OrganizationConstant.MessageError;
                rm.Status = OrganizationConstant.statusError;
                var field = new { fieldErrors = lstError };
                rm.fieldError = field;
                return StatusCode(400, rm);
            }

            string token = httpContextAccessor.HttpContext.Request.Headers[OrganizationConstant.AUTHORIZATION];
            //token = token.Length != 0 ? token.Replace(OrganizationConstant.BEARER_REPLACE, string.Empty) : string.Empty;
            string username = User.Claims.FirstOrDefault().Value;
            model.Org.CreateBy = username;
            model.Org.UpdateBy = username;
            string imgbase64 = model.Org.OrganizationLogo;
            string Logo = model.Org.OrganizationCode + OrganizationConstant.DirectorySlatSave + model.Org.OrganizationCode + ".png";
            Byte[] bitmapData = null;
            model.Org.OrganizationLogo = Logo;
            if (imgbase64 != null)
            {
                try
                {
                    bitmapData = Convert.FromBase64String(imgbase64);
                }
                catch
                {
                    model.Org.OrganizationLogo = null;
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _iOrganizationRepository.UpdateOrganization(model);

            if (code == 2)
            {
                var obj = new { message = OrganizationConstant.EditOK };
                if (bitmapData != null)
                {
                    try
                    {
                        string str = null;
                        // Get Directory Organization
                        if (!Directory.Exists(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat);
                        }
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat + model.Org.OrganizationCode + ".png"))
                        {
                            filestream.Write(bitmapData, 0, bitmapData.Length);
                            filestream.Flush();
                            str = OrganizationConstant.DirectoryUploads + model.Org.OrganizationCode + OrganizationConstant.DirectorySlat + model.Org.OrganizationCode + ".png";
                        }


                    }
                    catch (Exception ex)
                    {

                    }
                }
                return StatusCode(200, obj);

            }
            else
            {

                if (code == 411)
                {
                    var obj = new { message = OrganizationConstant.DuplicateOrgCode };
                    return StatusCode(400, obj);
                }

                if (code == 412)
                {
                    var obj = new { message = OrganizationConstant.DuplicateEmail };
                    return StatusCode(400, obj);
                }

                if (code == 413)
                {
                    var obj = new { message = OrganizationConstant.DuplicateOrgTaxCode };
                    return StatusCode(400, obj);
                }

                else
                {
                    return StatusCode(400);
                }
            }




        }


        /// <summary>
        /// // Delete Org
        /// </summary>
        /// Phongtv
        /// <returns></returns>
        [Route("~/api/Organization/DeleteOrganization")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteOrganization(int OrganizationId)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _iOrganizationRepository.DeleteOrganization(OrganizationId);


            if (code == 1)
            {
                var obj = new { message = "Xóa thành công !" };
                return StatusCode(201, obj);
            }
            else
            {
                var obj = new { message = OrganizationConstant.NoDelete };
                return StatusCode(400, obj);
            }

        }

        /// <summary>
        /// ActiveOrganization
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="IsCheckedOrg"></param>
        /// <returns></returns>

        [Route("~/api/Organization/ActiveOrganization")]
        [HttpPatch]
        [Authorize]
        public IActionResult ActiveOrganization(int OrganizationId, bool IsCheckedOrg)
        {
            var response = new { Code = 0 };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _iOrganizationRepository.ActiveOrganization(OrganizationId, IsCheckedOrg);
            response = new { Code = code };


            if (code == 1)
            {

            }
            return Json(response);
        }

        /// <summary>
        /// // Delete User to Org
        /// </summary>
        /// Phongtv
        /// <returns></returns>
        [Route("~/api/Organization/DeleteUserOrg")]
        [HttpDelete]
        [Authorize]
        public IActionResult DeleteUserOrg([FromBody] TblOrganizationUser OrgUser)
        {
            var response = new { Code = 0 };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int code = _iOrganizationRepository.DeleteUserOrg(OrgUser);
            response = new { Code = code };

            if (code == 1)
            {

            }
            return Json(response);
        }

        /// <summary>
        /// HaiHM
        /// Upload Avatar
        /// </summary>
        /// <param name="files">file</param>
        /// <returns></returns>
        [Route("~/api/Organization/UploadFile")]
        [HttpPost]
        [Authorize]
        public IActionResult UploadFile(IFormFile files)
        {
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            string token = HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Length != 0 ? token.Replace("Bearer ", "") : "";
                var arr = new JwtSecurityToken(token);
                var ab = arr.Claims.ToArray();
                var orgCode = ab[2].Value;
                if (orgCode != null)
                {
                    if (files.Length > OrganizationConstant.MaximumFile || files.Length <= 0)
                    {

                        rm.Title = OrganizationConstant.TitleMaximumFile;
                        rm.Message = OrganizationConstant.MaximumFileOver;
                        rm.Status = OrganizationConstant.statusFail;
                        var field = new { fieldErrors = rm.Title };
                        rm.fieldError = field;

                        return StatusCode(400, Json(rm));
                    }
                    else
                    {
                        try
                        {
                            string str = null;
                            // Get Directory Organization
                            if (!Directory.Exists(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat))
                            {
                                Directory.CreateDirectory(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat);
                            }
                            using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat + files.FileName))
                            {
                                files.CopyTo(filestream);
                                filestream.Flush();
                                str = OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat + files.FileName;
                            }


                            //var base64 = ImageToBase64(str);
                            TblOrganization organization = _iOrganizationRepository.GetOrganization(orgCode);
                            organization.OrganizationLogo = str;
                            _iOrganizationRepository.UpdateOrganizationLogo(organization);
                            object obj = new { message = str };
                            return StatusCode(201, obj);
                        }
                        catch (Exception ex)
                        {
                            return BadRequest();
                        }
                    }
                }
            }
            return BadRequest();
        }

        private string ImageToBase64(string path)
        {
            string base64String = null;
            try
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);
                return base64String = Convert.ToBase64String(imageBytes);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Function convert ImageToBase64
        /// CreateBy: HaiHM
        /// CreatedDate: 01/06/2019
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> ImageToBase64Async(string path)
        {
            string p = _environment.WebRootPath + path;
            string base64String = null;
            return await Task.Run(() =>
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(p);
                base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            });
        }

        /// <summary>
        /// Function upload file image
        /// CreatedBy: HaiHM
        /// CreatedDate: 2019/06/01
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [Route("~/api/Organization/UploadFileForOrganization")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFileForOrganization(IFormFile files)
        {
            bool checkExtentionFile = false;
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            OrganizationCommon organizationCommon = new OrganizationCommon();
            string orgCode = User.Claims.Where(u => u.Type == OrganizationConstant.orgCode).FirstOrDefault().Value;
            //JPG, PNG
            string fileName = organizationCommon.ConvertStringToVNChar(files.FileName.Replace(" ", string.Empty));

            if (organizationCommon.ImageExtensions.Contains(Path.GetExtension(fileName).ToUpperInvariant()))
            {
                checkExtentionFile = true;
            }

            if (orgCode != null && checkExtentionFile)
            {
                if (files.Length > OrganizationConstant.MaximumFile || files.Length <= 0)
                {
                    rm.Title = OrganizationConstant.TitleMaximumFile;
                    rm.Message = OrganizationConstant.MaximumFileOver;
                    rm.Status = OrganizationConstant.statusError;
                    var field = new { fieldErrors = rm.Title };

                    return StatusCode(400, Json(rm));
                }
                else
                {
                    try
                    {
                        string str = null;
                        if (!Directory.Exists(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat);
                        }
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat + orgCode + ".png"))
                        {
                            files.CopyTo(filestream);
                            filestream.Flush();
                            str = OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat + orgCode + ".png";
                        }
                        string base64 = await ImageToBase64Async(str);
                        Byte[] bitmapData = Convert.FromBase64String(base64);
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + OrganizationConstant.DirectoryUploads + orgCode + OrganizationConstant.DirectorySlat + orgCode + ".png"))
                        {
                            filestream.Write(bitmapData, 0, bitmapData.Length);
                            filestream.Flush();
                        }

                        string Logo = orgCode + OrganizationConstant.DirectorySlatSave + orgCode + ".png";
                        TblOrganization organization = _iOrganizationRepository.GetOrganization(orgCode);
                        organization.OrganizationLogo = Logo;
                        _iOrganizationRepository.UpdateLogoInforOrg(organization);
                        if (!string.IsNullOrEmpty(str))
                        {
                            object obj = new { data = base64 };
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
        /// Function get all infor org
        /// CreatedBy: HaiHM
        /// CreatedDate: 22/5/2019
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Organization/GetInforOrganization")]
        [HttpGet]
        [Authorize]
        public async Task<object> GetInforOrganization()
        {
            int userId = Int32.Parse(User.Claims.Where(u => u.Type == OrganizationConstant.userId).FirstOrDefault().Value);
            return await _iOrganizationRepository.GetInforOrganization(userId);
        }

        /// <summary>
        /// Function update infor Orga
        /// CreatedBy: HaiHM
        /// CreatedDate: 27/5/2019
        /// </summary>
        /// <param name="organization">object</param>
        /// <returns>object infor</returns>
        [Route("~/api/Organization/UpdateInforOrganization")]
        [HttpPut]
        [Authorize]
        public Object UpdateInforOrganization([FromBody] TblOrganization organization)
        {
            int update = OrganizationConstant.UpdateOrgFail;
            ErrorObject response = new ErrorObject();
            ResponseMessage rm = new ResponseMessage();
            FieldErrors error = new FieldErrors();
            if (organization != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (string.IsNullOrEmpty(organization.OrganizationEmail) || string.IsNullOrEmpty(organization.OrganizationTaxCode))
                {
                    List<FieldErrors> lsterror = new List<FieldErrors>();
                    if (string.IsNullOrEmpty(organization.OrganizationEmail))
                    {
                        error.objectName = OrganizationConstant.TypeOrg;
                        error.field = OrganizationConstant.TypeNullEmail;
                        error.message = OrganizationConstant.MessageTypeNullEmail;
                        lsterror.Add(error);
                    }
                    if (string.IsNullOrEmpty(organization.OrganizationTaxCode))
                    {
                        error.objectName = OrganizationConstant.TypeOrg;
                        error.field = OrganizationConstant.TypeNullTaxCode;
                        error.message = OrganizationConstant.MessageTypeNullTaxCode;
                        lsterror.Add(error);
                    }
                    rm.Title = OrganizationConstant.TypeOrg;
                    rm.Message = OrganizationConstant.MessageError;
                    rm.Status = OrganizationConstant.statusError;
                    var field = new { fieldErrors = lsterror };
                    rm.fieldError = field;
                    return StatusCode(400, rm);
                }
                int userId = Int32.Parse(User.Claims.Where(u => u.Type == OrganizationConstant.userId).FirstOrDefault().Value);
                string username = User.Claims.FirstOrDefault().Value;
                string Logo = organization.OrganizationCode + OrganizationConstant.DirectorySlatSave + organization.OrganizationCode + ".png";
                organization.OrganizationLogo = Logo;
                update = _iOrganizationRepository.UpdateInforOrganization(organization, userId, username);
                if (update == OrganizationConstant.UpdateOrgDuplicateEmail)
                {
                    response.entityName = OrganizationConstant.TypeOrg;
                    response.errorKey = OrganizationConstant.ErrorKeyOrgDuplicateEmail;
                    response.status = OrganizationConstant.statusError;
                    response.message = OrganizationConstant.MessageOrgDuplicateEmail;
                    return StatusCode(400, Json(response));
                }
                else if (update == OrganizationConstant.UpdateOrgDuplicateTaxCode)
                {
                    response.entityName = OrganizationConstant.TypeOrg;
                    response.errorKey = OrganizationConstant.ErrorKeyOrgDuplicateTaxCode;
                    response.status = OrganizationConstant.statusError;
                    response.message = OrganizationConstant.MessageOrgDuplicateTaxCode;
                    return StatusCode(400, Json(response));
                }
                else if (update == OrganizationConstant.UpdateOrgSuccess)
                {
                    var obj = new { message = OrganizationConstant.MessageUpdateOrgSuccess };
                    return StatusCode(200, obj);
                }
                else
                {
                    response.entityName = OrganizationConstant.TypeOrg;
                    response.errorKey = OrganizationConstant.ErrorKeyOrgServerErr;
                    response.status = OrganizationConstant.statusError;
                    response.message = OrganizationConstant.MessageOrgOrgServerErr;
                    return StatusCode(400, Json(response));
                }
            }
            else
            {
                response.entityName = OrganizationConstant.TypeOrg;
                response.errorKey = OrganizationConstant.ErrorKeyOrgServerErr;
                response.status = OrganizationConstant.statusError;
                response.message = OrganizationConstant.MessageOrgOrgServerErr;
                return StatusCode(400, Json(response));
            }

        }

        /// <summary>
        /// Function get list org
        /// </summary>
        /// <returns></returns>
        [Route("~/api/Organization/GetListOrg")]
        [HttpGet]
        [Authorize]

        public Object GetListOrg()
        {
            return _iOrganizationRepository.GetListOrg();
        }


        /// <summary>
        /// API get images of organization
        /// CreatedBy: DaiBH
        /// CreatedDate: 04/06/2019
        /// </summary>
        /// <param name="organizationCode">organizationCode</param>
        /// <param name="imageFile">imageFile</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("~/api/Organization/Images/{organizationCode}/{imageFile}")]
        [HttpGet("{organizationCode}/{imageFile}")]
        public async Task<ActionResult> GetResourceImage(string organizationCode, string imageFile)
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
                byte[] data = await _iOrganizationRepository.GetImageAsync(organizationCode, imageFile);
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
            }
            catch (Exception ex)
            {
                // throw not found resource when requested file is not existing on systems
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }



    }
}
