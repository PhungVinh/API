using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class CategoryChildren:TblCategory
    {
        public List<TblCategory> children { get; set; }
        public List<ObjectDeleteCategoryCode> deleteCategory { get; set; }
        public CategoryChildren()
        {
            children = new List<TblCategory>();
            deleteCategory = new List<ObjectDeleteCategoryCode>();
        }
        
    }
    public class ObjectDeleteCategoryCode
    {
        public string CategoryCode { get; set; }
    }
}
