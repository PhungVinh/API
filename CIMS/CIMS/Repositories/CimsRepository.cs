using CIMS.DTO;
using CIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIMS.Repositories
{
    public interface CimsRepository : IBaseRepository
    {
        object AddCimsValue(List<TblCimsattributeValue> Cims);
        object GetCimsvalue(string ModuleParent, int currPage, int recodperpage);

        object GetCustomerList_RecordId(string @RecordId);
        object EditCimsValue(List<TblCimsattributeValue> lstCustomer, string rowIdentify);
        int DeleteCims(string RecordId);

        #region vudt
        /// <summary>
        /// Lấy tất cả trường thông tin đã mã hóa, chưa mã hóa và đang mã hóa
        /// </summary>
        /// <returns>Danh sách trường thông tin để mã hóa, chưa mã hóa và đang mã hóa</returns>
        Task<ResponseDTO> GetListAttributesEncryption(string parentCode);
        /// <summary>
        /// Execute encrpytion and decrpytion attributes
        /// </summary>
        /// <param name="lstAttributes"></param>
        /// <return>Status code 200 is success, otherwise status code is 400</return>
        Task<ResponseDTO> ExecuteEncrpytion(AttributesDTO lstAttributes);
        #endregion

    }
}
