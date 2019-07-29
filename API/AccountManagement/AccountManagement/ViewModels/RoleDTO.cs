using AccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class RoleDTO : TblRole
    {
        public string MenuName { get; set; }
        public RoleDTO()
        {
            Id = 0;
            MenuName = null;
            AuthorityId = null;
            MenuCode = null;
            IsEncypt = false;
            IsShowAll = false;
            IsShow = false;
            IsAdd = false;
            IsEditAll = false;
            IsEdit = false;
            IsDeleteAll = false;
            IsDelete = false;
            IsImport = false;
            IsExport = false;
            IsPrint = false;
            IsApprove = false;
            IsEnable = false;
            IsPermission = false;
            IsFirstExtend = false;
            IsSecondExtend = false;
            IsThirdExtend = false;
            IsFouthExtend = false;
        }
    }
}
