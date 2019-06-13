using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class UserGrantDTO
    {
        public List<UserDTO> Users { get; set; }
        public int AuthorityId { get; set; }
    }
}
