﻿using System;
using System.Collections.Generic;

namespace Encryption.Models
{
    public partial class TblCategory
    {
        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string ExtContent { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public int? IsActive { get; set; }
        public int? OrderNum { get; set; }
    }
}
