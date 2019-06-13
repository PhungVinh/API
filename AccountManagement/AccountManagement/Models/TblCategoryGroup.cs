﻿using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblCategoryGroup
    {
        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public string CategoryGroupName { get; set; }
        public string CategoryDescription { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
    }
}
