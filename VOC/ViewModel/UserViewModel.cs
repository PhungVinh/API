using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.ViewModel
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsLock { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string CategoryCodeDepartment { get; set; }
        public bool Select { get; set; }
    }
}
