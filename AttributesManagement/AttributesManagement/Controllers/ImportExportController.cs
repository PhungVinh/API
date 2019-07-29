using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttributesManagement.Constant;
using AttributesManagement.DataAccess;
using AttributesManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OfficeOpenXml;

namespace AttributesManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ImportExportController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private ImportExportDA da = new ImportExportDA();
        private IExportAndImportRepository _ImportExportRepository;
        private string _organizationCode;
        public ImportExportController(IHttpContextAccessor contextAccessor, IHostingEnvironment hostingEnvironment, IExportAndImportRepository importExportRepository, IDistributedCache distributedCache)
        {
            string token = contextAccessor.HttpContext.Request.Headers[AttributeConstant.Authorization];
            token = token.Length != 0 ? token.Replace(AttributeConstant.BearerReplace, string.Empty) : string.Empty;
            var arr = new JwtSecurityToken(token);
            string orgCode = arr.Claims.ToList()[2].Value;// + AttributeConstant.ConnectionAdd;
            _organizationCode = orgCode;
            importExportRepository.LoadContext(orgCode, distributedCache);
            _ImportExportRepository = importExportRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        //public ImportExportController(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        [Route("~/api/ImportExport/ExportCustomer")]
        [HttpGet]
        [Authorize]
        public string ExportCustomer()
        {
            string rootFolder = _hostingEnvironment.WebRootPath;
            string str  = _ImportExportRepository.ExportCustomer("rootFolder");
            //return da.ExportCustomer(rootFolder);
            return str;
        }

        [Route("~/api/ImportExport/Import")]
        [HttpGet]
        [Authorize]
        public object Import(string fileName)
        {
            string rootFolder = _hostingEnvironment.WebRootPath;
            return da.Import(fileName,rootFolder);
        }

    }
}