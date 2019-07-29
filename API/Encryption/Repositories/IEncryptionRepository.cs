using Encryption.DTO;
using Encryption.ViewModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.Repositories
{
    public interface IEncryptionRepository : IBaseRepository
    {
        #region vudt
        ResponseDTO GetListModule(string orgCode);
        /// <summary>
        /// Get all attributes by parent code 
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns>List attributes</returns>
        Task<ResponseDTO> GetListAttributesWithParentCode(string parentCode, string orgCode);
        /// <summary>
        /// Get all attributes to configure encryption
        /// </summary>
        /// <returns>List encryption information</returns>
        Task<ResponseDTO> GetListAttributesEncryption(string orgCode);
        /// <summary>
        /// Execute encrpytion and decrpytion attributes
        /// </summary>
        /// <param name="lstAttributes">Attributes are need encryption or decryption</param>
        /// <return>Status code 200 is success, otherwise status code is 400</return>
        ResponseDTO UpdateEncrpytion(AttributeModel lstAttributes, string orgCode);
        /// <summary>
        /// Scheduler to execute encryption
        /// </summary>
        /// <param name="scheduler"></param>
        void SchedulerExecuteEncrpytion(IScheduler scheduler);
        /// <summary>
        /// Execute encryption
        /// </summary>
        /// <returns></returns>
        void ExecuteEncrpytion();
        #endregion
    }
}
