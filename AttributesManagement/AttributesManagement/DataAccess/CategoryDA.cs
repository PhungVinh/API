using AttributesManagement.Constant;
using AttributesManagement.ContextFactory;
using AttributesManagement.Models;
using AttributesManagement.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Text;

namespace AttributesManagement.DataAccess
{
    public class CategoryDA : IcategoryRepository
    {
        public CategoryDA()
        {

        }
        private CRM_MPContext db = new CRM_MPContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MPContext>());
        private SP_Attributes sp;
        private IDistributedCache _distributedCache;
        public string strconnect { get; set; }
        /// <summary>
        /// Tìm kiếm danh mục
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public object GetAllCategory(string TextSearch, int currPage, int recodperpage)
        {
            try
            {
                List<List<dynamic>> obj = sp.GetAllCategory(TextSearch, currPage, recodperpage);
                var response = new { data = obj[0], paging = obj[1] };
                return response;
                
            }
            catch (Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// Tạo mới danh mục
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="tblCategory">object</param>
        /// <returns></returns>
        public object AddCategory(CategoryChildren tblCategory)
        {
            int code = 0;
            object objCategory = new object();
            try
            {
                string cateCode = LocDau(tblCategory.CategoryName).ToUpper();
                string cateCode1= (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
                TblCategoryGroup addCateGroup = new TblCategoryGroup();
                TblCategory addCategory = new TblCategory();
                TblCategory checkCateCode = db.TblCategory.Where(v => v.CategoryCode == cateCode1 && v.IsDelete == false).FirstOrDefault();
                TblCategoryGroup checkCateGroup = db.TblCategoryGroup.Where(v => v.CategoryCode == cateCode1 && v.IsDelete == false).FirstOrDefault();

                if (tblCategory.CategoryTypeCode != "")
                {
                    if(checkCateCode == null)
                    {
                        if (checkCateGroup == null)
                        {
                            using (var ts = new TransactionScope())
                            {
                                addCateGroup.CategoryCode = cateCode1;
                                addCateGroup.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                addCateGroup.CategoryGroupName = tblCategory.CategoryName;
                                addCateGroup.CategoryDescription = tblCategory.CategoryDescription;
                                addCateGroup.CreateBy = tblCategory.CreateBy;
                                addCateGroup.CreateDate = DateTime.Now;
                                addCateGroup.IsDelete = false;
                                db.TblCategoryGroup.Add(addCateGroup);
                                code = db.SaveChanges();
                                if (tblCategory.children.Count > 0)
                                {
                                    AddCategoryChild(addCateGroup.CategoryCode, tblCategory.children);
                                }
                                objCategory = new { code = code, CategoryCode = addCateGroup.CategoryCode };
                                ts.Complete();
                            }
                        }
                        else
                        {
                            objCategory = new { code = code, CategoryCode = "" };
                        }
                    }
                    else
                    {
                        objCategory = new { code = code, CategoryCode = "" };
                    }
                }
                else
                {
                    if(checkCateGroup == null)
                    {
                        if (checkCateCode == null)
                        {
                            using (var ts = new TransactionScope())
                            {
                                addCategory.CategoryCode = cateCode1;
                                addCategory.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                addCategory.CategoryName = tblCategory.CategoryName;
                                addCategory.CategoryDescription = tblCategory.CategoryDescription;
                                addCategory.IsDelete = false;
                                addCategory.CreateBy = tblCategory.CreateBy;
                                addCategory.CreateDate = DateTime.Now;
                                db.TblCategory.Add(addCategory);
                                code = db.SaveChanges();
                                if (tblCategory.children.Count > 0)
                                {
                                    AddCategoryChild(addCategory.CategoryCode, tblCategory.children);
                                }
                                objCategory = new { code = code, CategoryCode = addCategory.CategoryCode };
                                ts.Complete();
                            }
                        }
                        else
                        {
                            objCategory = new { code = code, CategoryCode = "" };
                        }
                    }
                    else
                    {
                        objCategory = new { code = code, CategoryCode = "" };
                    }
                }
                return objCategory;
            }
            catch(Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// chỉnh sửa danh mục
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="tblCategory">object</param>
        /// <returns></returns>
        public object UpdateCategory(CategoryChildren tblCategory)
        {
            try
            {
                int code = 0;
                object objCategory = new object();
                string cateCode = LocDau(tblCategory.CategoryName).ToUpper();
                string cateCode1= (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
                TblCategoryGroup checkCateGroup = db.TblCategoryGroup.Where(v => v.CategoryCode == tblCategory.CategoryCode && v.IsDelete == false).FirstOrDefault();
                TblCategory checkCateCode = db.TblCategory.Where(v => v.CategoryCode == tblCategory.CategoryCode && v.IsDelete == false).FirstOrDefault();
                if (tblCategory.CategoryTypeCode != "")
                {
                    if (checkCateGroup != null)
                    {
                        TblCategoryGroup checkCategoryCode_New = db.TblCategoryGroup.Where(v => v.CategoryCode == cateCode1 && v.CategoryGroupName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd() && v.IsDelete == false).FirstOrDefault();
                        if(checkCategoryCode_New != null)
                        {
                            using (var ts = new TransactionScope())
                            {
                                checkCateGroup.CategoryCode = cateCode1;
                                checkCateGroup.CategoryGroupName = tblCategory.CategoryName;
                                checkCateGroup.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                checkCateGroup.CategoryDescription = tblCategory.CategoryDescription;
                                checkCateGroup.UpdateBy = tblCategory.UpdateBy;
                                checkCateGroup.UpdateDate = DateTime.Now;
                                checkCateGroup.IsDelete = false;
                                db.Entry(checkCateGroup).State = EntityState.Modified;
                                code = db.SaveChanges();
                                if (tblCategory.children.Count > 0)
                                {
                                    AddCategoryChild(checkCateGroup.CategoryCode, tblCategory.children);
                                }
                                if (tblCategory.deleteCategory.Count > 0)
                                {
                                    DeleteCategoryChild(tblCategory.deleteCategory);
                                }
                                objCategory = new { code = code, CategoryCode = checkCateGroup.CategoryCode };
                                ts.Complete();
                            }
                        }
                        else
                        {
                            TblCategoryGroup checkTblCategoryGroup = db.TblCategoryGroup.Where(v => v.CategoryCode == cateCode1 && v.IsDelete == false).FirstOrDefault();
                            if(checkTblCategoryGroup !=null)
                            {
                                objCategory = new { code = 0, CategoryCode = "" };
                            }
                            else
                            {
                                using (var ts = new TransactionScope())
                                {
                                    checkCateGroup.CategoryCode = cateCode1;
                                    checkCateGroup.CategoryGroupName = tblCategory.CategoryName;
                                    checkCateGroup.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                    checkCateGroup.CategoryDescription = tblCategory.CategoryDescription;
                                    checkCateGroup.UpdateBy = tblCategory.UpdateBy;
                                    checkCateGroup.UpdateDate = DateTime.Now;
                                    checkCateGroup.IsDelete = false;
                                    db.Entry(checkCateGroup).State = EntityState.Modified;
                                    code = db.SaveChanges();
                                    if (tblCategory.children.Count > 0)
                                    {
                                        AddCategoryChild(checkCateGroup.CategoryCode, tblCategory.children);
                                    }
                                    if (tblCategory.deleteCategory.Count > 0)
                                    {
                                        DeleteCategoryChild(tblCategory.deleteCategory);
                                    }
                                    objCategory = new { code = code, CategoryCode = checkCateGroup.CategoryCode };
                                    ts.Complete();
                                }
                            }
                        }
                    }
                    else 
                    {

                        TblCategory checkCateCode1 = db.TblCategory.Where(v => v.CategoryCode == tblCategory.CategoryCode && v.IsDelete == false).FirstOrDefault();
                        using (var ts = new TransactionScope())
                        {
                            TblCategoryGroup addCategory = new TblCategoryGroup();
                            addCategory.CategoryCode = cateCode1;
                            addCategory.CategoryGroupName = tblCategory.CategoryName;
                            addCategory.CategoryTypeCode = tblCategory.CategoryTypeCode;
                            addCategory.CategoryDescription = tblCategory.CategoryDescription;
                            addCategory.CreateBy = tblCategory.CreateBy;
                            addCategory.CreateDate = DateTime.Now;
                            addCategory.IsDelete = false;
                            db.TblCategoryGroup.Add(addCategory);
                            code = db.SaveChanges();
                            if (checkCateCode1 != null)
                            {
                                db.TblCategory.Remove(checkCateCode1);
                                db.SaveChanges();
                            }
                            if (tblCategory.children.Count > 0)
                            {
                                AddCategoryChild(addCategory.CategoryCode, tblCategory.children);
                            }
                            if (tblCategory.deleteCategory.Count > 0)
                            {
                                DeleteCategoryChild(tblCategory.deleteCategory);
                            }
                            objCategory = new { code = code, CategoryCode = addCategory.CategoryCode };
                            ts.Complete();
                        }
                    }
                }
                else
                {
                    if (checkCateCode != null)
                    {
                        TblCategory checkCateCode_New = db.TblCategory.Where(v => v.CategoryCode == tblCategory.CategoryCode && v.CategoryName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd() && v.IsDelete == false).FirstOrDefault();
                        if (checkCateCode_New != null)
                        {
                            using (var ts = new TransactionScope())
                            {
                                checkCateCode.CategoryCode = cateCode1;
                                checkCateCode.CategoryName = tblCategory.CategoryName;
                                checkCateCode.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                checkCateCode.CategoryDescription = tblCategory.CategoryDescription;
                                checkCateCode.CreateBy = tblCategory.CreateBy;
                                checkCateCode.UpdateDate = DateTime.Now;
                                checkCateCode.UpdateBy = tblCategory.UpdateBy;
                                checkCateCode.IsDelete = false;
                                db.Entry(checkCateCode).State = EntityState.Modified;
                                code = db.SaveChanges();
                                if (tblCategory.children.Count > 0)
                                {
                                    AddCategoryChild(checkCateCode.CategoryCode, tblCategory.children);
                                }
                                if (tblCategory.deleteCategory.Count > 0)
                                {
                                    DeleteCategoryChild(tblCategory.deleteCategory);
                                }
                                objCategory = new { code = code, CategoryCode = checkCateCode.CategoryCode };
                                ts.Complete();
                            }
                        }
                        else
                        {
                            TblCategory checkCategory = db.TblCategory.Where(v => v.CategoryCode == cateCode1 && v.IsDelete == false).FirstOrDefault();
                            if (checkCategory != null)
                            {
                                objCategory = new { code = 0, CategoryCode = "" };
                            }
                            else
                            {
                                TblCategoryGroup removeCategoryGroup = db.TblCategoryGroup.Where(v => v.CategoryCode == cateCode1 && v.IsDelete == false).FirstOrDefault();
                                using (var ts = new TransactionScope())
                                {
                                    TblCategory addCate = new TblCategory();
                                    addCate.CategoryCode = cateCode1;
                                    addCate.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                    addCate.CategoryName = tblCategory.CategoryName;
                                    addCate.CategoryDescription = tblCategory.CategoryDescription;
                                    addCate.IsDelete = false;
                                    addCate.CreateBy = tblCategory.UpdateBy;
                                    addCate.CreateDate = DateTime.Now;
                                    db.TblCategory.Add(addCate);
                                    code = db.SaveChanges();
                                    if (removeCategoryGroup != null)
                                    {
                                        db.TblCategoryGroup.Remove(removeCategoryGroup);
                                        db.SaveChanges();
                                    }
                                    if (tblCategory.children.Count > 0)
                                    {
                                        AddCategoryChild(addCate.CategoryCode, tblCategory.children);
                                    }
                                    if (tblCategory.deleteCategory.Count > 0)
                                    {
                                        DeleteCategoryChild(tblCategory.deleteCategory);
                                    }
                                    objCategory = new { code = code, CategoryCode = addCate.CategoryCode };
                                    ts.Complete();
                                }
                            }
                        }
                    }
                }
                return objCategory;
            }
            catch(Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// thêm mới giá trị danh mục
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="categoryParentCode"></param>
        /// <param name="lstCategoryChild"></param>
        public void AddCategoryChild(string categoryParentCode, List<TblCategory> lstCategoryChild)
        {
            try
            {
                object objCategory = new object();
                //var lstCate = db.TblCategory.Where(x =>x.CategoryTypeCode == categoryParentCode && lstCategoryChild.Where(c => c.CategoryCode == x.CategoryCode && c.ExtContent == x.ExtContent).Select(c => c.CategoryCode).Contains(x.CategoryCode)).ToList();
                List<TblCategory> lstUpdatecategory = new List<TblCategory>();
                List<TblCategory> lstAddcategory = new List<TblCategory>();
                foreach (TblCategory item in lstCategoryChild)
                {
                    int num;
                    string cateCode = LocDau(item.CategoryName).ToUpper();
                    string cateCode1 = (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
                    TblCategory checkCateCode = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == item.CategoryCode && v.CategoryTypeCode == categoryParentCode).FirstOrDefault();
                    TblCategory CheckchildCateCode = db.TblCategory.Where(v => v.CategoryTypeCode == categoryParentCode && v.IsDelete == false).LastOrDefault();
                    string cateCode2 = (CheckchildCateCode != null) ? CheckchildCateCode.CategoryCode.Split("-").FirstOrDefault() : "";
                    if (checkCateCode != null)
                    {
                        TblCategory checkCategoryChild= db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == item.CategoryCode && v.CategoryName.ToLower().TrimStart().TrimEnd() == item.CategoryName.ToLower().TrimStart().TrimEnd() && v.CategoryTypeCode == categoryParentCode).FirstOrDefault();
                        if (checkCategoryChild != null)
                        {
                            checkCategoryChild.CategoryCode = item.CategoryCode;
                            checkCategoryChild.CategoryName = item.CategoryName;
                            checkCategoryChild.CategoryTypeCode = categoryParentCode;
                            checkCategoryChild.CategoryDescription = item.CategoryDescription;
                            checkCategoryChild.ExtContent = (item.CategoryTypeCode != "") ? item.CategoryTypeCode : "";
                            checkCategoryChild.IsDelete = false;
                            checkCategoryChild.UpdateBy = item.UpdateBy;
                            checkCategoryChild.UpdateDate = DateTime.Now;
                            db.Entry(checkCateCode).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TblCategory checkCategoryName = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == cateCode1 && v.CategoryTypeCode == categoryParentCode).FirstOrDefault();
                            TblCategory checkCateCode1 = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == item.CategoryCode && v.CategoryTypeCode == categoryParentCode).FirstOrDefault();
                            if (CheckchildCateCode != null)
                            {
                                var index = (CheckchildCateCode.CategoryCode.Contains("-")) ? CheckchildCateCode.CategoryCode.Split("-").LastOrDefault() : "";
                                if(index != "")
                                {
                                    num = int.Parse(index) + 1;
                                }
                                else
                                {
                                    num = 1;
                                }
                            }
                            else
                            {
                                num = 1;
                            }
                            if (checkCategoryName == null)
                            {
                                checkCateCode1.CategoryCode =cateCode1 + "-" + num ;
                                checkCateCode1.CategoryName = item.CategoryName;
                                checkCateCode1.CategoryTypeCode = categoryParentCode;
                                checkCateCode1.CategoryDescription = item.CategoryDescription;
                                checkCateCode1.ExtContent = (item.CategoryTypeCode != "") ? item.CategoryTypeCode : "";
                                checkCateCode1.IsDelete = false;
                                checkCateCode1.UpdateBy = item.UpdateBy;
                                checkCateCode1.UpdateDate = DateTime.Now;
                                db.Entry(checkCateCode1).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        TblCategory addCategory = new TblCategory();
                        if (CheckchildCateCode != null)
                        {
                            var index = (CheckchildCateCode.CategoryCode.Contains("-")) ? CheckchildCateCode.CategoryCode.Split("-").LastOrDefault() : "";
                            if(index != "")
                            {
                                num = int.Parse(index) + 1;
                            }
                            else
                            {
                                num = 1;
                            }
                        }
                        else
                        {
                            num = 1;
                        }
                        addCategory.CategoryCode = cateCode1 + "-" + num;
                        addCategory.CategoryTypeCode = categoryParentCode;
                        addCategory.CategoryName = item.CategoryName;
                        addCategory.CategoryDescription = item.CategoryDescription;
                        addCategory.ExtContent = (item.CategoryTypeCode != "") ? item.CategoryTypeCode : "";
                        addCategory.IsDelete = false;
                        addCategory.CreateBy = item.CreateBy;
                        addCategory.CreateDate = DateTime.Now;
                        db.TblCategory.Add(addCategory);
                        db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// xóa danh mục
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public object DeleteCategory(string categoryCode)
        {
            try
            {
                int code = 0;
                object deleteCateCode = new object();
                if (categoryCode != "")
                {
                    List<TblAttributeConstraint> checkUsing = db.TblAttributeConstraint.Where(v =>v.IsDelete == false && (v.ContraintsType == categoryCode || v.LinkContraints == categoryCode)).ToList();
                    List<TblAttributes> checkDataUsing = db.TblAttributes.Where(v =>v.IsDelete == false && (v.DataType == categoryCode || v.AttributeType == categoryCode || v.CategoryParentCode == categoryCode || v.DefaultValue == categoryCode)).ToList();
                    if (checkDataUsing.Count > 0  )
                    {
                        deleteCateCode = new { code = 0, CategoryCode = "" };
                    }
                    else if(checkUsing.Count > 0)
                    {
                        deleteCateCode = new { code = 0, CategoryCode = "" };
                    }
                    else
                    {
                        TblCategory deleteCategory = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == categoryCode).FirstOrDefault();
                        TblCategoryGroup deleteCategroup = db.TblCategoryGroup.Where(v => v.IsDelete == false && v.CategoryCode == categoryCode).FirstOrDefault();
                        if (deleteCategory != null)
                        {
                            //delete child
                            List<TblCategory> deleteChild = db.TblCategory.Where(v => v.CategoryTypeCode == categoryCode && v.IsDelete == false).ToList();
                            List<TblCategoryGroup> deleteChild1 = db.TblCategoryGroup.Where(v => v.CategoryTypeCode == categoryCode && v.IsDelete == false).ToList();

                            if (deleteChild1.Count > 0)
                            {
                                deleteCateCode = new { code = 0, CategoryCode = "" };
                            }
                            else
                            {
                                if (deleteChild.Count > 0)
                                {
                                    db.TblCategory.RemoveRange(deleteChild);
                                    db.SaveChanges();
                                }
                                db.TblCategory.Remove(deleteCategory);
                                code = db.SaveChanges();
                                deleteCateCode = new { code = code, CategoryCode = deleteCategory.CategoryCode };
                            }

                        }
                        else if (deleteCategroup != null)
                        {
                            List<TblCategoryGroup> deleteCateGroupchild = db.TblCategoryGroup.Where(v => v.CategoryTypeCode == categoryCode && v.IsDelete == false).ToList();
                            List<TblCategory> deleteCategoryChild = db.TblCategory.Where(v => v.CategoryTypeCode == categoryCode && v.IsDelete == false).ToList();
                            if (deleteCateGroupchild.Count > 0)
                            {
                                deleteCateCode = new { code = 0, CategoryCode = "" };
                            }
                            else
                            {
                                if (deleteCategoryChild.Count > 0)
                                {
                                    db.TblCategory.RemoveRange(deleteCategoryChild);
                                    db.SaveChanges();
                                }
                                db.TblCategoryGroup.Remove(deleteCategroup);
                                code = db.SaveChanges();
                                deleteCateCode = new { code = code, CategoryCode = deleteCategroup.CategoryCode };
                            }
                        }
                        else
                        {
                            deleteCateCode = new { code = 2, CategoryCode = "" };
                        }
                    }
                }
                else
                {
                    deleteCateCode = new { code = 2, CategoryCode = "" };
                }
                return deleteCateCode;
            }
            catch (Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// xóa danh mục con
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="lstCategoryCode"></param>
        public void DeleteCategoryChild(List<ObjectDeleteCategoryCode> lstCategoryCode)
        {
            List<TblCategory> lstcate = new List<TblCategory>();
            foreach(ObjectDeleteCategoryCode item in lstCategoryCode)
            {
                TblCategory deleteCatgory = db.TblCategory.Where(v => v.CategoryCode == item.CategoryCode && v.IsDelete == false).FirstOrDefault();
                if(deleteCatgory != null)
                {
                    lstcate.Add(deleteCatgory);
                }
            }
            db.TblCategory.RemoveRange(lstcate);
            db.SaveChanges();
        }
        public object checkCategoryCode(string categoryCode)
        {
            object objCateCode = new object();
            List<TblAttributeConstraint> checkUsing = db.TblAttributeConstraint.Where(v =>( v.ContraintsType == categoryCode || v.LinkContraints == categoryCode) && v.IsDelete == false).ToList();
            List<TblAttributes> checkDataUsing = db.TblAttributes.Where(v => (v.DataType == categoryCode || v.AttributeType == categoryCode || v.CategoryParentCode == categoryCode) && v.IsDelete == false).ToList();
            if(checkDataUsing.Count >0 || checkUsing.Count >0)
            {
                objCateCode = new { code = 0, CategoryCode = "" };
            }
            else
            {
                objCateCode = new { code = 1, CategoryCode = categoryCode };
            }
            return objCateCode;

        }
        /// <summary>
        /// bỏ dấu trong tiếng việt
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Lấy chuỗi đã được bỏ dấu
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="name"></param>
        /// <param name="regex"></param>
        /// <param name="space"></param>
        /// <returns></returns>
        public string Regex1(string name, string regex, string space)
        {
            string s = Regex.Replace(name, regex, space);
            return s;
        }
        
        /// <summary>
        /// Lấy ra danh sách danh mục cha
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <returns></returns>
        public object GetAllParentCategory()
        {
            try
            {
                var objectParentCategory = (from a in db.TblCategory
                                            where (a.IsDelete == false && (a.CategoryTypeCode == null || a.CategoryTypeCode ==""))
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
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// Lấy ra danh sách danh mục theo CategoryCode
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public object GetCategoryByCateCode(string CategoryCode)
        {
            try
            {
                TblCategory category = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == CategoryCode).FirstOrDefault();
                TblCategoryGroup categroup = db.TblCategoryGroup.Where(v => v.IsDelete == false && v.CategoryCode == CategoryCode).FirstOrDefault();
                ListCategory objCategory = new ListCategory();
                if (category != null)
                {
                    objCategory.CategoryCode = category.CategoryCode;
                    objCategory.CategoryTypeCode = category.CategoryTypeCode;
                    objCategory.CategoryName = category.CategoryName;
                    objCategory.CategoryDescription = category.CategoryDescription;
                    objCategory.children = GetAllChildCategory(category.CategoryCode);
                }
                else
                {
                    objCategory.CategoryCode = categroup.CategoryCode;
                    objCategory.CategoryTypeCode = categroup.CategoryTypeCode;
                    objCategory.CategoryName = categroup.CategoryGroupName;
                    objCategory.CategoryDescription = categroup.CategoryDescription;
                    objCategory.children = GetAllChildCategory(categroup.CategoryCode);
                }
                return objCategory;
            }
            catch(Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// Lấy ra danh sách danh mục Con
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="categoryCode"></param>
        /// <returns></returns>
        public object GetAllChildCategory(string categoryCode)
        {
            try
            {
                List<List<dynamic>> obj = sp.GetAllChildCategory(categoryCode);
                var response = obj[0];
                return response;
            }
            catch(Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
            
        }
        /// <summary>
        /// Lấy ra danh sách danh mục 
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public object GetObjectCategory(string CategoryCode)
        {
            try
            {
                var objCategory = from a in db.TblCategory
                                  where (a.IsDelete == false && a.CategoryCode == CategoryCode)
                                  select new
                                  {
                                      a.CategoryCode,
                                      a.CategoryName
                                  };
                return objCategory;
            }
           catch(Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }
        /// <summary>
        /// Read file and Import File 
        /// CreatedBy: Cuongpv1
        /// CreatedDate: 23/05/2019
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// 
        public void LoadContext(string orgCode, IDistributedCache distributedCache)
        {
            if (db == null || strconnect != orgCode)
            {
                string conn = CategoryConstant.SQL_CONNECTION;
                var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                optionsBuilder.UseSqlServer(conn.Replace("MP", orgCode), providerOptions => providerOptions.CommandTimeout(300));
                db = new CRM_MPContext(optionsBuilder.Options);
            }
            sp = new SP_Attributes(db);
            _distributedCache = distributedCache;
        }

        public string GetStringCache(string cacheKey)
        {
            return _distributedCache.GetString(cacheKey);
        }

        public void SetContextFactory(ConnectionStrings connectionStrings)
        {
            DbContextFactory.AddChildContext(connectionStrings);
        }
        public void SetStringCache(string cacheKey, object obj)
        {
             _distributedCache.GetString(cacheKey);
        }
    }
}
