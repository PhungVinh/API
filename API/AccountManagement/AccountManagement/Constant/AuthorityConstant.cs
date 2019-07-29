namespace AccountManagement.Constant
{
    public static class AuthorityConstant
    {
        #region Authority
        public const string orgCode = "orgCode";
        public const int statusSuccess = 200;
        public const int statusError = 400;
        public const string TypeAuthority = "TblAuthority";

        public const string TitleAddAuthority = "null.authority";
        public const string MessageAddAuthority = "null.authority";

        public const string TitleAddAuthorityNullOrg = "null.OrganizationId";
        public const string MessageAddAuthorityNullOrg = "null.OrganizationId";

        public const string ErrorKeyAddError = "error.duplicatename";
        public const string MessageAddError = "Tên đơn vị đã tồn tại";

        public const string TitleDeleteAuthority = "error.deleteerror";
        public const string MessageDeleteAuthority = "error.deleteerror";
        public const string ErrorKeyDeleteError = "error.deleteerror";
        public const string MessageDeleteError = "Delete Fail";

        public const string TitleNullAuthorityId = "error.nullidauthority";
        public const string MessageNullAuthority = "error.null id authority";
        public const string ErrorKeyNullError = "error.nullidauthority";

        public const string DeleteSuccess = "Xóa thành công";

        public const string ExceptionGetUsersToGrantAuthority = "Error: Không có nhóm quyền này";
        #endregion

        public const string CheckDupicateOK = "Not Dupicate";
        public const string Error = "Error occurred";
    }
}
