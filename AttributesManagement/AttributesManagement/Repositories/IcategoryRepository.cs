using AttributesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Repositories
{
    public interface IcategoryRepository: IBaseRepository
    {
        object GetAllCategory(string TextSearch, int currPage, int recodperpage);
        object AddCategory(CategoryChildren tblCategory);
        object UpdateCategory(CategoryChildren tblCategory);
        object GetObjectCategory(string CategoryCode);
        object DeleteCategory(string categoryCode);
        object GetAllParentCategory();
        object GetCategoryByCateCode(string CategoryCode);
        object checkCategoryCode(string categoryCode);
        void SetStringCache(string cacheKey, Object obj);
        void SetContextFactory(ConnectionStrings connectionStrings);
    }
}
