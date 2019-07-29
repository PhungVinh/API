using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblLogUser
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string OrganizationCode { get; set; }
        public string Module { get; set; }
        public string ActionType { get; set; }
        public string ActionName { get; set; }
        public string ActionResult { get; set; }
        public DateTime? ActionDate { get; set; }
        public string Username { get; set; }
        public string LoginOperatingSystemName { get; set; }
        public string LoginBrowserName { get; set; }
        public string LoginIpaddress { get; set; }
        public DateTime? LoginTimeStart { get; set; }
    }
}
