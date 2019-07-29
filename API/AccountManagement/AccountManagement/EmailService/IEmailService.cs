using AccountManagement.Models;

namespace AccountManagement.EmailService
{
    public interface IEmailService
    {
        object SendEmailAsync(TblRecieveEmail objSendEmail);
        //void SendEmailMeeting(TblRecieveEmail objSendEmail);
    }
}
