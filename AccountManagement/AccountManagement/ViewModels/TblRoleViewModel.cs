using AccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class TblRoleViewModel
    {
        public List<RoleDTO> Roles { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public TblRoleViewModel()
        {
            Roles = new List<RoleDTO>();
        }

    }
}
