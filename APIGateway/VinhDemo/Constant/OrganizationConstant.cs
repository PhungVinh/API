using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationManagement.Constant
{
    public static class OrganizationConstant
    {
        public const string ORGANIZATION_GET_ORGANIZATION_LIST = "api/Organization/GetOrganizationList";

        public const string StringSlipSearch = ",";
        public const string strJWT = "Jwt";

        public const string strKeyAES = "AES256:Key";

        public const string URI = "http://192.168.50.123:50000";

        public const string DateFormat = "dd/MM/yyyy HH:mm:ss";
        public const string MENU_PARENT_CODE = "CRM";


        
        public const string entityEmail = "Email";
        public const string entityTaxCode = "TaxCode";
        public const string entityOrgCode = "OrgCode";
        public const string Message = "NotNull";

        public const string Title = "Method argument not valid";
        public const string MessageError = "error.validation";
        public const int statusError = 400;

        public const string AddOK = "Thêm thành công!";
        public const string EditOK = "Sửa thành công!";
        public const string DuplicateOrgCode = "Mã Đơn vị đã tồn tại!";
        public const string DuplicateEmail = "Email vị đã tồn tại!";
        public const string DuplicateOrgTaxCode = "Mã số thuế vị đã tồn tại!";
        public const string NoDelete = "Không được phép xóa!";
        
        #region Email
        public const string Remove = "XOAUTH2";
        public const string Zero = "0";
        public const string One = "1";
        public const string Two = "2";
        public const string Three = "3";
        public const string Fullname = "- Họ và tên:";
        public const string EmailCandiate = "- Email:";
        public const string PhoneCandidate = "- Số điện thoại:";
        public const int Number = 1;
        public const string Postion = "vị trí ";
        public const string Datetime = "ddMMyyyy";
        public const string EmailConfig = "nhansu@mpsoftware.com.vn";
        public const string From = "From:";
        public const string PathURL = "wwwroot/";
        public const string PathURLFolder = "wwwroot/Uploads/";
        #endregion

        #region UploadFile
        public const string DirectoryUploads = "\\Uploads\\";
        public const string DirectorySlat = "\\";
        public const string DirectorySlatSave = "/";
        public const int MaximumFile = 5245329;
        public const string TitleMaximumFile = "error.maximumzise";
        public const string MaximumFileOver = "Error Maximum File Upload Size";

        public const string UpLoadFail = "Error Maximum File Upload Size";
        public const string TitleTokenInvalid = "error.tokeninvalid";
        public const int statusFail = 400;
        public static string TypeOrganization = "tblOrganization";

        #endregion

        #region Create new schema
        public const string DATABASE_PREFIX = "CRM_";
        public const string DATABASE_CHECK_CREATE = "IF (EXISTS (SELECT name FROM master.sys.databases WHERE name = ";
        public static string SQL_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MASTER;user id=sa;password=123@123a";
        public const string DATABASE_MASTER = "CRM_MASTER";
        public const string DATABASE_DROP = "DROP DATABASE ";
        public const string DATABASE_CREATE = "CREATE DATABASE ";
        public const string SPLIT_DATABASE = "~";
        public const string DATABASE_SCRIPT_FILE = "CRM_MP_07062019.sql";
        public const string DATABASE_GO = "GO";
        public const string DATABASE_USE = "USE ";
        public const string CONNECTION_CONFIG = "Connection";
        public const string BEARER_REPLACE = "Bearer ";
        public const string AUTHORIZATION = "Authorization";
        public const string BODY_TYPE = "application/json";
        public const string API_CONTEXT = "api/Attributes/SetContextFactory";
        #endregion

        #region HaiHM using for add authority
        public const string AuthorityType = "Admin";
        public const string AuthorityName = "Admin";
        public const string AuthorityDescription = "Admin";
        #endregion

        #region MenuCode
        public const string ADMIN_DEPART = "ADMIN_DEPART";
        public const string ATTR_RELATION = "ATTR_RELATION";
        #endregion

        #region DateExpri
        public const int ExpirationDate = 30;
        #endregion

        #region orgCode - HaiHM add
        public const string orgCode = "orgCode";
        public const string userId = "userId";

        public const int UpdateOrgFail = 0;
        public const int UpdateOrgDuplicateEmail = 1;
        public const int UpdateOrgDuplicateTaxCode = 2;
        public const int UpdateOrgSuccess = 3;

        public const string TypeOrg = "TblOrganization";
        public const string ErrorKeyOrgDuplicateTaxCode = "error.organization.duplicatetaxcode";
        public const string ErrorKeyOrgDuplicateEmail = "error.organization.duplicateemail";
        public const string MessageOrgDuplicateEmail = "DuplicateEmail";
        public const string MessageOrgDuplicateTaxCode = "DuplicateTaxCode";

        public const string ErrorKeyOrgServerErr = "error.organization.servererr";
        public const string MessageOrgOrgServerErr = "Server error";
        public const string MessageUpdateOrgSuccess = "Cập nhật thông tin thành công";

        public const string TypeNullTaxCode = "OrganizationTaxCode";
        public const string MessageTypeNullTaxCode = "Not null";
        public const string TypeNullEmail = "OrganizationEmail";
        public const string MessageTypeNullEmail = "Not null";
        public const string MessageNullError = "Validate null";


        public const string LinkDefaulfLogo = "Default/LogoDefault.jpeg";
        public const string LinkDefaulfLogo1 = "Default\\LogoDefault.jpeg";
        #endregion
    }
}
