namespace AccountManagement.Constant
{
    public static class AccountConstant
    {
        public const int NumberRoles = 18;
        public const string Role = "Role";
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Member = "Member";
            
        public const string IpRedisCache = "127.0.0.1:6379";

        public const string Issuer = "Issuer";
        public const string DefaultOrganizationCode = "MP";
        public const string strJWT = "Jwt";
        public const string FileAppsettingsJson = "appsettings.json";
        public const string strKeyAES = "AES256:Key";

        public const string URI = "http://192.168.50.175:50000";

        public const string DateFormat = "dd/MM/yyyy HH:mm:ss";

        #region UpdateInforOrganization
        public const int UpdateOrgDuplicateEmail = 1;
        public const int UpdateOrgDuplicateTaxCode = 2;
        public const int UpdateOrgFail = 0;
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
        public const string MessageError = "Validate null";
        #endregion
        #region Token
        public const string EncodedJwt = "[encoded jwt";
        public const string MPConnection = "MPConnection";
        public const string Authorization = "Authorization";
        public const string BearerReplace = "Bearer ";
        public const string ListLogoutToken = "ListLogoutToken ";
        public const string MessageDeleteSuccess = "Xóa thành công";
        #endregion

        #region Compare

        public const int Min0 = 0;//
        public const int AddUserSuccess = 4;// Success 
        public const int AddUserDuplicateUsername = 1;
        public const int AddUserDuplicateEmail = 2;
        public const int AddUserDuplicatePhoneNumber = 3;

        public const int EditUserSuccess = 3;// Success
        public const int EditUserDuplicateEmail = 1;
        public const int EditUserDuplicatePhoneNumber = 2;

        public const int ResetPassSuccess = 1;// Success
        public const int ResetPassFail = 0;// Success

        #endregion

        #region Header

        public const string XForwardedFor = "X-Forwarded-For";
        public const string UserAgent = "User-Agent";

        #endregion

        #region Log
        public const string sub = "sub";

        public const string LoginSuccess = "Login Success";
        public const string ActionNameLogin = "Login";
        public const string ActionTypeLogin = "Login";
        public const string ModuleLogin = "Login";
        public const string TypeLoginS = "Login";

        #endregion

        #region Change Password
        public const int ChangePasswordDuplicateOldPass = 3;
        public const int InputPasswordError = 4;
        public const int NotSame = 1;
        public const int ChangePasswordSuccess = 2;
        #endregion

        #region DeleteUser
        public const int DeleteUserSuccess = 1;
        public const int DeleteUserFail = 0;
        #endregion

        public const string MENU_PARENT_CODE = "CRM";
        public const string MENU_ADMIN_DEPART = "ADMIN_DEPART";
        public const string MENU_ATTR_RELATION = "ATTR_RELATION";
        public const string MENU_PROFILE = "PROFILE";
        public const string MENU_PROFILE_USER = "PROFILE_USER";
        public const string MENU_PROFILE_CHANGEPASSWORD = "PROFILE_CHANGEPASSWORD";
        public const string MENU_PROFILE_ORGANIZATION = "PROFILE_ORGANIZATION";
        public const string MENU_CIMS_LIST = "CIMS_LIST";
        public const string MENU_ADMIN_USER = "ADMIN_USER";


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
        public const string ApiLinkReset = "http://192.168.50.37:90/auth/reset-password-finish";
        public const string ApiLinkCreated = "http://192.168.50.37:90";
        public const string SubjectSendMailReset = "Reset mật khẩu!";
        public const string SubjectSendMailCreated = "Thông tin tài khoản CRM!";
        
        #endregion

        #region Cache

        public const string GetUserList = "api/Account/GetUserList";

        public const string GetTblMenuParent = "api/Account/GetTblMenuParent";

        #endregion Error

        #region 
        public const int statusSuccess = 200;
        public const int statusError = 400;
        public const string TypeUser = "TblUsers";

        public const string TitleLoginOrgLockOrDelete = "Đơn vị tạm thời bị khóa hoặc không có trong hệ thống";
        public const string ErrorLoginLockOrgLockOrDelete = "errir.orglockordelete";
        public const string MessageFailLoginOrgLockOrDelete = "Đơn vị tạm thời bị khóa hoặc không có trong hệ thống";
        

        public const string TitleAddUserNull = "error.tblusers.nullusernameoremailorphonenumber";
        public const string MessageAddUserNull = "AddUser UserName or Email or PhoneNumber can not null";
        public const string TitleAddUserNullRoleId = "error.tblusers.addusertblrolenullid";
        public const string MessageAddUserNullRoleId = "AddUser tblRole can not null id";
        public const string TitleAddUserNullOrganizationId = "error.tblUsers.addusertblorganizationnullid";
        public const string MessageAddUserNullOrganizationId = "AddUser tblOrganization can not null id";

        public const string TitleAddSuccess = "user.add.success";
        public const string MessageAddSuccess = "Lưu thông tin thành công";

        public const string ErrorKeyDuplicateUsername = "error.tblusers.duplicateusername";
        public const string MessageAddDuplicateUsernames = "Username đã tồn tại";

