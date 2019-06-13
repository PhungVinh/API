using AccountManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class UserAndOrgViewModel
    {
        public TblUsers tblUsers;
        public TblOrganization tblOrganization;
        public List<TblAuthority> tblAuthority;
    }
}
