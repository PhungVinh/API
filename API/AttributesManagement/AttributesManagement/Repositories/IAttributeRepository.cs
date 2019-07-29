using AttributesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Repositories
{
    public interface IAttributeRepository : IBaseRepository
    {
        /// <summary>
        /// Create a new attribute
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns>Attribute just have been created</returns>
        object AddAttribute(InfoAttribute attributes);
        /// <summary>
        /// Update a attribute
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns>Attribute just have been updated</returns>
        object UpdateAttribute(InfoAttribute attributes);
        /// <summary>
        /// Get All Constraint
        /// </summary>
        /// <param name="TextSearch"></param>
        /// <param name="currPage"></param>
        /// <param name="recodperpage"></param>
        /// <returns></returns>
        object GetAllConstraint(string TextSearch, int currPage, int recodperpage);
        /// <summary>
        /// Get all Contraint
        /// </summary>
        /// <returns></returns>
        object GetAllConstraint1();
        /// <summary>
        /// get all parent Category
        /// </summary>
        /// <returns></returns>
        object GetAllParentCategory();
        /// <summary>
        /// Get all children Category
        /// </summary>
        /// <returns></returns>
        object GetAllChildCategory();
        /// <summary>
        /// Get all Attribute in cims
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        object GetAllAttributeCims(int formId);
        /// <summary>
        /// Delete a attribute by id
        /// </summary>
        /// <param name="attributesId"></param>
        /// <returns></returns>
        int DeleteAttributes(int attributesId);
        /// <summary>
        /// Get attributes by parent code
        /// </summary>
        /// <param name="menucode"></param>
        /// <returns>List attribute</returns>
        List<InfoAttribute> GetListAttributes(string moduleParent);
        /// <summary>
        /// get object attribute by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        object GetObjectAttributes(int Id);
        /// <summary>
        /// get all data type show
        /// </summary>
        /// <returns></returns>
        List<TblCategory> GetListDataType();
        /// <summary>
        /// get list object in attribute
        /// </summary>
        /// <returns></returns>
        List<TblCategory> GetListController();
        /// <summary>
        /// get list object in constraint
        /// </summary>
        /// <returns></returns>
        List<TblCategory> GetListControllerObject();
        /// <summary>
        /// Get requirement attributes by parent code
        /// </summary>
        /// <param name="menucode"></param>
        /// <returns>List requirement attribute</returns>
        List<TblAttributes> GetAllAttributeRequired(string menucode);
        /// <summary>
        /// get list constraint by Categorycode
        /// </summary>
        /// <param name="cateCode"></param>
        /// <returns></returns>
        List<TblAttributeConstraint> GetListConstraintByCateCode(string cateCode);
        /// <summary>
        ///  Create a new form
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        int AddFormCims(FormOptionValue addForm);
        /// <summary>
        /// Update form thông tin
        /// </summary>
        /// <param name="updateForm"></param>
        /// <returns></returns>
        int UpdateFormCims(FormOptionValue updateForm);
        int UpdateFormCimsList(UpdateFormDTO updateForm);
        List<TblCimsform> GetlistFormCims(string MenuCode);
        /// <summary>
        /// Create a new attribute constraint
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        object AddAttributeConstraint(TblAttributeConstraint addConstraint);
        /// <summary>
        /// Update a attribute constraint
        /// </summary>
        /// <param name="updateConstraint"></param>
        /// <returns></returns>
        object UpdateAttributeConstraint(TblAttributeConstraint updateConstraint);
        /// <summary>
        /// Update a attribute constraint by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        TblAttributeConstraint GetAttributesConstraintbyId(int Id);
        /// <summary>
        /// Delete a attribute constraint by id
        /// </summary>
        /// <param name="attributesId"></param>
        /// <returns></returns>
        int DeleteAttributesConstraint(int attributesId);
        /// <summary>
        /// Get All Attribute By Module
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        string GetStringCache(string cacheKey);
        void SetStringCache(string cacheKey, Object obj);
        void SetContextFactory(ConnectionStrings connectionStrings);
        #region vudt
        /// <summary>
        /// Get module's form by children code
        /// </summary>
        /// <param name="ChildCode">children code of module</param>
        /// <returns>form of children code</returns>
        object GetAllAttributesCimsWithRowDetails(string childCode);
        object UpdateAttributeFormList(TblCimsattributeForm tblCimsattributeForm);
        object UpdateTableFormList(List<InfoAttribute> table);
        #endregion
    }
}