        public const string ErrorKeyDuplicateEmail = "error.tblusers.duplicateemail";
        public const string TitleDuplicateEmail = "error.tblusers.duplicateemail";
        public const string MessageAddDuplicateEmail = "Email đã tồn tại";

        public const string ErrorKeyDuplicatePhoneNumber = "error.tblusers.duplicatephonenumber";
        public const string TitleDuplicatePhoneNumber = "error.tblusers.duplicateemail";
        public const string MessageAddDuplicatePhoneNumber = "PhoneNumber đã tồn tại";

        public const string MessageGetUserByIdSuccess = "GetUserById Success";
        public const string MessageGetUserByIdFail = "GetUserById Fail";

        public const string TitleNullUserOrId = "error.tblusers.nulluserorid";
        public const string MessageNullUserOrId = "Fail NullUserOrId";

        public const string TitleEditUserTblRoleNullId = "error.tblusers.editusertblrolenullid";
        public const string MessageEditUserTblRoleNullId = "EditUser tblRole can not null id";

        public const string TitleEditUserTblOrganizationNullId = "error.tblusers.editusertblorganizationnullid";
        public const string MessageEditUserTblOrganizationNullId = "EditUser tblOrganization can not null id";

        public const string TitleEditSuccess = "edituser.success";
        public const string MessageEditSuccess = "Cập nhật thông tin thành công";

        public const string MessageChangePasswordNullUserOrId = "ChangePassword Fail NullUserOrId";

        public const string TitleNullNewPassword = "error.tblusers.nullnewpassword";
        public const string MessageNullNewPasswordFail = "New Password can not null";

        public const string MessageNewPasswordSuccess = "Thay đổi mật khẩu thành công";

        public const string ErrorKeyChangePassFail = "error.tblusers.changepassfail";
        public const string TitleChangePassFail = "Mật khẩu không được trùng với 03 mật khẩu thay đổi gần nhất";
        public const string MessageChangePassFail = "Mật khẩu không được trùng với 03 mật khẩu thay đổi gần nhất";

        public const string ErrorKeyChangePassNotSame = "error.tblusers.notsame";
        public const string TitleChangePassNotSame = "Mật khẩu mới không giống nhau";
        public const string MessageChangePassNotSame = "Mật khẩu mới không giống nhau";

        public const string ErrorKeyChangeInputPassFail = "error.tblusers.inputpassFail";
        public const string TitleChangePassInputPassFail = "Mật khẩu không hợp lệ";
        public const string MessageChangePassInputPassFail = "Mật khẩu không hợp lệ";

        public const string MessageLockUserSuccess = "LockUser Success";

        public const string ErrorKeyLockUserFail = "error.tblusers.lockuserfail";
        public const string TitleLockUserFail = "LockUser Fail NullUserOrId";
        public const string MessageLockUserFail = "LockUser Fail";

        public const string MessageActiveUserSuccess = "ActiveUser Success";

        public const string ErrorKeyActiveUserFail = "error.tblusers.activeuserfail";
        public const string TitleActiveUserFail = "Active Fail NullUserOrId";
        public const string MessageActiveUserFail = "Active Fail";

        public const string MessageDeleteUserSuccess = "Xóa thành công";

        public const string ErrorKeyDeleteUserFail = "error.tblusers.deleteuserfail";
        public const string TitleDeleteUserFail = "Delete Fail NullUserOrId";
        public const string MessageDeleteUserFail = "Delete Fail";

        public const string MessageUpdateUserFail = "UpdateUser Fail";
        public const string MessageUpdateUserSuccess = "Cập nhật thông tin thành công";

        public const string TypeNullToken = "error.nulltoken";
        public const string TitleNullToken = "error.nulltoken";
        public const string MessageNullToken = "nulltoken";
        public const string MessageTokenResetOk = "Token Valid";

        public const string TypeInValidToken = "error.tokeninvalid";
        public const string MessageTokenResetFail = "Token Invalid";

        public const string TypeResetPass = "error.usernotfound";
        public const string ErrorKeyResetPass = "error.usernotfound";
        public const string TitleResetPass = "error.usernotfound";
        public const string MessageResetPassFail = " error.usernotfound";
        public const string MessageResetPassOk = "Reset mật khẩu thành công";

        public const string TypeLogin = "error.nullusernameorpassword";
        public const string ErrorLoginNullParameter = "error.nullusernameorpassword";
        public const string ErrorLoginNotFound = "error.loginerror";
        public const string TitleLoginNotFound = "Thông tin đăng nhập không chính xác";

        public const string TitleLoginNullParameter = "nullusernameorpassword";
        public const string MessageLoginNullParameterFail = "Null username or password";
        public const string MessageLoginNotFound = "Thông tin đăng nhập không chính xác";

         public const string ErrorLoginDelete = "error.userhasdelete";
        public const string TitleLoginDelete = "error.userhasdelete";
        public const string MessageLoginDelete = " user has delete";

