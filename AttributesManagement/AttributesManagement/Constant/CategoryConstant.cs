using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Constant
{
    public static class CategoryConstant
    {
        public const int statusError = 400;
        public const string Code = "code";
        public const string CategoryCode = "CategoryCode";
       
        public const string Space = "";
        public const string regex = "\\s+";
        public const string entityName = "CategoryName";
        public const string Message = "NotNull";
        public const string TypeCategory = "Tblcategory";
        public const string Title = "Method argument not valid";
        public const string MessageError = "error.validation";
        public const string titleDuplicate = "The title already exists in datatable";
        public const string errorKey = "titleexists";
        public const string MessageDulicateCategoryName = "error.titleexists";

        public const string MessageValidate = "Danh mục chưa được sử dụng";

        public const string titleDelete = "CategoryCode is being used";
        public const string errorKeyDelete = "CategoryUsing";
        public const string MessageDelete = "error.CategoryUsing";
        public const string entityNameDelete = "Category";
        public static readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public const string SQL_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MP;user id=sa;password=123@123a";
        public const string MASTER_STORE_PROC = "GetAllConnection";
        public const string CONNECTION_CONFIG = "Connection";
        public const string DATABASE_MASTER = "CRM_MP";
        public const string DATABASE_PREFIX = "CRM_";
    }
}
