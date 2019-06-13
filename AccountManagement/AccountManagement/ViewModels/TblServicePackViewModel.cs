﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagement.ViewModels
{
    public class TblServicePackViewModel
    {
        public Int64 index { get; set; }
        public int Id { get; set; }
        public string CodeServicePack { get; set; }
        public string NameServicePack { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public int? MaxUser { get; set; }
        public double? Price { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public int? Discount { get; set; }
        public bool? Hot { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
