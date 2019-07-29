using System;

namespace AccountManagement.EmailService
{
    public class EmailConfig
    {
        public string From;
        public string To;
        public string Bcc;
        public string Cc;
        public DateTime? SendDate;
        public string EmailContents;
        public string AttachFile;
        public int UserId;
        public int CreateBy;
        public bool StatusEmail;
    }
}
