using System;
using System.Collections.Generic;

namespace AccountManagement.Models
{
    public partial class TblRecieveEmail
    {
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string References { get; set; }
        public string ReferenCode { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public DateTime? SendDate { get; set; }
        public string EmailContents { get; set; }
        public string HtmlBody { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDelete { get; set; }
        public int? UserId { get; set; }
        public bool? IsReadEmail { get; set; }
        public string StepStatus { get; set; }
        public string AttachFile { get; set; }
        public bool? StatusEmail { get; set; }
        public bool? IsConfirm { get; set; }
        public int? OrganizationId { get; set; }
    }
}
