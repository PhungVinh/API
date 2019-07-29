using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Constant
{
    public static class AttributeConstant
    {
        public const string ConnectionConfig = "ConnectionStrings";
        public const string ConnectionConfigReplace = "ConnectionStrings:";
        public const string Authorization = "Authorization";
        public const string StringEmply = "";
        public const string BearerReplace = "Bearer ";
        public const string ConnectionAdd = "Connection";
        public const string DataType = "DataCode";
        public const string Control = "control";
        public const string Object = "Object";
        public const string UserName = "sub";
        public const string IsDuplicate = "Check Trùng";
        public const string IsRequired = "Bắt Buộc";
        public const string IsTableShow = "Hiển thị ";
        public const string IsTableShow1 = "Hiển thị danh sách";
        public const string IsInputValue = "rdInputvalue";
        public const string IsGeneratingValue = "rdGeneratingValue";
        public const string IsDependentValue = "rdDependentValue";
        public const string Calculation = "rdCalculation";
        public const string FunctionCalculation = "rdFunctionCalculation";
        public const string StringNullGetListConstraint = ",1,15";
        public const int PageCurent = 1;
        public const string StringTextSearch = "";
        public const int Recodperpage = 15;
        public const string Attributes_GetListAttributes = "api/Attributes/GetListAttributes";
        public const string Attributes_GetListVOCForm = "api/Attributes/GetListVOCForm";
        public const string Form_GetListFormCims = "api/Attributes/GetListFormCims";
        public const string Attributes_GetListDatatype = "api/Attributes/GetListDataType";
        public const string Attributes_GetListController = "api/Attributes/GetListController";
        public const string Attributes_GetListConstraint = "api/Attributes/GetListConstraintByCateCode";
        public const string Attributes_GetListConstraint1 = "api/Attributes/GetAllConstraints1";
        public const string Attributes_GetAllAttributeCims = "api/Attributes/GetAllAttributeCims";
        public const string Attributes_GetAllParentCategory = "api/Attributes/GetAllParentCategory";
        public const string Attributes_GetAllChildCategory = "api/Attributes/GetAllChildCategory";
        public const string Attributes_Validate_Duplicate = "Trùng mã attribute";
        public const string Attributes_Validate_Code = "Thiếu mã attribute";
        public const string Attributes_Validate_Lable = "Thiếu tên attribute";
        public const string Attributes_Validate_Type = "Thiếu kiểu attribute";
        public const string Attributes = "ATTRIBUTE";
        public const string Attributes_GetFormWithChildCode = "GetAllAttributesCimsWithRowDetails";
        public const string CreateForm = "Tạo mới form";
        public const string UpdateForm = "Sửa form";
        public const string Attributes_CreateBy = "ATTRIBUTE1";
        public const string Attributes_CreateDate = "ATTRIBUTE2";

        public const int statusSuccess = 200;
        public const int statusError = 400;
        public const string Code = "code";
        public const string Id = "Id";

        public const string TypeAttribute = "TblVOCAttributes";
        public const string TypeAttributeConstraint = "TblAttributeConstraint";
        public const string Message = "NotNull";


        public const string titleDuplicate = "The title already exists in datatable";
        public const string errorKey = "titleexists";
        public const string MessageDulicateAttributename = "error.titleexists";
        public const string entityName = "AttributesName";
        public const string entityType = "AttributesType";
        public const string entityDataType = "DataType";
        public const string entityNameConsatraint = "AttributesConstraintsName";
        public const string entityNameConstraint = "Name";
        public const string entityTypeConsatraint = "ContraintsType";

        public const string Title = "Method argument not valid";
        public const string MessageError = "error.validation";

        public const string titleDelete = "Attribute is being used";
        public const string errorKeyDelete = "AttributeUsing";
        public const string MessageDelete = "error.AttributeUsing";
        public const string entityNameDelete = "Attributes";

        public const string titleConstraintDelete = "AttributeConstraint is being used";
        public const string errorKeyConstraintDelete = "AttributeConstraintUsing";
        public const string MessageConstraintDelete = "error.AttributeConstraintUsing";
        public const string entityNameConstraintDelete = "AttributesConstraint";
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
        #region Module
        public const string InformationForm = "CIMS_ADD";
        public const string ListForm = "CIMS_LIST";
        public const string CIMS = "CIMS";
        #endregion
        #region ContextFactory
        public const string SQL_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MASTER;user id=sa;password=123@123a";
        public const string MP_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MP;user id=sa;password=123@123a";
        public const string MASTER_STORE_PROC = "GetAllConnection";
        public const string CONNECTION_CONFIG = "Connection";
        public const string DATABASE_MASTER = "CRM_MASTER";
        public const string DATABASE_PREFIX = "CRM_";
        public const string DATABASE_MP = "MP";
        #endregion
    }
}
