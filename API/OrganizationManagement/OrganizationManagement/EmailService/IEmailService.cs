using OrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationManagement.EmailService
{
    public interface IEmailService
    {
        object SendEmailAsync(TblRecieveEmail objSendEmail);
        //void SendEmailMeeting(TblRecieveEmail objSendEmail);
        //HaiHM add Function: SendEmail
        void SendEmail(TblRecieveEmail objSendEmail);
    }
}
