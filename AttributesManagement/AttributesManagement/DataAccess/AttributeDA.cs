using System;
using System.Collections.Generic;
using System.Linq;
using AttributesManagement.ContextFactory;
using AttributesManagement.Models;
using AttributesManagement.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using AttributesManagement.Constant;
using AttributesManagement.Common;
using System.Transactions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AttributesManagement.DataAccess
{
    public class AttributeDA : IAttributeRepository
    {

        public AttributeDA()
        {

        }
        private CRM_MPContext db = new CRM_MPContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MPContext>());

        private IDistributedCache _distributedCache;
        public string strconnect { get; set; }

        private SP_Attributes sp;

        #region Attribute
        /// <summary>
        /// Add new Attribute
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public object AddAttribute(InfoAttribute attributes)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    int code = 0;
                    int num = 0;
                    TblVocattributes addAttributes = new TblVocattributes();
                    TblVocattributes checkCode = db.TblVocattributes.Where(v => v.IsDelete == false).LastOrDefault();
                    if (checkCode == null)
                    {
                        num = 1;
                    }
                    else
                    {
                        var index = checkCode.AttributeCode.Replace(AttributeConstant.Attributes, "");
                        num = int.Parse(index) + 1;
                    }
                    addAttributes.AttributeCode = AttributeConstant.Attributes + num;
                    addAttributes.AttributeDescription = attributes.AttributeDescription;
                    addAttributes.DefaultValue = attributes.DefaultValue;
                    addAttributes.IsCategory = attributes.IsCategory;
                    addAttributes.AttributeType = attributes.AttributeType;
                    addAttributes.AttributeWidth = attributes.AttributeWidth;
                    addAttributes.AttributeLabel = attributes.AttributeLabel;
                    addAttributes.IsRequired = attributes.IsRequired;
                    addAttributes.IsTableShow = attributes.IsTableShow;
                    addAttributes.IsDuplicate = attributes.IsDuplicate;
                    addAttributes.DataType = attributes.DataType;
                    addAttributes.CreateBy = attributes.CreateBy;
                    addAttributes.CreateDate = DateTime.Now;
                    addAttributes.IsVisible = attributes.IsVisible;
                    addAttributes.DefaultValueWithTextBox = attributes.DefaultValueWithTextBox;
                    addAttributes.IsReuse = attributes.IsReuse;
                    addAttributes.CategoryParentCode = attributes.CategoryParentCode;
                    //addAttributes.DefaultValue = attributes.CategoryParentCode != "" ? attributes.DefaultValue : "";
                    addAttributes.IsDelete = false;
                    addAttributes.Encyption = false;
                    addAttributes.EncyptWaiting = false;
                    addAttributes.IsSort = attributes.IsSort;
                    addAttributes.ModuleParent = attributes.ModuleParent;
                    addAttributes.AttributeDescription = attributes.AttributeDescription;
                    db.TblVocattributes.Add(addAttributes);
                    code = db.SaveChanges();
                    if (attributes.DetailRefer.Count() > 0)
                    {
                        AddReferenceConstraint(attributes.DetailRefer, addAttributes.AttributeCode, addAttributes.ModuleParent);
                    }
                    SetStringCache(AttributeConstant.Attributes_GetListAttributes, GetListAttributes(addAttributes.ModuleParent));
                    scope.Complete();
                    object objAttributes = new { code = code, Id = addAttributes.AttributesId };
                    return objAttributes;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Update Attribute
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public object UpdateAttribute(InfoAttribute attributes)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    TblVocattributes checkData = db.TblVocattributes.Where(v => v.IsDelete == false && v.AttributeCode == attributes.AttributeCode).FirstOrDefault();
                    object objAttributes = new object();
                    int code = 0;
                    if (checkData != null)
                    {
                        if (checkData.AttributeLabel.ToLower() == attributes.AttributeLabel.ToLower())
                        {
                            checkData.AttributeType = attributes.AttributeType;
                            checkData.AttributeLabel = attributes.AttributeLabel;
                            checkData.AttributeDescription = attributes.AttributeDescription;
                            checkData.DefaultValueWithTextBox = attributes.DefaultValueWithTextBox != null ? attributes.DefaultValueWithTextBox : null;
                            checkData.AttributeWidth = attributes.AttributeWidth;
                            checkData.DataType = attributes.DataType;
                            checkData.DefaultValue = attributes.DefaultValue != null ? attributes.DefaultValue : null;
                            checkData.IsVisible = attributes.IsVisible;
                            checkData.IsReuse = attributes.IsReuse;
                            checkData.IsDuplicate = attributes.IsDuplicate;
                            checkData.IsRequired = attributes.IsRequired;
                            checkData.IsTableShow = attributes.IsTableShow;
                            checkData.UpDateBy = attributes.UpDateBy;
                            checkData.CategoryParentCode = attributes.CategoryParentCode != null ? attributes.CategoryParentCode : null;
                            checkData.UpdateDate = DateTime.Now;
                            //checkData.DefaultValue = attributes.CategoryParentCode != "" ? attributes.DefaultValue : "";
                            checkData.IsDelete = false;
                            checkData.ModuleParent = attributes.ModuleParent;
                            checkData.AttributeDescription = attributes.AttributeDescription;
                            db.Entry(checkData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            code = db.SaveChanges();

                            if (attributes.DetailRefer.Count() > 0)
                            {
                                AddReferenceConstraint(attributes.DetailRefer, checkData.AttributeCode, checkData.ModuleParent);
                            }
                            SetStringCache(AttributeConstant.Attributes_GetListAttributes, GetListAttributes(attributes.ModuleParent));                            
                            objAttributes = new { code = code, Id = checkData.AttributesId };
                        }
                        else
                        {
                            TblVocattributes checkAttributeName = db.TblVocattributes.Where(v => v.AttributeLabel.ToLower() == attributes.AttributeLabel.ToLower() && v.IsDelete == false).FirstOrDefault();
                            if (checkAttributeName != null)
                            {
                                objAttributes = new { code = code, Id = 0 };

                            }
                            else
                            {
                                //checkData.AttributeName = attributes.AttributeName.TrimStart().TrimEnd();
                                checkData.AttributeDescription = attributes.AttributeDescription;
                                checkData.AttributeWidth = attributes.AttributeWidth;
                                checkData.AttributeType = attributes.AttributeType;
                                checkData.AttributeLabel = attributes.AttributeLabel;
                                checkData.DefaultValueWithTextBox = attributes.DefaultValueWithTextBox != null ? attributes.DefaultValueWithTextBox : null;
                                checkData.DataType = attributes.DataType;
                                checkData.DefaultValue = attributes.DefaultValue != null ? attributes.DefaultValue : null;
                                checkData.IsVisible = attributes.IsVisible;
                                checkData.IsReuse = attributes.IsReuse;
                                checkData.IsTableShow = attributes.IsTableShow;
                                checkData.IsRequired = attributes.IsRequired;
                                checkData.IsDuplicate = attributes.IsDuplicate;
                                checkData.UpDateBy = attributes.UpDateBy;
                                checkData.UpdateDate = DateTime.Now;
                                checkData.CategoryParentCode = attributes.CategoryParentCode != null ? attributes.CategoryParentCode : null;
                                //checkData.DefaultValue = attributes.CategoryParentCode != "" ? attributes.DefaultValue : "";
                                checkData.IsDelete = false;
                                checkData.ModuleParent = attributes.ModuleParent;
                                checkData.AttributeDescription = attributes.AttributeDescription;
                                db.Entry(checkData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                code = db.SaveChanges();
                                if (attributes.DetailRefer.Count() > 0)
                                {
                                    AddReferenceConstraint(attributes.DetailRefer, checkData.AttributeCode, checkData.ModuleParent);
                                }
                                SetStringCache(AttributeConstant.Attributes_GetListAttributes, GetListAttributes(attributes.ModuleParent));
                                objAttributes = new { code = code, Id = checkData.AttributesId };
                            }
                        }
                    }
                    if (attributes.ChildCode != null)
                    {
                        var result = db.TblCimsattributeForm.Where(x => x.AttributeCode == attributes.AttributeCode && x.ChildCode == attributes.ChildCode).FirstOrDefault();
                        if (result != null)
                        {
                            result.DefaultValue = attributes.DefaultValue;
                            db.Entry(result).State = EntityState.Modified;
                            db.SaveChanges();
                        }                        
                    }
                    scope.Complete();
                    return objAttributes;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// List Attribute in form cims
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public object GetAllAttributeCims(int formId)
        {
            List<FormAttribute> lstFormAttr = new List<FormAttribute>();
            //1. Load form info
            TblCimsform tblCimsform = db.TblCimsform.Where(a => a.FormId == formId).FirstOrDefault();
            if (tblCimsform != null)
            {
                var lstObj = db.TblCimsattributeForm.Where(a => a.FormId == formId).GroupBy(a => a.RowIndex).OrderBy(a => a.Key).ToList();
                for (int i = 0; i < lstObj.Count; i++)
                {
                    FormAttribute formAttribute = new FormAttribute();
                    formAttribute.FormId = tblCimsform.FormId;
                    formAttribute.FormName = tblCimsform.FormName;
                    formAttribute.RowId = i + 1;
                    formAttribute.children = GetRowDetail(formId, i + 1);
                    lstFormAttr.Add(formAttribute);
                }
                return lstFormAttr;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Get row detail in form cims
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public List<AttributeObject> GetRowDetail(int formId, int rowId)
        {
            List<TblCimsattributeForm> lstAttribute = db.TblCimsattributeForm.Where(a => a.FormId == formId && a.RowIndex == rowId).OrderBy(a => a.AttrOrder).ToList();
            List<AttributeObject> lstAttrObject = new List<AttributeObject>();
            if (lstAttribute.Count > 0)
            {
                for (int i = 0; i < lstAttribute.Count; i++)
                {
                    TblVocattributes tblVocattributes = db.TblVocattributes.Where(a => a.AttributeCode == lstAttribute[i].AttributeCode).FirstOrDefault();
                    if (tblVocattributes != null)
                    {
                        List<AttributeOption> lstOption = new List<AttributeOption>();
                        AttributeObject attributeObject = new AttributeObject();
                        attributeObject.AttributeId = tblVocattributes.AttributesId;
                        attributeObject.AttributeCode = tblVocattributes.AttributeCode;
                        attributeObject.AttributeLabel = tblVocattributes.AttributeLabel;
                        attributeObject.AttributeType = tblVocattributes.AttributeType;
                        attributeObject.AttributeCol = lstAttribute[i].AttributeColumn.Value;
                        attributeObject.DataType = tblVocattributes.DataType;
                        attributeObject.DefaultValue = tblVocattributes.DefaultValue;
                        attributeObject.IsCategory = tblVocattributes.IsCategory.HasValue ? tblVocattributes.IsCategory.Value : false;
                        attributeObject.IsDuplicate = tblVocattributes.IsDuplicate.HasValue ? tblVocattributes.IsDuplicate.Value : false;
                        attributeObject.IsRequired = tblVocattributes.IsRequired.HasValue ? tblVocattributes.IsRequired.Value : false;
                        attributeObject.IsTableShow = tblVocattributes.IsTableShow.HasValue ? tblVocattributes.IsTableShow.Value : false;
                        if (tblVocattributes.IsCategory.HasValue)
                        {
                            List<TblCategory> tblCategories = db.TblCategory.Where(a => a.CategoryTypeCode == tblVocattributes.CategoryParentCode).ToList();
                            for (int j = 0; j < tblCategories.Count; j++)
                            {
                                AttributeOption attributeOption = new AttributeOption();
                                attributeOption.OptionValue = tblCategories[j].CategoryCode;
                                attributeOption.OptionText = tblCategories[j].CategoryName;
                                lstOption.Add(attributeOption);
                            }
                        }
                        attributeObject.children = lstOption;
                        lstAttrObject.Add(attributeObject);
                    }
                }
                return lstAttrObject;
            }
            return null;
        }
        /// <summary>
        /// Delete attribute by attributeId
        /// </summary>
        /// <param name="attributesId"></param>
        /// <returns></returns>
        public int DeleteAttributes(int attributesId)
        {
            int code = 0;
            TblAttributeOptions check = db.TblAttributeOptions.Where(v => v.AttributeId == attributesId && v.IsDelete == false).FirstOrDefault();
            if (check == null)
            {
                TblVocattributes deleteAttributes = db.TblVocattributes.Where(c => c.AttributesId == attributesId && c.IsDelete == false).FirstOrDefault();
                deleteAttributes.IsDelete = true;
                db.Entry(deleteAttributes).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
                code = 1;
            }
            return code;
        }
        /// <summary>
        /// list attribute by module
        /// </summary>
        /// <param name="moduleParent"></param>
        /// <returns></returns>
        public List<InfoAttribute> GetListAttributes(string moduleParent)
        {
            List<InfoAttribute> lstAddtribute = new List<InfoAttribute>();
            List<TblVocattributes> lst = db.TblVocattributes.Where(v => v.ModuleParent == moduleParent && v.IsDelete == false).ToList();
            foreach (var item in lst)
            {
                InfoAttribute addAddtribute = new InfoAttribute();
                addAddtribute.DetailRefer = getArrayString(item.AttributeCode);
                addAddtribute.AttributeCode = item.AttributeCode;
                addAddtribute.ChildCode = item.ModuleParent;
                addAddtribute.AttributeLabel = item.AttributeLabel;
                addAddtribute.AttributesId = item.AttributesId;
                addAddtribute.AttributeType = item.AttributeType;
                addAddtribute.AttributeWidth = item.AttributeWidth;
                addAddtribute.CategoryParentCode = item.CategoryParentCode;
                addAddtribute.DefaultValueWithTextBox = item.DefaultValueWithTextBox;
                addAddtribute.IsSort = item.IsSort;
                addAddtribute.CreateBy = item.CreateBy;
                addAddtribute.CreateDate = item.CreateDate;
                addAddtribute.DataType = item.DataType;
                addAddtribute.IsRequired = item.IsRequired;
                addAddtribute.IsTableShow = item.IsTableShow;
                addAddtribute.IsVisible = item.IsVisible;
                addAddtribute.IsReuse = item.IsReuse;
                addAddtribute.Disabled = item.Disabled;
                addAddtribute.IsDuplicate = item.IsDuplicate;
                addAddtribute.IndexTitleTable = item.IndexTitleTable;
                addAddtribute.DefaultValue = item.DefaultValue;
                addAddtribute.ModuleParent = item.ModuleParent;
                addAddtribute.AttributeDescription = item.AttributeDescription;
                if (addAddtribute.CategoryParentCode != null)
                {
                    var result = db.TblCategory.Where(x => x.CategoryTypeCode == addAddtribute.CategoryParentCode).ToList();
                    if (result.Count > 0)
                    {
                        foreach (var item1 in result)
                        {
                            addAddtribute.ListCategory.Add(item1);
                        }
                    }
                }
                lstAddtribute.Add(addAddtribute);
            }
            return lstAddtribute;
        }
        /// <summary>
        /// list object attributes by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public object GetObjectAttributes(int Id)
        {
            List<InfoAttribute> lstAddtribute = new List<InfoAttribute>();
            List<TblVocattributes> lst = db.TblVocattributes.Where(v => v.AttributesId == Id && v.IsDelete == false).ToList();
            foreach (var item in lst)
            {
                InfoAttribute addAddtribute = new InfoAttribute();
                addAddtribute.DetailRefer = getArrayString(item.AttributeCode);
                addAddtribute.AttributeCode = item.AttributeCode;
                addAddtribute.AttributeLabel = item.AttributeLabel;
                addAddtribute.AttributesId = item.AttributesId;
                addAddtribute.AttributeType = item.AttributeType;
                addAddtribute.AttributeWidth = item.AttributeWidth;
                addAddtribute.CategoryParentCode = item.CategoryParentCode;
                addAddtribute.DefaultValueWithTextBox = item.DefaultValueWithTextBox;
                addAddtribute.CreateBy = item.CreateBy;
                addAddtribute.CreateDate = item.CreateDate;
                addAddtribute.DataType = item.DataType;
                addAddtribute.IsRequired = item.IsRequired;
                addAddtribute.IsTableShow = item.IsTableShow;
                addAddtribute.IsVisible = item.IsVisible;
                addAddtribute.IsReuse = item.IsReuse;
                addAddtribute.IsDuplicate = item.IsDuplicate;
                addAddtribute.DefaultValue = item.DefaultValue;
                addAddtribute.ModuleParent = item.ModuleParent;
                addAddtribute.AttributeDescription = item.AttributeDescription;
                if (item.CategoryParentCode != null)
                {
                    var result = db.TblCategory.Where(x => x.CategoryTypeCode == item.CategoryParentCode).ToList();
                    if (result.Count > 0)
                    {
                        foreach (var item1 in result)
                        {
                            addAddtribute.ListCategory.Add(item1);
                        }
                    }
                }
                lstAddtribute.Add(addAddtribute);
            }
            return lstAddtribute;
        }
        /// <summary>
        /// get list object constraint by attributecode
        /// </summary>
        /// <param name="AttributeCode"></param>
        /// <returns></returns>
        private string[] getArrayString(string AttributeCode)
        {
            var lst = (from a in db.TblReferenceConstraint
                       where (a.AttributeCode == AttributeCode)
                       select new
                       {
                           a.ConstraintId
                       }
                       );

            return lst.Select(s => s.ConstraintId.ToString()).ToArray();
        }
       
        #region Ràng buộc dữ liệu
        /// <summary>
        /// Add new Attribute Constraint
        /// </summary>
        /// <param name="addConstraint"></param>
        /// <returns></returns>
        public object AddAttributeConstraint(TblAttributeConstraint addConstraint)
        {
            try
            {
                int code = 0;
                object objAttributesConstraint = new object();
                string IsDuplicate = LocDau(AttributeConstant.IsDuplicate).ToLower().TrimStart().TrimEnd();
                string IsTableShow = LocDau(AttributeConstant.IsTableShow).ToLower().TrimStart().TrimEnd();
                string IsTableShow1 = LocDau(AttributeConstant.IsTableShow1).ToLower().TrimStart().TrimEnd();
                string IsRequired = LocDau(AttributeConstant.IsRequired).ToLower().TrimStart().TrimEnd();
                string constraintName = LocDau(addConstraint.Name).ToLower().TrimStart().TrimEnd();
                if (constraintName == IsRequired || constraintName == IsTableShow || constraintName == IsTableShow1 || constraintName == IsDuplicate)
                {
                    objAttributesConstraint = new { code = code, Id = 0 };
                }
                else
                {
                    TblAttributeConstraint checkdata = db.TblAttributeConstraint.Where(v => v.Name == addConstraint.Name && v.IsDelete == false).FirstOrDefault();
                    if (checkdata == null)
                    {
                        TblAttributeConstraint addAttributeConstraint = new TblAttributeConstraint();
                        addAttributeConstraint.Name = addConstraint.Name;
                        addAttributeConstraint.ContraintsType = addConstraint.ContraintsType;
                        addAttributeConstraint.LinkContraints = addConstraint.LinkContraints;
                        addAttributeConstraint.ContraintsValue = addConstraint.LinkContraints != "" ? addConstraint.ContraintsValue : "";
                        addAttributeConstraint.CreateBy = addConstraint.CreateBy;
                        addAttributeConstraint.CreateDate = DateTime.Now;
                        addAttributeConstraint.IsDelete = false;
                        db.TblAttributeConstraint.Add(addAttributeConstraint);
                        code = db.SaveChanges();
                        objAttributesConstraint = new { code = code, Id = addAttributeConstraint.Id };
                    }
                    else
                    {
                        objAttributesConstraint = new { code = code, Id = 0 };
                    }
                }
                return objAttributesConstraint;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// add new Refernce Constraint
        /// </summary>
        /// <param name="constraintType"></param>
        /// <param name="attributeCode"></param>
        /// <param name="MenuCode"></param>
        public void AddReferenceConstraint(string[] constraintType, string attributeCode, string MenuCode)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    List<TblReferenceConstraint> lstReferneceConstraint = db.TblReferenceConstraint.Where(v => v.AttributeCode == attributeCode && v.MenuCode == MenuCode).ToList();
                    if (lstReferneceConstraint.Count > 0)
                    {
                        db.TblReferenceConstraint.RemoveRange(lstReferneceConstraint);
                        db.SaveChanges();
                    }
                    if (constraintType != null)
                    {
                        foreach (string item in constraintType)
                        {
                            TblReferenceConstraint addReferenceConstraint = new TblReferenceConstraint();
                            addReferenceConstraint.ConstraintId = Int32.Parse(item);
                            addReferenceConstraint.AttributeCode = attributeCode;
                            addReferenceConstraint.MenuCode = MenuCode;
                            db.TblReferenceConstraint.Add(addReferenceConstraint);
                            db.SaveChanges();
                        }
                    }
                    scope1.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// update Attribute Constraint
        /// </summary>
        /// <param name="updateConstraint"></param>
        /// <returns></returns>
        public object UpdateAttributeConstraint(TblAttributeConstraint updateConstraint)
        {
            try
            {
                int code = 0;
                object objAttributesConstraint = new object();
                string IsDuplicate = LocDau(AttributeConstant.IsDuplicate).ToLower().TrimStart().TrimEnd();
                string IsTableShow = LocDau(AttributeConstant.IsTableShow).ToLower().TrimStart().TrimEnd();
                string IsTableShow1 = LocDau(AttributeConstant.IsTableShow1).ToLower().TrimStart().TrimEnd();
                string IsRequired = LocDau(AttributeConstant.IsRequired).ToLower().TrimStart().TrimEnd();
                string constraintName = LocDau(updateConstraint.Name).ToLower().TrimStart().TrimEnd();
                if (constraintName == IsRequired || constraintName == IsTableShow || constraintName == IsTableShow1 || constraintName == IsDuplicate)
                {
                    objAttributesConstraint = new { code = code, Id = 0 };
                }
                else
                {
                    TblAttributeConstraint checkConstraint = db.TblAttributeConstraint.Where(v => v.Id == updateConstraint.Id).FirstOrDefault();
                    if (checkConstraint != null)
                    {
                        if (checkConstraint.Name.ToLower() == updateConstraint.Name.ToLower())
                        {
                            checkConstraint.Name = updateConstraint.Name;
                            checkConstraint.ContraintsType = updateConstraint.ContraintsType;
                            checkConstraint.LinkContraints = updateConstraint.LinkContraints;
                            checkConstraint.ContraintsValue = updateConstraint.LinkContraints != "" ? updateConstraint.ContraintsValue : "";
                            checkConstraint.UpdateBy = updateConstraint.UpdateBy;
                            checkConstraint.UpdateDate = DateTime.Now;
                            checkConstraint.IsDelete = false;
                            db.Entry(checkConstraint).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            code = db.SaveChanges();
                            objAttributesConstraint = new { code = code, Id = checkConstraint.Id };
                        }
                        else
                        {
                            TblAttributeConstraint checkConstraintName = db.TblAttributeConstraint.Where(v => v.Name.ToLower() == updateConstraint.Name.ToLower()).FirstOrDefault();
                            if (checkConstraintName == null)
                            {
                                checkConstraint.Name = updateConstraint.Name;
                                checkConstraint.ContraintsType = updateConstraint.ContraintsType;
                                checkConstraint.LinkContraints = updateConstraint.LinkContraints;
                                checkConstraint.ContraintsValue = updateConstraint.LinkContraints != "" ? updateConstraint.ContraintsValue : "";
                                checkConstraint.UpdateBy = updateConstraint.UpdateBy;
                                checkConstraint.UpdateDate = DateTime.Now;
                                checkConstraint.IsDelete = false;
                                db.Entry(checkConstraint).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                code = db.SaveChanges();
                                objAttributesConstraint = new { code = code, Id = checkConstraint.Id };
                            }
                            else
                            {
                                objAttributesConstraint = new { code = code, Id = 0 };
                            }
                        }
                    }
                }

                return objAttributesConstraint;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TblAttributeConstraint GetAttributesConstraintbyId(int Id)
        {
            TblAttributeConstraint getAttributesConstraint = db.TblAttributeConstraint.Where(v => v.Id == Id && v.IsDelete == false).FirstOrDefault();
            if (getAttributesConstraint != null)
            {
                return getAttributesConstraint;
            }
            else
                return null;
        }
        /// <summary>
        /// Delete attribute Constraint
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteAttributesConstraint(int Id)
        {
            try
            {
                int code = 0;
                TblReferenceConstraint checkConstraint = db.TblReferenceConstraint.Where(v => v.ConstraintId == Id).FirstOrDefault();
                if (checkConstraint == null)
                {
                    TblAttributeConstraint attributeConstraint = db.TblAttributeConstraint.Where(v => v.Id == Id).FirstOrDefault();
                    attributeConstraint.IsDelete = true;
                    db.Entry(attributeConstraint).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    code = db.SaveChanges();
                }
                return code;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
        #endregion
        #region Form CIMS
        /// <summary>
        /// add new Form cims
        /// </summary>
        /// <param name="cimsForm"></param>
        /// <returns></returns>
        public int AddFormCims(FormOptionValue cimsForm)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int code = 0;
                try
                {
                    db.TblCimsform.Add(cimsForm.tblCimsForm);
                    code = db.SaveChanges();
                    int formId = cimsForm.tblCimsForm.FormId != 0 ? cimsForm.tblCimsForm.FormId : 0;
                    if (formId != 0)
                    {
                        AddAttributeForm(cimsForm.tblCimsattributeForm, cimsForm.tblCimsForm.ChildCode, formId);
                        var attCreateBy = db.TblVocattributes.Where(x => x.AttributeCode == AttributeConstant.Attributes_CreateBy).FirstOrDefault();
                        attCreateBy.DefaultValue = cimsForm.tblCimsForm.CreateBy;
                        attCreateBy.DefaultValueWithTextBox = cimsForm.tblCimsForm.CreateBy;
                        db.Entry(attCreateBy).State = EntityState.Modified;
                        var attCreateDate = db.TblVocattributes.Where(x => x.AttributeCode == AttributeConstant.Attributes_CreateDate).FirstOrDefault();
                        attCreateDate.DefaultValue = String.Format("{0:r}", cimsForm.tblCimsForm.CreateDate);
                        db.Entry(attCreateDate).State = EntityState.Modified;
                        var result = db.TblCimsform.Where(x => x.ChildCode == cimsForm.tblCimsForm.ChildCode).FirstOrDefault();
                        if (result != null)
                        {
                            var formHistory = new TblCimsFormHistory();
                            formHistory.ChildCode = result.ChildCode;
                            formHistory.Description = AttributeConstant.CreateForm;
                            formHistory.CreateBy = result.CreateBy;
                            formHistory.CreateDate = result.CreateDate;                            
                            db.TblCimsFormHistory.Add(formHistory);
                            db.SaveChanges();
                        }
                    }
                    SetStringCache(AttributeConstant.Form_GetListFormCims, GetlistFormCims(cimsForm.tblCimsForm.MenuCode));
                    scope.Complete();
                    return code;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// update form cims
        /// </summary>
        /// <param name="updateForm"></param>
        /// <returns></returns>
        public int UpdateFormCims(FormOptionValue updateForm)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int code = 0;
                try
                {
                    TblCimsform editForm = db.TblCimsform.Where(v => v.ChildCode == updateForm.tblCimsForm.ChildCode && v.IsDelete == false).FirstOrDefault();
                    if (editForm != null)
                    {
                        editForm.FormName = updateForm.tblCimsForm.FormName;
                        editForm.FormDescription = updateForm.tblCimsForm.FormDescription;
                        editForm.IsContinute = updateForm.tblCimsForm.IsContinute;
                        editForm.IsDelete = false;
                        editForm.UpdateBy = updateForm.tblCimsForm.CreateBy;
                        editForm.UpdateDate = DateTime.Now;
                        db.Entry(editForm).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        code = db.SaveChanges();
                        AddAttributeForm(updateForm.tblCimsattributeForm, updateForm.tblCimsForm.ChildCode, editForm.FormId);
                        var formHistory = new TblCimsFormHistory();
                        formHistory.ChildCode = updateForm.tblCimsForm.ChildCode;
                        formHistory.Description = AttributeConstant.UpdateForm;
                        formHistory.UpdateBy = updateForm.tblCimsForm.UpdateBy;
                        formHistory.UpdateDate = updateForm.tblCimsForm.UpdateDate;
                        db.TblCimsFormHistory.Add(formHistory);
                        db.SaveChanges();
                        SetStringCache(AttributeConstant.Form_GetListFormCims, GetlistFormCims(editForm.MenuCode));
                        scope.Complete();
                        GetStringCache(AttributeConstant.Form_GetListFormCims);
                        return code;
                    }
                    return code;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// add attribute form(insert row,column)
        /// </summary>
        /// <param name="cimsAttributesForm"></param>
        /// <param name="ChildCode"></param>
        /// <param name="formId"></param>
        public void AddAttributeForm(List<TblCimsattributeForm> cimsAttributesForm, string ChildCode, int formId)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    List<TblCimsattributeForm> checkForm = db.TblCimsattributeForm.Where(v => v.ChildCode == ChildCode).ToList();
                    if (checkForm.Count > 0)
                    {
                        db.TblCimsattributeForm.RemoveRange(checkForm);
                        db.SaveChanges();
                    }
                    if (cimsAttributesForm.Count() > 0)
                    {
                        foreach (TblCimsattributeForm item in cimsAttributesForm)
                        {
                            TblCimsattributeForm addCimsForm = new TblCimsattributeForm();
                            addCimsForm.FormId = formId;
                            addCimsForm.ChildCode = ChildCode;
                            addCimsForm.AttributeCode = item.AttributeCode;
                            addCimsForm.AttributeColumn = item.AttributeColumn;
                            addCimsForm.AttrOrder = item.AttrOrder;
                            addCimsForm.RowTitle = item.RowTitle;
                            addCimsForm.RowIndex = item.RowIndex;
                            addCimsForm.AttributeType = item.AttributeType;
                            addCimsForm.IsShowLabel = item.IsShowLabel;
                            addCimsForm.DefaultValue = item.DefaultValue;
                            db.TblCimsattributeForm.Add(addCimsForm);
                            db.SaveChanges();
                        }
                    }
                    foreach (var item in cimsAttributesForm)
                    {
                        var result = db.TblVocattributes.Where(x => x.AttributeCode == item.AttributeCode).FirstOrDefault();
                        //result.Disabled = item.Dis;
                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    scope1.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// get list form cims
        /// </summary>
        /// <param name="MenuCode"></param>
        /// <returns></returns>
        public List<TblCimsform> GetlistFormCims(string MenuCode)
        {
            List<TblCimsform> lstCimsForm = db.TblCimsform.Where(c => c.MenuCode == MenuCode && c.IsDelete == false).ToList();
            return lstCimsForm;
        }
        /// <summary>
        /// get all attribute required to add new form
        /// </summary>
        /// <param name="menucode"></param>
        /// <returns></returns>
        public List<TblVocattributes> GetAllAttributeRequired(string menucode)
        {
            List<TblVocattributes> lstAttribute = db.TblVocattributes.Where(v => v.IsDelete == false && v.ModuleParent == menucode && v.IsRequired == true).ToList();
            return lstAttribute;
        }
        #endregion
        #region Form VOC

        #endregion

        public void LoadContext(string orgCode, IDistributedCache distributedCache)
        {
            if (db == null || strconnect != orgCode)
            {
                string conn = AttributeConstant.MP_CONNECTION;
                var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                optionsBuilder.UseSqlServer(conn.Replace(AttributeConstant.DATABASE_MP, orgCode), providerOptions => providerOptions.CommandTimeout(300));
                db = new CRM_MPContext(optionsBuilder.Options);
            }
            sp = new SP_Attributes(db);
            _distributedCache = distributedCache;
        }
        public void SetContextFactory(ConnectionStrings connectionStrings)
        {
            //ConnectionStrings connectionStrings = new ConnectionStrings();
            //connectionStrings.ConnectionKey = orgCode + AttributeConstant.CONNECTION_CONFIG;
            //connectionStrings.ConnectionValue = AttributeConstant.SQL_CONNECTION.Replace(AttributeConstant.DATABASE_MASTER, AttributeConstant.DATABASE_PREFIX + orgCode);
            DbContextFactory.AddChildContext(connectionStrings);
        }
        #region Cache
        public string GetStringCache(string cacheKey)
        {
            return _distributedCache.GetString(cacheKey);
        }

        public void SetStringCache(string cacheKey, Object obj)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            _distributedCache.SetString(cacheKey, Newtonsoft.Json.JsonConvert.SerializeObject(obj), options.SetSlidingExpiration(TimeSpan.FromMinutes(15)));

        }
        #region get list dataType,Control
        public List<TblCategory> GetListDataType()
        {
            List<TblCategory> lstDataType = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryTypeCode == AttributeConstant.DataType).ToList();
            return lstDataType;
        }
        public List<TblCategory> GetListController()
        {
            List<TblCategory> lstController = db.TblCategory.Where(v => v.CategoryTypeCode == AttributeConstant.Control && v.IsDelete == false).ToList();
            return lstController;
        }
        public List<TblCategory> GetListControllerObject()
        {
            List<TblCategory> lstController = db.TblCategory.Where(v => v.CategoryTypeCode == AttributeConstant.Object && v.IsDelete == false).ToList();
            return lstController;
        }
        public List<TblAttributeConstraint> GetListConstraintByCateCode(string cateCode)
        {
            List<TblAttributeConstraint> lstConstraint = db.TblAttributeConstraint.Where(v => v.IsDelete == false && v.ControlType == cateCode).ToList();
            return lstConstraint;
        }
        /// <summary>
        /// Tìm kiếm Ràng buộc
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetAllConstraint(string TextSearch, int currPage, int recodperpage)
        {
            try
            {
                List<List<dynamic>> obj = sp.GetAllConstraint(TextSearch, currPage, recodperpage);
                var response = new { data = obj[0], paging = obj[1] };
                return response;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
        /// <summary>
        /// Get All Contraints
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetAllConstraint1()
        {
            try
            {
                var objectConstraint = from a in db.TblAttributeConstraint
                                       join c in db.TblCategory on a.ContraintsType equals c.CategoryCode
                                       where a.IsDelete == false
                                       select new
                                       {
                                           a.Id,
                                           a.Name,
                                           c.CategoryName
                                       };
                return objectConstraint;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
        /// <summary>
        /// Get All Parent Category
        /// </summary>
        /// <returns></returns>
        public object GetAllParentCategory()
        {
            try
            {
                var objectParentCategory = from a in db.TblCategory
                                           where (a.IsDelete == false && (a.CategoryTypeCode == null || a.CategoryTypeCode == ""))
                                           select new
                                           {
                                               a.Id,
                                               a.CategoryName,
                                               a.CategoryCode

                                           };
                return objectParentCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// get all Child Category
        /// </summary>
        /// <returns></returns>
        public object GetAllChildCategory()
        {
            try
            {
                var objectChildCategory = from a in db.TblCategory
                                          where a.IsDelete == false && (a.CategoryTypeCode != null || a.CategoryTypeCode != "")
                                          select new
                                          {
                                              a.Id,
                                              a.CategoryName,
                                              a.CategoryCode,
                                              a.CategoryTypeCode
                                          };
                return objectChildCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion
        public static string LocDau(string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < CategoryConstant.VietNamChar.Length; i++)
            {
                for (int j = 0; j < CategoryConstant.VietNamChar[i].Length; j++)
                    str = str.Replace(CategoryConstant.VietNamChar[i][j], CategoryConstant.VietNamChar[0][i - 1]);
            }
            return str;
        }


        #region vudt1
        public object GetAllAttributesCimsWithRowDetails(string ChildCode)
        {
            try
            {
                List<AllAttributesRowDetailsDTO> lstFormAttr = new List<AllAttributesRowDetailsDTO>();
                TblCimsform tblCimsform = db.TblCimsform.Where(a => a.ChildCode == ChildCode).FirstOrDefault();
                if (tblCimsform != null)
                {
                    var lstObj = db.TblCimsattributeForm.Where(a => a.ChildCode == ChildCode).GroupBy(a => a.RowIndex).OrderBy(a => a.Key).ToList();
                    for (int i = 0; i < lstObj.Count; i++)
                    {
                        AllAttributesRowDetailsDTO formAttribute = new AllAttributesRowDetailsDTO();
                        formAttribute.FormId = tblCimsform.FormId;
                        formAttribute.FormName = tblCimsform.FormName;
                        formAttribute.RowTitle = lstObj[i].FirstOrDefault().RowTitle;
                        formAttribute.RowId = i + 1;
                        formAttribute.children = GetRowDetailAttribute(ChildCode, i + 1);
                        lstFormAttr.Add(formAttribute);
                    }
                    //SetStringCache(AttributeConstant.Attributes_GetFormWithChildCode, lstFormAttr);
                    //return GetStringCache(AttributeConstant.Attributes_GetFormWithChildCode);
                    return lstFormAttr;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message };
            }
            
        }
        public object GetRowDetailAttribute(string childCode, int rowId)
        {
            try
            {
                List<TblCimsattributeForm> lstAttribute = db.TblCimsattributeForm.Where(a => a.ChildCode == childCode && a.RowIndex == rowId).OrderBy(a => a.AttrOrder).ToList();
                List<AttributesObjectDTO> lstAttrObject = new List<AttributesObjectDTO>();
                if (lstAttribute.Count > 0)
                {
                    for (int i = 0; i < lstAttribute.Count; i++)
                    {
                        TblVocattributes tblVocattributes = db.TblVocattributes.Where(a => a.AttributeCode == lstAttribute[i].AttributeCode && a.IsDelete == false).FirstOrDefault();
                        if (tblVocattributes != null)
                        {
                            List<AttributeOption> lstOption = new List<AttributeOption>();
                            AttributesObjectDTO attributeObject = new AttributesObjectDTO();
                            attributeObject.AttributeId = tblVocattributes.AttributesId;
                            attributeObject.AttributeCode = tblVocattributes.AttributeCode;
                            attributeObject.AttributeLabel = tblVocattributes.AttributeLabel;
                            attributeObject.AttributeType = lstAttribute[i].AttributeType;
                            attributeObject.AttributeDescription = tblVocattributes.AttributeDescription;
                            attributeObject.AttributeWidth = tblVocattributes.AttributeWidth;
                            attributeObject.IsReuse = tblVocattributes.IsReuse;
                            attributeObject.IsVisible = tblVocattributes.IsVisible;
                            attributeObject.IsDuplicate = tblVocattributes.IsDuplicate;
                            attributeObject.DetailRefer = getArrayString(tblVocattributes.AttributeCode);
                            attributeObject.AttributeCol = lstAttribute[i].AttributeColumn.Value;
                            attributeObject.AttrOrder = lstAttribute[i].AttrOrder.Value;
                            attributeObject.IsShowLabel = lstAttribute[i].IsShowLabel;
                            attributeObject.DefaultValue = lstAttribute[i].DefaultValue;
                            attributeObject.IsDuplicate = tblVocattributes.IsDuplicate;
                            attributeObject.RowIndex = lstAttribute[i].RowIndex.Value;
                            attributeObject.AttributeColumn = lstAttribute[i].AttributeColumn.Value;
                            attributeObject.DataType = tblVocattributes.DataType;
                            attributeObject.CategoryParentCode = tblVocattributes.CategoryParentCode;
                            attributeObject.DefaultValueWithTextBox = tblVocattributes.DefaultValueWithTextBox;
                            attributeObject.IsCategory = tblVocattributes.IsCategory.HasValue ? tblVocattributes.IsCategory.Value : false;
                            attributeObject.IsRequired = tblVocattributes.IsRequired.HasValue ? tblVocattributes.IsRequired.Value : false;
                            attributeObject.IsTableShow = tblVocattributes.IsTableShow.HasValue ? tblVocattributes.IsTableShow.Value : false;
                            if (tblVocattributes.IsCategory.HasValue)
                            {
                                List<TblCategory> tblCategories = db.TblCategory.Where(a => a.CategoryTypeCode == tblVocattributes.CategoryParentCode).ToList();
                                for (int j = 0; j < tblCategories.Count; j++)
                                {
                                    AttributeOption attributeOption = new AttributeOption();
                                    attributeOption.OptionValue = tblCategories[j].CategoryCode;
                                    attributeOption.OptionText = tblCategories[j].CategoryName;
                                    lstOption.Add(attributeOption);
                                }
                            }
                            if (tblVocattributes.CategoryParentCode != null)
                            {
                                var result = db.TblCategory.Where(x => x.CategoryTypeCode == tblVocattributes.CategoryParentCode).ToList();
                                if (result != null)
                                {
                                    foreach (var item in result)
                                    {
                                        attributeObject.ListCategory.Add(item);
                                    }
                                }                                
                            }
                            attributeObject.children = lstOption;
                            lstAttrObject.Add(attributeObject);
                        }
                    }
                    return lstAttrObject;
                }
                return null;
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message };
            }
            
        }
        public int UpdateFormCimsList(UpdateFormDTO updateForm)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int code = 0;
                try
                {
                    TblCimsform editForm = db.TblCimsform.Where(v => v.ChildCode == updateForm.tblCimsForm.ChildCode && v.IsDelete == false).FirstOrDefault();
                    if (editForm != null)
                    {
                        editForm.FormName = updateForm.tblCimsForm.FormName;
                        editForm.FormDescription = updateForm.tblCimsForm.FormDescription;
                        editForm.IsContinute = updateForm.tblCimsForm.IsContinute;
                        editForm.IsDelete = false;
                        editForm.UpdateBy = updateForm.tblCimsForm.UpdateBy;
                        editForm.UpdateDate = updateForm.tblCimsForm.UpdateDate;
                        db.Entry(editForm).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        AddAttributeForm(updateForm.tblCimsattributeForm, updateForm.tblCimsForm.ChildCode, editForm.FormId);
                        if (updateForm.Table.Count() > 0 && updateForm.tblCimsForm.ChildCode == AttributeConstant.ListForm)
                        {
                            foreach (var item in updateForm.Table)
                            {
                                var result = db.TblVocattributes.Where(x => x.AttributeCode == item.AttributeCode && x.ModuleParent == item.ModuleParent && x.IsDelete == false).FirstOrDefault();
                                if(result != null)
                                {
                                    result.IsTableShow = item.IsTableShow;
                                    result.IndexTitleTable = item.IndexTitleTable;
                                    result.IsSort = item.IsSort;
                                    db.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                }
                            }
                            var lst = db.TblVocattributes.Where(x => !updateForm.Table.Where(c => c.AttributeCode == x.AttributeCode && c.ModuleParent == x.ModuleParent && x.IsDelete == false).Select(c => c.AttributeCode).Contains(x.AttributeCode)).ToList();
                            foreach (var item in lst)
                            {
                                item.IsTableShow = false;
                                item.IndexTitleTable = null;
                                item.IsSort = null;
                                db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                        else
                        {
                            var lst = db.TblVocattributes.ToList();
                            foreach (var item in lst)
                            {
                                item.IsTableShow = false;
                                item.IndexTitleTable = null;
                                item.IsSort = null;
                                db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                        var formHistory = new TblCimsFormHistory();
                        formHistory.ChildCode = updateForm.tblCimsForm.ChildCode;
                        formHistory.Description = AttributeConstant.UpdateForm;
                        formHistory.UpdateBy = updateForm.tblCimsForm.UpdateBy;
                        formHistory.UpdateDate = updateForm.tblCimsForm.UpdateDate;
                        db.TblCimsFormHistory.Add(formHistory);
                        code = db.SaveChanges();
                        SetStringCache(AttributeConstant.Form_GetListFormCims, GetlistFormCims(editForm.MenuCode));
                        scope.Complete();
                        GetStringCache(AttributeConstant.Form_GetListFormCims);
                        return code;
                    }
                    return code;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public object UpdateAttributeFormList(TblCimsattributeForm tblCimsattributeForm)
        {
            try
            {
                var result = db.TblCimsattributeForm.Where(x => x.ChildCode == tblCimsattributeForm.ChildCode && x.AttributeCode == tblCimsattributeForm.AttributeCode).FirstOrDefault();
                if (result != null)
                {
                    result.DefaultValue = tblCimsattributeForm.DefaultValue;
                    result.AttributeType = tblCimsattributeForm.AttributeType;
                    result.IsShowLabel = tblCimsattributeForm.IsShowLabel;
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();
                    return result;
                }
                return AttributesMessages.MS00011;
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message };
            }
        }
        public int AddFormCimsList(UpdateFormDTO cimsForm)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int code = 0;
                try
                {
                    db.TblCimsform.Add(cimsForm.tblCimsForm);
                    code = db.SaveChanges();
                    int formId = cimsForm.tblCimsForm.FormId != 0 ? cimsForm.tblCimsForm.FormId : 0;
                    if (formId != 0)
                    {
                        AddAttributeForm(cimsForm.tblCimsattributeForm, cimsForm.tblCimsForm.ChildCode, formId);
                        if (cimsForm.Table.Count() > 0 && cimsForm.tblCimsForm.ChildCode == AttributeConstant.ListForm)
                        {
                            foreach (var item in cimsForm.Table)
                            {
                                var result1 = db.TblVocattributes.Where(x => x.AttributeCode == item.AttributeCode && x.ModuleParent == item.ModuleParent && x.IsDelete == false).FirstOrDefault();
                                if (result1 != null)
                                {
                                    result1.IsTableShow = item.IsTableShow;
                                    result1.IndexTitleTable = item.IndexTitleTable;
                                    result1.IsSort = item.IsSort;
                                    db.Entry(result1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                }
                            }
                            var lst = db.TblVocattributes.Where(x => !cimsForm.Table.Where(c => c.AttributeCode == x.AttributeCode && c.ModuleParent == x.ModuleParent && x.IsDelete == false).Select(c => c.AttributeCode).Contains(x.AttributeCode)).ToList();
                            foreach (var item in lst)
                            {
                                item.IsTableShow = false;
                                item.IndexTitleTable = null;
                                item.IsSort = null;
                                db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                        else
                        {
                            var lst = db.TblVocattributes.ToList();
                            foreach (var item in lst)
                            {
                                item.IsTableShow = false;
                                item.IndexTitleTable = null;
                                item.IsSort = null;
                                db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                        var result = db.TblCimsform.Where(x => x.ChildCode == cimsForm.tblCimsForm.ChildCode).FirstOrDefault();
                        if (result != null)
                        {
                            var formHistory = new TblCimsFormHistory();
                            formHistory.ChildCode = result.ChildCode;
                            formHistory.Description = AttributeConstant.CreateForm;
                            formHistory.CreateBy = result.CreateBy;
                            formHistory.CreateDate = result.CreateDate;
                            db.TblCimsFormHistory.Add(formHistory);                            
                        }
                    }
                    SetStringCache(AttributeConstant.Form_GetListFormCims, GetlistFormCims(cimsForm.tblCimsForm.MenuCode));
                    code = db.SaveChanges();
                    scope.Complete();
                    return code;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
    }
}
