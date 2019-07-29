using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class AllRoleDTO
    {
        public List<TblRoleViewModel> RoleModule { get; set; }
        public List<RoleDTO> AllRoles { get; set; }
        public AllRoleDTO()
        {
            RoleModule = new List<TblRoleViewModel>();
            AllRoles = new List<RoleDTO>();
        }
    }
}
