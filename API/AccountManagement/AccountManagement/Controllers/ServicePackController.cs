using AccountManagement.Models;
using AccountManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicePackController : Controller
    {
        private IConfiguration _config;
        private IDistributedCache _distributedCache;
        private IServicePackRepository _servicePackRepository;

        public ServicePackController(IConfiguration config, IDistributedCache distributedCache, IServicePackRepository servicePackRepository)
        {
            _distributedCache = distributedCache;
            _config = config;
            _servicePackRepository = servicePackRepository;
        }

        /// <summary>
        /// Function Add ServicePack
        /// CreatedBy: HaiHM
        /// CreatedDate: 13/05/2019
        /// </summary>
        /// <param name="ServicePack">object</param>
        /// <returns></returns>
        [Route("~/api/ServicePack/Add")]
        [HttpPost]
        [Authorize]
        public IActionResult AddServicePack([FromBody] TblServicePack model)
        {
            if(model != null)
            {
                _servicePackRepository.AddServicePack(model);
            }

           return BadRequest();
        }

        /// <summary>
        /// Function Edit ServicePack 
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/05/2019
        /// </summary>
        /// <param name="ServicePack"></param>
        /// <returns></returns>
        [Route("~/api/ServicePack/Edit")]
        [HttpPut]
        [Authorize]
        public IActionResult EditServicePack([FromBody] TblServicePack model)
        {
            if (model != null)
            {
                _servicePackRepository.EditServicePack(model);
            }

            return BadRequest();
        }

        /// <summary>
        /// Function search And Get List ServicePack
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/05/2019
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="isActive"></param>
        /// <param name="orgCode"></param>
        /// <param name="currPage"></param>
        /// <param name="Record"></param>
        /// <returns></returns>
        [Route("~/api/ServicePack/GetListServicePack")]
        [HttpGet]
        [Authorize]
        public object SearchServicePack(string textSearch, string isActive, string orgCode, string currPage, string Record)
        {
            string arr = ",,1,15";
            return Json(_servicePackRepository.GetListServicePack(arr));
        }

        /// <summary>
        /// Function delete ServicePack
        /// CreatedBy: HaiHM
        /// CreatedDate: 14/05/2019
        /// </summary>
        /// <param name="id">id of ServicePack</param>
        /// <returns></returns>
        [Route("~/api/ServicePack/Delete/{id}")]
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteServicePack(int id)
        {
            var response = new { };
            _servicePackRepository.DeleteServicePack(id);
            return BadRequest();
        }

        /// <summary>
        /// Active And Lock ServicePack
        /// CreatedBy: HaiHM
        /// CreatedDate: 13/05/2019
        /// </summary>
        /// <param name="sp">id</param>
        /// <returns></returns>
        [Route("~/api/ServicePack/ActiveOrLock/{id}")]
        [HttpGet("id")]
        [Authorize]
        public IActionResult ActiveOrLock(int id)
        {
            var response = new { };
            _servicePackRepository.ActiveAndLockServicePack(id);
            return BadRequest();
        }
    }
}