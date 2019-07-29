using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Constant
{
    public class VOCConstant
    {

        public const string BearerReplace = "Bearer ";
        public const string orgCode = "orgCode ";
        public const string Authorization = "Authorization";
        #region ContextFactory
        public const string SQL_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MASTER;user id=sa;password=123@123a";
        public const string MP_CONNECTION = "Server=192.168.50.149;initial catalog=CRM_MP;user id=sa;password=123@123a";
        public const string MASTER_STORE_PROC = "GetAllConnection";
        public const string CONNECTION_CONFIG = "Connection";
        public const string DATABASE_MASTER = "CRM_MASTER";
        public const string DATABASE_PREFIX = "CRM_";
        public const string DATABASE_MP = "MP";
        public const string ConnectionKey = "Connection";
        #endregion

        public const string sp_SearchVOCProcess = "sp_SearchVOCProcess";
        public const string userId = "userId";
        public const string sp_voc_GetUserAssignee = "sp_voc_GetUserAssignee";
        public const string sp_GetAllCategoryByTypeCode = "sp_GetAllCategoryByTypeCode";
        public const string Department = "Department";
        public const string DepartmentNULL = "Phòng ban khác";
        public const string ListLogoutToken = "ListLogoutToken ";

        #region Role Policy
        public const string PolicyCanAdd = "PolicyCanAdd";
        public const string PolicyEdit = "PolicyEdit";
        public const string PolicyDelete = "PolicyDelete";
        public const string PolicyShow = "PolicyShow";
        #endregion

        #region Role
        public const string CanEncypt = "CanEncyptVOC";
        public const string CanShowAll = "CanShowAllVOC";
        public const string CanShow = "CanShowVOC";
        public const string CanAdd = "CanAddVOC";
        public const string CanEditAll = "CanEditAllVOC";
        public const string CanEdit = "CanEditVOC";
        public const string CanDeleteAll = "CanDeleteAllVOC";
        public const string CanDelete = "CanDeleteVOC";
        public const string CanImport = "CanImportVOC";
        public const string CanExport = "CanExportVOC";
        public const string CanPrint = "CanPrintVOC";
        public const string CanApprove = "CanApproveVOC";
        public const string CanEnable = "CanEnableVOC";
        public const string CanPermission = "CanPermissionVOC";
        public const string CanFirstExtend = "CanFirstExtendVOC";
        public const string CanSecondExtend = "CanSecondExtendVOC";
        public const string CanThirdExtend = "CanThirdExtendVOC";
        public const string CanFouthExtend = "CanFouthExtendVOC";
        #endregion

        #region 

        #endregion

    }
}
