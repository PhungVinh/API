using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class UserDTO : TblUserViewModel
    {
        public bool IsGrantedAuthority { get; set; }
    }
}
