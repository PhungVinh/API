using AccountManagement.Constant;
using AccountManagement.Models;
using MailKit.Net.Pop3;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace AccountManagement.Common
{
    // HaiHM-Modified
    public class CommonFunction 
    {
        //private 
        private static Pop3Client pop3Client;
        private static SmtpClient smtpClient;
        public static string strDomainCV;

        private static CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());

        public CommonFunction() { }

        /// <summary>
        /// Function set smtp for get email
        /// CreatedBy: HaiHM
        /// CreatedDate: xx/04/2019
        /// </summary>
        /// <returns></returns>
        public static Pop3Client InstancePOP3Client()
        {
            #region CuongPV old -- HaiHM Modify 8/4/2019
            #endregion
            if (EmailConf == null)
            {
                EmailConf = db.TblEmailConfigs.FirstOrDefault();
            }
            pop3Client = new Pop3Client();
            try
            {
                pop3Client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                pop3Client.AuthenticationMechanisms.Remove(AccountConstant.Remove);
                if (EmailConf.IsDeleted.HasValue && EmailConf.IsDeleted.HasValue)
                    pop3Client.Connect(EmailConf.ServerGet, Convert.ToInt32(EmailConf.PortGet), SecureSocketOptions.SslOnConnect);
                else
                    pop3Client.Connect(EmailConf.ServerGet, Convert.ToInt32(EmailConf.PortGet), SecureSocketOptions.Auto);
                pop3Client.Authenticate(EmailConf.UserName, EmailConf.Password);
            }
            catch (Exception e)
            {
                throw e;
            }
            return pop3Client;
        }

        /// <summary>
        /// Function set smtp for send email
        /// CreatedBy: HaiHM
        /// CreatedDate: xx/04/2019
        /// </summary>
        /// <returns></returns>
        public static SmtpClient InstanceSendClient()
        {
            if (smtpClient == null)
            {
                TblEmailConfigs config = db.TblEmailConfigs.Where(mail => mail.CreatedBy == "HaiHM").FirstOrDefault();
                smtpClient = new SmtpClient();
                config.ServerGet = config.ServerGet;
                config.PortGet = config.PortGet;
                config.UserName = config.UserName;
                config.Password = config.Password;
                try
                {
                    var credential = new NetworkCredential
                    {
                        UserName = config.UserName,
                        Password = config.Password
                    };

                    smtpClient.Credentials = credential;
                    smtpClient.Host = config.ServerPush;
                    smtpClient.Port = int.Parse(config.PortPush);
                    smtpClient.EnableSsl = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return smtpClient;
        }

        public static TblEmailConfigs EmailConf;

    }
}
