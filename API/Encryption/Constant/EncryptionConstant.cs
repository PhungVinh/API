using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.Constant
{
    public class EncryptionConstant
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public const string ConnectionConfigReplace = "ConnectionStrings:";
        public const string Authorization = "Authorization";
        public const string StringEmply = "";
        public const string BearerReplace = "Bearer ";
        public const string Connection = "Connection";
        public const string DataType = "DataCode";
        public const string Control = "control";
        public const string OrgCode = "orgCode";
        public const string MP = "MP";
        public const string MASTER = "MASTER";
        public const string IP = "127.0.0.1:6379";
        public const string SQL_MP_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MP;user id=sa;password=123@123a";
        public const string Role = "Role";
        public const string SuperAdmin = "SuperAdmin";
        public const string ParentCode = "CRM";
        public const string ADMIN_USER = "ADMIN_USER";
        public const string ADMIN_USER_NAME = "Quản lý khách hàng";
        public const string CIMS = "CIMS";
        public const string HoTen = "Họ tên";
        public const string DiaChi = "Địa chỉ";
        public const string FullName = "FullName";
        public const string Address = "Address";

        public const string MS0013 = "Mã hóa trường dữ liệu";
        public const string MS0014 = "Hủy mã hóa trường dữ liệu";
        public const string MS0015 = "Đã áp dụng";
        public const string keyString = "*hTniPGgDukF$eow";
        public const string ENCRYPTIONKEY = "A1B2C3D4";

        public const string Key = "Jwt:Key";
        public const string Issuer = "Jwt:Issuer";

        public const string API_GetListAttributesEncryption = "~/api/Encryption/GetListAttributesEncryption";
        public const string API_ExecuteEncrpytion = "~/api/Encryption/ExecuteEncrpytion";
        public const string API_GetListAttributesWithParentCode = "~/api/Encryption/GetListAttributesWithParentCode";
        public const string API_GetListModule = "~/api/Encryption/GetListModule";

        public const string SuccessEncryption = "Mã hóa thành công !";
        public const string NotExecuteEncryption = "Không thực hiện mã hóa !";
        public const string FailureEncryption = "Error occurred !";
        public const string MASTER_STORE_PROC = "GetAllModule";
        public const string MASTER_STORE_PROC_ACCOUNT = "GetAttributeAccount";
        public const string SQL_MASTER_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MASTER;user id=sa;password=123@123a";
        public const string SQL_FSOFT_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_FSOFT;user id=sa;password=123@123a";
    }

}
