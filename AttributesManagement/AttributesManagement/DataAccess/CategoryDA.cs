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

namespace AttributesManagement.DataAccess
{
    public class CategoryDA: IcategoryRepository
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
                TblCategoryGroup addCateGroup = new TblCategoryGroup();
                TblCategory addCategory = new TblCategory();
                TblCategory checkCategoryName = db.TblCategory.Where(v => v.CategoryName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd()).FirstOrDefault();
                TblCategoryGroup checkCategoryGroupName = db.TblCategoryGroup.Where(v => v.CategoryGroupName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd()).FirstOrDefault();
                if (tblCategory.CategoryTypeCode != "")
                {
                    if(checkCategoryGroupName == null)
                    {
                        using (var ts = new TransactionScope())
                        {
                            addCateGroup.CategoryCode = (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
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
                    if(checkCategoryGroupName == null)
                    {
                        if (checkCategoryName == null)
                        {
                            using (var ts = new TransactionScope())
                            {
                                addCategory.CategoryCode = (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
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
                if (tblCategory.CategoryTypeCode != "")
                {
                    TblCategoryGroup checkCateGroup = db.TblCategoryGroup.Where(v => v.CategoryCode == tblCategory.CategoryCode).FirstOrDefault();
                    if (checkCateGroup != null)
                    {
                        if (checkCateGroup.CategoryGroupName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd())
                        {
                            using (var ts = new TransactionScope())
                            {
                                checkCateGroup.CategoryCode = (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
                                checkCateGroup.CategoryGroupName = tblCategory.CategoryName;
                                checkCateGroup.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                checkCateGroup.CategoryDescription = tblCategory.CategoryDescription;
                                checkCateGroup.UpdateBy = tblCategory.UpdateBy;
                                checkCateGroup.UpdateDate = DateTime.Now;
                                db.Entry(checkCateGroup).State = EntityState.Modified;
                                code = db.SaveChanges();
                                if (tblCategory.children != null)
                                {
                                    AddCategoryChild(checkCateGroup.CategoryCode, tblCategory.children);
                                }
                                if (tblCategory.deleteCategory !=null)
                                {
                                    DeleteCategoryChild(tblCategory.deleteCategory);
                                }
                                objCategory = new { code = code, CategoryCode = checkCateGroup.CategoryCode };
                                ts.Complete();
                            }
                        }
                        else
                        {
                            TblCategoryGroup checkGroupName = db.TblCategoryGroup.Where(v => v.CategoryGroupName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd()).FirstOrDefault();
                            if (checkGroupName != null)
                            {
                                objCategory = new { code = 0, CategoryCode = "" };
                            }
                            else
                            {
                                using (var ts = new TransactionScope())
                                {
                                    checkCateGroup.CategoryCode = (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
                                    checkCateGroup.CategoryGroupName = tblCategory.CategoryName;
                                    checkCateGroup.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                    checkCateGroup.CategoryDescription = tblCategory.CategoryDescription;
                                    checkCateGroup.UpdateBy = tblCategory.UpdateBy;
                                    checkCateGroup.UpdateDate = DateTime.Now;
                                    db.Entry(checkCateGroup).State = EntityState.Modified;
                                    code = db.SaveChanges();
                                    if (tblCategory.children != null)
                                    {
                                        AddCategoryChild(checkCateGroup.CategoryCode, tblCategory.children);
                                    }
                                    if (tblCategory.deleteCategory != null)
                                    {
                                        DeleteCategoryChild(tblCategory.deleteCategory);
                                    }
                                    objCategory = new { code = code, CategoryCode = checkCateGroup.CategoryCode };
                                    ts.Complete();
                                }
                                
                            }
                        }
                    }
                }
                else
                {
                    TblCategory checkCategory = db.TblCategory.Where(v => v.CategoryCode == tblCategory.CategoryCode && v.IsDelete == false).FirstOrDefault();
                    if (checkCategory != null)
                    {
                        if (checkCategory.CategoryName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd())
                        {
                            using (var ts = new TransactionScope())
                            {
                                checkCategory.CategoryCode = tblCategory.CategoryCode;
                                checkCategory.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                checkCategory.CategoryName = tblCategory.CategoryName;
                                checkCategory.CategoryDescription = tblCategory.CategoryDescription;
                                checkCategory.IsDelete = false;
                                checkCategory.UpdateBy = tblCategory.UpdateBy;
                                checkCategory.UpdateDate = DateTime.Now;
                                db.Entry(checkCategory).State = EntityState.Modified;
                                code = db.SaveChanges();
                                if (tblCategory.children != null)
                                {
                                    AddCategoryChild(checkCategory.CategoryCode, tblCategory.children);
                                }
                                if (tblCategory.deleteCategory != null)
                                {
                                    DeleteCategoryChild(tblCategory.deleteCategory);
                                }
                                objCategory = new { code = code, CategoryCode = checkCategory.CategoryCode };
                                ts.Complete();
                            }
                        }
                        else
                        {
                            TblCategory checkCategoryName = db.TblCategory.Where(v => v.CategoryName.ToLower().TrimStart().TrimEnd() == tblCategory.CategoryName.ToLower().TrimStart().TrimEnd() && v.IsDelete == false).FirstOrDefault();
                            if (checkCategory == null)
                            {
                                using (var ts = new TransactionScope())
                                {
                                    checkCategory.CategoryCode = tblCategory.CategoryCode;
                                    checkCategory.CategoryTypeCode = tblCategory.CategoryTypeCode;
                                    checkCategory.CategoryName = tblCategory.CategoryName;
                                    checkCategory.CategoryDescription = tblCategory.CategoryDescription;
                                    checkCategory.IsDelete = false;
                                    checkCategory.UpdateBy = tblCategory.UpdateBy;
                                    checkCategory.UpdateDate = DateTime.Now;
                                    db.Entry(checkCategory).State = EntityState.Modified;
                                    code = db.SaveChanges();
                                    if (tblCategory.children != null)
                                    {
                                        AddCategoryChild(checkCategory.CategoryCode, tblCategory.children);
                                    }
                                    if (tblCategory.deleteCategory != null)
                                    {
                                        DeleteCategoryChild(tblCategory.deleteCategory);
                                    }
                                    objCategory = new { code = code, CategoryCode = checkCategory.CategoryCode };
                                    ts.Complete();
                                }
                            }
                            else
                            {
                                objCategory = new { code = 0, CategoryCode = "" };
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
                int num = 0;
                List<TblCategory> lstUpdatecategory = new List<TblCategory>();
                List<TblCategory> lstAddcategory = new List<TblCategory>();
                foreach (TblCategory item in lstCategoryChild)
                {
                    TblCategory addcategoryChild = new TblCategory();
                    TblCategory checkCateCode = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryCode == item.CategoryCode).FirstOrDefault();
                    if(checkCateCode !=null)
                    {
                        if(checkCateCode.CategoryName.ToLower().TrimStart().TrimEnd() == item.CategoryName.ToLower().TrimStart().TrimEnd())
                        {
                            checkCateCode.CategoryCode = item.CategoryCode;
                            checkCateCode.CategoryName = item.CategoryName;
                            checkCateCode.CategoryTypeCode = categoryParentCode;
                            checkCateCode.CategoryDescription = item.CategoryDescription;
                            checkCateCode.ExtContent = (item.CategoryTypeCode != "") ? item.CategoryTypeCode : "";
                            checkCateCode.IsDelete = false;
                            checkCateCode.UpdateBy = item.UpdateBy;
                            checkCateCode.UpdateDate = DateTime.Now;
                            db.Entry(checkCateCode).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            TblCategory checkCategoryName = db.TblCategory.Where(v => v.IsDelete == false && v.CategoryName.ToLower().TrimStart().TrimEnd() == item.CategoryName.ToLower().TrimStart().TrimEnd()).FirstOrDefault();
                            if(checkCategoryName ==null)
                            {
                                checkCateCode.CategoryCode = item.CategoryCode;
                                checkCateCode.CategoryName = item.CategoryName;
                                checkCateCode.CategoryTypeCode = categoryParentCode;
                                checkCateCode.CategoryDescription = item.CategoryDescription;
                                checkCateCode.ExtContent = (item.CategoryTypeCode != "") ? item.CategoryTypeCode : "";
                                checkCateCode.IsDelete = false;
                                checkCateCode.UpdateBy = item.UpdateBy;
                                checkCateCode.UpdateDate = DateTime.Now;
                                db.Entry(checkCateCode).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        TblCategory CheckchildCateCode = new TblCategory();
                        string cateCode = LocDau(item.CategoryName).ToUpper();
                        string cateCode1= (cateCode != "") ? Regex1(cateCode, CategoryConstant.regex, CategoryConstant.Space) : "";
                        CheckchildCateCode = db.TblCategory.Where(v => v.CategoryCode == cateCode1 && v.IsDelete == false).LastOrDefault();
                        if(CheckchildCateCode != null)
                        {
                            
                            num = 1;
                            addcategoryChild.CategoryCode = cateCode1 + num;
                        }
                        else
                        {
                            addcategoryChild.CategoryCode = cateCode1;
                        }
                        addcategoryChild.CategoryTypeCode = categoryParentCode;
                        addcategoryChild.CategoryName = item.CategoryName;
                        addcategoryChild.CategoryDescription = item.CategoryDescription;
                        addcategoryChild.ExtContent = (item.CategoryTypeCode != "") ? item.CategoryTypeCode : "";
                        addcategoryChild.IsDelete = false;
                        addcategoryChild.CreateBy = item.CreateBy;
                        addcategoryChild.CreateDate = DateTime.Now;
                        lstAddcategory.Add(addcategoryChild);
                    }
                }
                db.TblCategory.AddRange(lstAddcategory);
                db.SaveChanges();

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
                    List<TblVocattributes> checkDataUsing = db.TblVocattributes.Where(v =>v.IsDelete == false && (v.DataType == categoryCode || v.AttributeType == categoryCode || v.CategoryParentCode == categoryCode)).ToList();
                    if (checkDataUsing.Count > 0 || checkUsing.Count > 0)
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
            List<TblVocattributes> checkDataUsing = db.TblVocattributes.Where(v => (v.DataType == categoryCode || v.AttributeType == categoryCode || v.CategoryParentCode == categoryCode) && v.IsDelete == false).ToList();
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
                var objcategory = from a in db.TblCategory
                                  where a.IsDelete == false && a.CategoryTypeCode == categoryCode
                                  select new
                                  {
                                      a.CategoryCode,
                                      a.CategoryTypeCode,
                                      a.CategoryName,
                                      a.ExtContent
                                  };
                return objcategory;
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
