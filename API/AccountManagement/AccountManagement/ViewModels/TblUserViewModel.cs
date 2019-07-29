﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class TblUserViewModel
    {
        public Int64 index { get; set; }
        public int UserId { get; set; }
        public int ID { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public string UserName { get; set; }
        //public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate {get; set;}
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsLock { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Position { get; set; }
        //public bool? EmailConfirmed { get; set; }
        public int? Gender { get; set; }
        //public int? LoginFail { get; set; }
        //public string HistoryPassword { get; set; }
        //public DateTime? DateUpdatePassword { get; set; }
        public DateTime? BirthDay { get; set; }
        //public string CodeReset { get; set; }
        //public DateTime? ExpirationDate { get; set; }
        public string CategoryCodeDepartment { get; set; }
        //public string CategoryCodeRole { get; set; }
    }
}