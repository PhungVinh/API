using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class ListAuthorityOfUserVM
    {
        public int AuthorityId { get; set; }
        public string AuthorityName { get; set; }
        public string OrganizationId { get; set; }
        public bool? CheckPack { get; set; }
    }
}