        public const string ErrorLoginLock = "error.userhaslock";
        public const string TitleLoginLock = "error.userhaslock";
        public const string MessageLoginLock = "Tài khoản đã bị khóa";

        public const string ErrorDateTimeExpr = "error.mustchangepassword";
        public const string TitleDateTimeExpr = "error.mustchangepassword";
        public const string MessageDateTimeExpr = "User must change password";


        public const string MessageLoginOK = "Login ok";

        public const string MessageNullUser = "Null user";
        public const string ErrorGetUserNull = "error.usernamenull";
        public const string MessageGetUserNull = " usernamenull";

        public const string ErrorKeyMaxUser = "error.maxuser";
        public const string TitleMaxUser = "Số lượng tài khoản vượt quá số lượng đăng ký";
        public const string MessageMaxUser = "Số lượng tài khoản vượt quá số lượng đăng ký";

        public const string TypeLoginAdmin = "login.organization";
        public const string ErrorLoginAdmin = "error.login.organization";
        public const string TitleLoginAdminErr = "Đơn vị quá thời hạn kích hoạt";
        public const string MessageLoginAdminErr = "Đơn vị quá thời hạn kích hoạt";

        public const string MessageUserNameMaxLength = "Vượt quá độ dài trường Username";

        #endregion

        #region Password Policy
        public const string PasswordPolicyCountPasswordHistory = "PasswordPolicy:CountPasswordHistory";
        public const string PasswordPolicySplit = "=";
        public const string PasswordPolicyPasDuration = "PasswordPolicy:PasDuration";
        public const string PasswordPolicyExpDateCreated = "PasswordPolicy:ExpDateCreated";

        #endregion

        #region GenerateJSONWebToken
        public const string JwtKey = "Jwt:Key";
        public const string userId = "userId";
        public const string orgCode = "orgCode";
        public const string JwtIssuer = "Jwt:Issuer";
        public const string Expr = "Expr";
        public const string ExpLogin = "ExpLogin";
        public const int AddDaysLinkReset = 30;
        public const int AddMinuteExpLogin = 480;
        public const string CountLoginFail = "PasswordPolicy:CountLoginFail";

        #endregion
        public const string StringNullGetListUser = ",,,1,15";
        public const string StringSlipSearch = ",";
        public const string GachDuoi = "_";
        public const string StringPageCurent = "1";
        public const string StringRecodperpage = "15";

        #region UploadFile
        public const string DirectoryUploads = "\\Uploads\\";
        public const string DirectorySlat = "\\";
        public const int MaximumFile = 5245329;
        public const string TitleMaximumFile = "error.maximumzise";
        public const string MaximumFileOver = "Dung lượng file tối đa 5 MB";

        public const string UpLoadFail = "Dung lượng file tối đa 5 MB";
        public const string TitleTokenInvalid = "error.tokeninvalid";

        #endregion
        public static string SQL_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MASTER;user id=sa;password=123@123a";

        #region Role
        public const string CanEncypt = "CanEncypt";
        public const string CanShowAll = "CanShowAll";
        public const string CanShow = "CanShow";
        public const string CanAdd = "CanAdd";
        public const string CanEditAll = "CanEditAll";
        public const string CanEdit = "CanEdit";
        public const string CanDeleteAll = "CanDeleteAll";
        public const string CanDelete = "CanDelete";
        public const string CanImport = "CanImport";
        public const string CanExport = "CanExport";
        public const string CanPrint = "CanPrint";
        public const string CanApprove = "CanApprove";
        public const string CanEnable = "CanEnable";
        public const string CanPermission = "CanPermission";
        public const string CanFirstExtend = "CanFirstExtend";
        public const string CanSecondExtend = "CanSecondExtend";
        public const string CanThirdExtend = "CanThirdExtend";
        public const string CanFouthExtend = "CanFouthExtend";
        #endregion

        #region Role Policy
        public const string PolicyCanAdd = "PolicyCanAdd";
        public const string PolicyEdit = "PolicyEdit";
        public const string PolicyDelete = "PolicyDelete";
        public const string PolicyShow = "PolicyShow";
        #endregion

        #region MenuCode
        public const string ADMIN_USER = "ADMIN_USER";
        public const string MENU_CATEGORY = "CATEGORY";
        #endregion

        #region Field Null
        public const string FieldPass1 = "Pass1";
        public const string FieldPass2 = "Pass2";
        public const string AddTypeNullEmail = "Email";
        public const string TypeNullUserName = "UserName";
        public const string TypeNullPhoneNumber = "PhoneNumber";
        public const string TypeNullId = "Id";
        public const string TypeNullBirthDay = "BirthDay";

        public const string MsgAddTypeNullEmail = "Not null";
        public const string MsgTypeNullUserName = "Not null";
        public const string MsgTypeNullPhoneNumber = "Not null";
        public const string MsgTypeNullId = "Not null";
        public const string MsgTypeNullBirthDay = "Not valid birthday";

        #endregion

        public const string LinkDefaulfLogoGirl = "Default/AvatarGirl.jpg";
        public const string LinkDefaulfLogoBoy = "Default/AvatarBoy.png";

    }
}
