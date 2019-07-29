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
                    TblAttributes addAttributes = new TblAttributes();
                    TblAttributes checkCode = db.TblAttributes.Where(v => v.IsDelete == false).LastOrDefault();
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
                    addAttributes.IsDelete = false;
                    addAttributes.Encyption = false;
                    addAttributes.EncyptWaiting = false;
                    addAttributes.IsSort = attributes.IsSort;
                    addAttributes.ModuleParent = AttributeConstant.CIMS;
                    addAttributes.AttributeDescription = attributes.AttributeDescription;
                    //thêm chức năng
                    addAttributes.MaximumLength = attributes.MaximumLength;
                    addAttributes.MinimumLength = attributes.MinimumLength;
                    addAttributes.IsEnable = attributes.IsEnable;
                    addAttributes.IsLinkFieldInformation = attributes.IsLinkFieldInformation;
                    addAttributes.MaxValue = attributes.MaxValue;
                    addAttributes.MinValue = attributes.MinValue;
                    addAttributes.RuleInputValue = attributes.RuleInputValue;
                    if (attributes.IsLinkFieldInformation.Value == true)
                    {
                        addAttributes.InputFieldValue = (attributes.InputFieldValue != "") ? attributes.InputFieldValue : "";
                    }
                    if (attributes.IsReuse == AttributeConstant.IsDependentValue)
                    {
                        AddDependentValue(attributes.DependentValues, addAttributes.AttributeCode, addAttributes.ModuleParent);
                    }
                    if (attributes.IsReuse == AttributeConstant.IsGeneratingValue)
                    {
                        AddGeneratingValue(attributes.GeneratingValues, addAttributes.AttributeCode, addAttributes.ModuleParent, addAttributes.IsReuse);
                    }
                    if (attributes.DetailRefer.Count() > 0)
                    {
                        AddReferenceConstraint(attributes.DetailRefer, addAttributes.AttributeCode, addAttributes.ModuleParent);
                    }
                    db.TblAttributes.Add(addAttributes);
                    code = db.SaveChanges();
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
                    TblAttributes checkData = db.TblAttributes.Where(v => v.IsDelete == false && v.AttributeCode == attributes.AttributeCode).FirstOrDefault();
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
                            checkData.IsDelete = false;
                            checkData.ModuleParent = attributes.ModuleParent;
                            checkData.AttributeDescription = attributes.AttributeDescription;
                            //Thêm chức năng
                            checkData.MaximumLength = attributes.MaximumLength;
                            checkData.MinimumLength = attributes.MinimumLength;
                            checkData.IsEnable = attributes.IsEnable;
                            checkData.IsLinkFieldInformation = attributes.IsLinkFieldInformation;
                            checkData.MaxValue = attributes.MaxValue;
                            checkData.MinValue = attributes.MinValue;
                            checkData.RuleInputValue = attributes.RuleInputValue;
                            if (attributes.IsLinkFieldInformation.Value == true)
                            {
                                checkData.InputFieldValue = (attributes.InputFieldValue != "") ? attributes.InputFieldValue : "";
                            }
                            if (attributes.IsReuse == AttributeConstant.IsDependentValue)
                            {
                                AddDependentValue(attributes.DependentValues, checkData.AttributeCode, checkData.ModuleParent);
                            }
                            if (attributes.IsReuse == AttributeConstant.IsGeneratingValue)
                            {
                                AddGeneratingValue(attributes.GeneratingValues, checkData.AttributeCode, checkData.ModuleParent, checkData.IsReuse);
                            }
                            if (attributes.DetailRefer.Count() > 0)
                            {
                                AddReferenceConstraint(attributes.DetailRefer, checkData.AttributeCode, checkData.ModuleParent);
                            }
                            db.Entry(checkData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            code = db.SaveChanges();
                            SetStringCache(AttributeConstant.Attributes_GetListAttributes, GetListAttributes(attributes.ModuleParent));
                            objAttributes = new { code = code, Id = checkData.AttributesId };
                        }
                        else
                        {
                            TblAttributes checkAttributeName = db.TblAttributes.Where(v => v.AttributeLabel.ToLower() == attributes.AttributeLabel.ToLower() && v.IsDelete == false).FirstOrDefault();
                            if (checkAttributeName != null)
                            {
                                objAttributes = new { code = code, Id = 0 };
                            }
                            else
                            {
                                checkData.AttributeDescription = attributes.AttributeDescription;
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
                                checkData.IsDelete = false;
                                checkData.ModuleParent = attributes.ModuleParent;
                                checkData.AttributeDescription = attributes.AttributeDescription;
                                //Thêm chức năng
                                checkData.MaximumLength = attributes.MaximumLength;
                                checkData.MinimumLength = attributes.MinimumLength;
                                checkData.IsEnable = attributes.IsEnable;
                                checkData.IsLinkFieldInformation = attributes.IsLinkFieldInformation;
                                checkData.MaxValue = attributes.MaxValue;
                                checkData.MinValue = attributes.MinValue;
                                checkData.RuleInputValue = attributes.RuleInputValue;
                                if (attributes.IsLinkFieldInformation.Value == true)
                                {
                                    checkData.InputFieldValue = (attributes.InputFieldValue != "") ? attributes.InputFieldValue : "";
                                }
                                if (attributes.IsReuse == AttributeConstant.IsDependentValue)
                                {
                                    AddDependentValue(attributes.DependentValues, checkData.AttributeCode, checkData.ModuleParent);
                                }
                                if (attributes.IsReuse == AttributeConstant.IsGeneratingValue)
                                {
                                    AddGeneratingValue(attributes.GeneratingValues, checkData.AttributeCode, checkData.ModuleParent, checkData.IsReuse);
                                }
                                if (attributes.DetailRefer.Count() > 0)
                                {
                                    AddReferenceConstraint(attributes.DetailRefer, checkData.AttributeCode, checkData.ModuleParent);
                                }
                                db.Entry(checkData).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                code = db.SaveChanges();
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
                    TblAttributes tblAttributes = db.TblAttributes.Where(a => a.AttributeCode == lstAttribute[i].AttributeCode).FirstOrDefault();
                    if (tblAttributes != null)
                    {
                        List<AttributeOption> lstOption = new List<AttributeOption>();
                        AttributeObject attributeObject = new AttributeObject();
                        attributeObject.AttributeId = tblAttributes.AttributesId;
                        attributeObject.AttributeCode = tblAttributes.AttributeCode;
                        attributeObject.AttributeLabel = tblAttributes.AttributeLabel;
                        attributeObject.AttributeType = tblAttributes.AttributeType;
                        attributeObject.MaximumLength = tblAttributes.MaximumLength;
                        attributeObject.MinimumLength = tblAttributes.MinimumLength;
                        attributeObject.AttributeCol = lstAttribute[i].AttributeColumn.Value;
                        attributeObject.DataType = tblAttributes.DataType;
                        attributeObject.DefaultValue = tblAttributes.DefaultValue;
                        attributeObject.IsCategory = tblAttributes.IsCategory.HasValue ? tblAttributes.IsCategory.Value : false;
                        attributeObject.IsDuplicate = tblAttributes.IsDuplicate.HasValue ? tblAttributes.IsDuplicate.Value : false;
                        attributeObject.IsRequired = tblAttributes.IsRequired.HasValue ? tblAttributes.IsRequired.Value : false;
                        attributeObject.IsTableShow = tblAttributes.IsTableShow.HasValue ? tblAttributes.IsTableShow.Value : false;
                        if (tblAttributes.IsCategory.HasValue)
                        {
                            List<TblCategory> tblCategories = db.TblCategory.Where(a => a.CategoryTypeCode == tblAttributes.CategoryParentCode).ToList();
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
                TblAttributes deleteAttributes = db.TblAttributes.Where(c => c.AttributesId == attributesId && c.IsDelete == false).FirstOrDefault();
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
            List<TblAttributes> lst = db.TblAttributes.Where(v => v.ModuleParent == moduleParent && v.IsDelete == false).ToList();
            foreach (var item in lst)
            {
                InfoAttribute addAddtribute = new InfoAttribute();
                addAddtribute.DetailRefer = getArrayString(item.AttributeCode);
                addAddtribute.AttributeCode = item.AttributeCode;
                addAddtribute.ChildCode = item.ModuleParent;
                addAddtribute.AttributeLabel = item.AttributeLabel;
                addAddtribute.AttributesId = item.AttributesId;
                addAddtribute.AttributeType = item.AttributeType;
                addAddtribute.MaximumLength = item.MaximumLength;
                addAddtribute.MinimumLength = item.MinimumLength;
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
                addAddtribute.IsEnable = item.IsEnable;
                addAddtribute.IsLinkFieldInformation = item.IsLinkFieldInformation;
                addAddtribute.InputFieldValue = item.InputFieldValue;
                addAddtribute.MaxValue = item.MaxValue;
                addAddtribute.MinValue = item.MinValue;
                addAddtribute.RuleInputValue = item.RuleInputValue;
                addAddtribute.GeneratingValues = GetDetailGeneratingValue(item.AttributeCode, item.ModuleParent);
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
            List<TblAttributes> lst = db.TblAttributes.Where(v => v.AttributesId == Id && v.IsDelete == false).ToList();
            foreach (var item in lst)
            {
                InfoAttribute addAddtribute = new InfoAttribute();
                addAddtribute.DetailRefer = getArrayString(item.AttributeCode);
                addAddtribute.AttributeCode = item.AttributeCode;
                addAddtribute.AttributeLabel = item.AttributeLabel;
                addAddtribute.AttributesId = item.AttributesId;
                addAddtribute.AttributeType = item.AttributeType;
                addAddtribute.MaximumLength = item.MaximumLength;
                addAddtribute.MinimumLength = item.MinimumLength;
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
                addAddtribute.IsLinkFieldInformation = item.IsLinkFieldInformation;
                addAddtribute.InputFieldValue = item.InputFieldValue;
                addAddtribute.MaxValue = item.MaxValue;
                addAddtribute.MinValue = item.MinValue;
                addAddtribute.IsEnable = item.IsEnable;
                addAddtribute.RuleInputValue = item.RuleInputValue;
                addAddtribute.GeneratingValues = GetDetailGeneratingValue(item.AttributeCode, item.ModuleParent);
                //addAddtribute.DependentValues = GetDetailDependentValue(item.AttributeCode, item.ModuleParent);
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
        /// <summary>
        /// Get detail value Generating
        /// </summary>
        /// <param name="attributeCode"></param>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public TblGeneratingValues GetDetailGeneratingValue(string attributeCode, string menuCode)
        {
            try
            {
                var lst = db.TblGeneratingValues.Where(v => v.AttributeCode == attributeCode && v.MenuCode == menuCode).FirstOrDefault();
                return lst;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// Get detail value Dependent
        /// </summary>
        /// <param name="attributeCode"></param>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public DependentValue GetDetailDependentValue(string attributeCode, string menuCode)
        {
            try
            {
                var lstDependent = db.TblDependentValues.Where(v => v.AttributeCode == attributeCode && v.ModuleCode == menuCode).FirstOrDefault();
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        /// Add new generating value
        /// </summary>
        /// <param name="generatingValue"></param>
        /// <param name="attributeCode"></param>
        /// <param name="menuCode"></param>
        /// <param name="isReuse"></param>
        public void AddGeneratingValue(TblGeneratingValues generatingValue, string attributeCode,string menuCode,string isReuse)
        {
            try
            {
                TblGeneratingValues checkData = db.TblGeneratingValues.Where(v => v.AttributeCode == attributeCode && v.MenuCode == menuCode).FirstOrDefault();
                if(checkData !=null)
                {
                    db.TblGeneratingValues.Remove(checkData);
                    db.SaveChanges();
                }
                if (generatingValue != null)
                {
                    TblGeneratingValues addGeneratingValue = new TblGeneratingValues();
                    addGeneratingValue.MenuCode = menuCode;
                    addGeneratingValue.AttributeCode = attributeCode;
                    addGeneratingValue.InputFormat = generatingValue.InputFormat;
                    addGeneratingValue.IsReuse = isReuse;
                    addGeneratingValue.MinimumLenght = generatingValue.MinimumLenght;
                    addGeneratingValue.MaxLenght = generatingValue.MaxLenght;
                    addGeneratingValue.ExclusionCharacters = generatingValue.ExclusionCharacters;
                    addGeneratingValue.RequiredCharacters = generatingValue.RequiredCharacters;
                    addGeneratingValue.IsCapitalizeLetter = generatingValue.IsCapitalizeLetter;
                    addGeneratingValue.IsLowercaseLetter = generatingValue.IsLowercaseLetter;
                    addGeneratingValue.IsSpecialCharacters = generatingValue.IsSpecialCharacters;
                    db.TblGeneratingValues.Add(addGeneratingValue);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddDependentValue(DependentValue dependentValues, string attributeCode, string menuCode)
        {
            try
            {
                List<TblDependentValues> lstDependentValue = db.TblDependentValues.Where(v => v.AttributeCode == attributeCode && v.ModuleCode == menuCode).ToList();
                if(lstDependentValue.Count() >0)
                {
                    db.TblDependentValues.RemoveRange(lstDependentValue);
                    db.SaveChanges();
                }
                if (dependentValues.InputValue != null)
                {
                    foreach (string item in dependentValues.InputValue)
                    {
                        TblDependentValues addDependentValue = new TblDependentValues();
                        addDependentValue.AttributeCode = attributeCode;
                        addDependentValue.ModuleCode = menuCode;
                        addDependentValue.CalculatingType = dependentValues.CalculatingType;
                        addDependentValue.CalculatingDetail = (dependentValues.CalculatingType == AttributeConstant.Calculation) ? dependentValues.CalculatingDetail : "";
                        addDependentValue.AttributeInfomation = dependentValues.AttributeInfomation;
                        addDependentValue.AttributeCondition = dependentValues.AttributeCondition;
                        addDependentValue.ConditionCode = dependentValues.ConditionCode;
                        addDependentValue.ConditionValue = item;
                        addDependentValue.FunctionCode = dependentValues.FunctionCode;
                        db.TblDependentValues.Add(addDependentValue);
                        db.SaveChanges();
                    }
                }
                else
                {
                    TblDependentValues addDependent = new TblDependentValues();
                    addDependent.AttributeCode = attributeCode;
                    addDependent.CalculatingType = dependentValues.CalculatingType;
                    addDependent.ModuleCode = menuCode;
                    addDependent.CalculatingDetail = (dependentValues.CalculatingType == AttributeConstant.Calculation) ? dependentValues.CalculatingDetail : "";
                    addDependent.AttributeInfomation = (dependentValues.CalculatingType == AttributeConstant.FunctionCalculation) ? dependentValues.AttributeInfomation : "";
                    addDependent.AttributeCondition = (dependentValues.CalculatingType == AttributeConstant.FunctionCalculation) ? dependentValues.AttributeCondition : "";
                    addDependent.ConditionCode = (dependentValues.CalculatingType == AttributeConstant.FunctionCalculation) ? dependentValues.ConditionCode : "";
                    addDependent.ConditionValue = (dependentValues.CalculatingType == AttributeConstant.FunctionCalculation) ? dependentValues.ConditionValue : "";
                    addDependent.FunctionCode = (dependentValues.CalculatingType == AttributeConstant.FunctionCalculation) ? dependentValues.FunctionCode : "";
                    db.TblDependentValues.Add(addDependent);
                    db.SaveChanges(); 
                }
            }
            catch(Exception ex)
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
            try
            {
                var lstReferneceConstraint = db.TblReferenceConstraint.Where(v => v.AttributeCode == attributeCode && v.MenuCode == MenuCode).ToList();
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
            }
            catch (Exception ex)
            {
                throw ex;
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
                        var attCreateBy = db.TblAttributes.Where(x => x.AttributeCode == AttributeConstant.Attributes_CreateBy).FirstOrDefault();
                        if (attCreateBy != null)
                        {
                            attCreateBy.DefaultValue = cimsForm.tblCimsForm.CreateBy;
                            attCreateBy.DefaultValueWithTextBox = cimsForm.tblCimsForm.CreateBy;
                            db.Entry(attCreateBy).State = EntityState.Modified;
                        }                        
                        var attCreateDate = db.TblAttributes.Where(x => x.AttributeCode == AttributeConstant.Attributes_CreateDate).FirstOrDefault();
                        if (attCreateDate != null)
                        {
                            attCreateDate.DefaultValue = String.Format("{0:r}", cimsForm.tblCimsForm.CreateDate);
                            attCreateDate.DefaultValueWithTextBox = String.Format("{0:r}", cimsForm.tblCimsForm.CreateDate);
                            db.Entry(attCreateDate).State = EntityState.Modified;
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
                    TblCimsform editForm = db.TblCimsform.Where(v => v.ChildCode == updateForm.tblCimsForm.ChildCode && v.IsDelete != true).FirstOrDefault();
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
        public List<TblAttributes> GetAllAttributeRequired(string menucode)
        {
            List<TblAttributes> lstAttribute = db.TblAttributes.Where(v => v.IsDelete == false && v.ModuleParent == menucode && v.IsRequired == true).ToList();
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

        #endregion
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
                var objectParentCategory = (from a in db.TblCategory
                                            where (a.IsDelete == false && (a.CategoryTypeCode == null || a.CategoryTypeCode == ""))
                                            select new
                                            {
                                                CategoryName = a.CategoryName,
                                                CategoryCode = a.CategoryCode,
                                                CategoryTypeCode = a.CategoryTypeCode
                                            }).Concat(from b in db.TblCategoryGroup
                                                      where (b.IsDelete == false)
                                                      select new
                                                      {
                                                          CategoryName = b.CategoryGroupName,
                                                          CategoryCode = b.CategoryCode,
                                                          CategoryTypeCode = b.CategoryTypeCode
                                                      });

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
                        TblAttributes tblAttributes = db.TblAttributes.Where(a => a.AttributeCode == lstAttribute[i].AttributeCode && a.IsDelete == false).FirstOrDefault();
                        if (tblAttributes != null)
                        {
                            List<AttributeOption> lstOption = new List<AttributeOption>();
                            AttributesObjectDTO attributeObject = new AttributesObjectDTO();
                            attributeObject.AttributeId = tblAttributes.AttributesId;
                            attributeObject.AttributeCode = tblAttributes.AttributeCode;
                            attributeObject.AttributeLabel = tblAttributes.AttributeLabel;
                            attributeObject.AttributeType = lstAttribute[i].AttributeType;
                            attributeObject.AttributeDescription = tblAttributes.AttributeDescription;
                            attributeObject.MaximumLength = tblAttributes.MaximumLength;
                            attributeObject.MinimumLength = tblAttributes.MinimumLength;
                            attributeObject.IsReuse = tblAttributes.IsReuse;
                            attributeObject.IsVisible = tblAttributes.IsVisible;
                            attributeObject.IsDuplicate = tblAttributes.IsDuplicate;
                            attributeObject.DetailRefer = getArrayString(tblAttributes.AttributeCode);
                            attributeObject.AttributeCol = lstAttribute[i].AttributeColumn.Value;
                            attributeObject.AttrOrder = lstAttribute[i].AttrOrder.Value;
                            attributeObject.IsShowLabel = lstAttribute[i].IsShowLabel;
                            attributeObject.DefaultValue = lstAttribute[i].DefaultValue;
                            attributeObject.IsDuplicate = tblAttributes.IsDuplicate;
                            attributeObject.RowIndex = lstAttribute[i].RowIndex.Value;
                            attributeObject.AttributeColumn = lstAttribute[i].AttributeColumn.Value;
                            attributeObject.DataType = tblAttributes.DataType;
                            attributeObject.CategoryParentCode = tblAttributes.CategoryParentCode;
                            attributeObject.DefaultValueWithTextBox = tblAttributes.DefaultValueWithTextBox;
                            attributeObject.IsCategory = tblAttributes.IsCategory.HasValue ? tblAttributes.IsCategory.Value : false;
                            attributeObject.IsRequired = tblAttributes.IsRequired.HasValue ? tblAttributes.IsRequired.Value : false;
                            attributeObject.IsTableShow = tblAttributes.IsTableShow.HasValue ? tblAttributes.IsTableShow.Value : false;
                            if (tblAttributes.IsCategory.HasValue)
                            {
                                List<TblCategory> tblCategories = db.TblCategory.Where(a => a.CategoryTypeCode == tblAttributes.CategoryParentCode).ToList();
                                for (int j = 0; j < tblCategories.Count; j++)
                                {
                                    AttributeOption attributeOption = new AttributeOption();
                                    attributeOption.OptionValue = tblCategories[j].CategoryCode;
                                    attributeOption.OptionText = tblCategories[j].CategoryName;
                                    lstOption.Add(attributeOption);
                                }
                            }
                            if (tblAttributes.CategoryParentCode != null)
                            {
                                var result = db.TblCategory.Where(x => x.CategoryTypeCode == tblAttributes.CategoryParentCode).ToList();
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
                    TblCimsform editForm = db.TblCimsform.Where(v => v.ChildCode == updateForm.tblCimsForm.ChildCode && v.IsDelete != true).FirstOrDefault();
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
                                var result1 = db.TblAttributes.Where(x => x.AttributeCode == item.AttributeCode && x.ModuleParent == item.ModuleParent && x.IsDelete == false).FirstOrDefault();
                                if (result1 != null)
                                {
                                    result1.IsTableShow = item.IsTableShow;
                                    result1.IndexTitleTable = item.IndexTitleTable;
                                    result1.IsSort = item.IsSort;
                                    db.Entry(result1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                }
                            }
                            var lst = db.TblAttributes.Where(x => !cimsForm.Table.Where(c => c.AttributeCode == x.AttributeCode && c.ModuleParent == x.ModuleParent && x.IsDelete == false).Select(c => c.AttributeCode).Contains(x.AttributeCode)).ToList();
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
                            var lst = db.TblAttributes.ToList();
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

        public object UpdateTableFormList(List<InfoAttribute> table)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (table.Count() > 0)
                    {
                        foreach (var item in table)
                        {
                            var result1 = db.TblAttributes.Where(x => x.AttributeCode == item.AttributeCode && x.ModuleParent == item.ModuleParent && x.IsDelete == false).FirstOrDefault();
                            if (result1 != null)
                            {
                                result1.IsTableShow = item.IsTableShow;
                                result1.IndexTitleTable = item.IndexTitleTable;
                                result1.IsSort = item.IsSort;
                                db.Entry(result1).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                        }
                        var lst = db.TblAttributes.Where(x => !table.Where(c => c.AttributeCode == x.AttributeCode && c.ModuleParent == x.ModuleParent && x.IsDelete == false).Select(c => c.AttributeCode).Contains(x.AttributeCode)).ToList();
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
                        var lst = db.TblAttributes.ToList();
                        foreach (var item in lst)
                        {
                            item.IsTableShow = false;
                            item.IndexTitleTable = null;
                            item.IsSort = null;
                            db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    scope.Complete();
                    return AttributesMessages.MS0003;
                }
            }
            catch (Exception ex)
            {
                return new { Message = ex.Message };
            }
        }
        #endregion
    }
}
