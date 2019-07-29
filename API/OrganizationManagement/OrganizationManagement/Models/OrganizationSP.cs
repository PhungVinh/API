using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class OrganizationSP
    {
        public Int64 index { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationCode { get; set;}
        public string OrganizationParentCode { get; set;}
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationPhone { get; set; }
        public string OrganizationTaxCode { get; set; }
        public string OrganizationRemark { get; set; }
        public DateTime? OrganizationFrom { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsDelete { get; set; }
        public string chkDelete { get; set; }
        public DateTime? OrganizationTo { get; set; }
        public string OrganizationHomePage { get; set; }
        public string OrganizationLogo { get; set; }
        public string OrganizationNote { get; set; }
        public string OrganizationSphereID { get; set; }
        public string ServiceList { get; set; }
        public string NumberUser { get; set; }
        public string OpName { get; set; }

    }
}
