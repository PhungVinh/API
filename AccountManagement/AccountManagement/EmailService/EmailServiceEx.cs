using AccountManagement.Common;
using AccountManagement.Constant;
using AccountManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccountManagement.EmailService
{
    public class EmailServiceEx : IEmailService
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        //public EmailServiceEx(CRM_MASTERContext _context)
        //{
        //    db = _context;
        //}

        /// <summary>
        /// HaiHM
        /// Send email, create user, edit user
        /// </summary>
        /// <returns></returns>
        public void SendEmail(TblRecieveEmail objSendEmail)
        {
            try
            {
                SmtpClient client = CommonFunction.InstanceSendClient();
                MailMessage newMail = new MailMessage();
                newMail.To.Add(new MailAddress(objSendEmail.To));
                // new MailAddress(emailConfig.UserName);
                newMail.From = new MailAddress("super88mp@gmail.com");
                newMail.Subject = objSendEmail.Subject;
                newMail.Body = objSendEmail.EmailContents;
                newMail.IsBodyHtml = true;
                newMail.SubjectEncoding = Encoding.UTF8;
                newMail.HeadersEncoding = Encoding.UTF8;

                new Thread(() => { client.Send(newMail); }).Start();
                //using (MailMessage newMail = new MailMessage())
                //{
                //    newMail.To.Add(new MailAddress(objSendEmail.To));
                //    // new MailAddress(emailConfig.UserName);
                //    newMail.From = new MailAddress("super88mp@gmail.com");
                //    newMail.Subject = objSendEmail.Subject;
                //    newMail.Body = objSendEmail.EmailContents;
                //    newMail.IsBodyHtml = true;
                //    newMail.SubjectEncoding = Encoding.UTF8;
                //    newMail.HeadersEncoding = Encoding.UTF8;
                //    await client.SendMailAsync(newMail);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public object SendEmailAsync(TblRecieveEmail objSendEmail)
        {
            var response = new { Code = 0 };
            string errorEmail = "";
            TblEmailConfigs emailConfig = db.TblEmailConfigs.FirstOrDefault<TblEmailConfigs>();

            #region add list to send email
            List<TblRecieveEmail> lstRecieveEmail = new List<TblRecieveEmail>();
            lstRecieveEmail.Add(new TblRecieveEmail()
            {
                From = emailConfig.UserName,
                To = objSendEmail.To,
                Bcc = objSendEmail.Bcc,
                Cc = objSendEmail.Cc,
                Subject = objSendEmail.Subject,
                SendDate = objSendEmail.SendDate,
                EmailContents = objSendEmail.EmailContents,
                IsDelete = objSendEmail.IsDelete,
                CreateBy = objSendEmail.CreateBy,
                UserId = objSendEmail.UserId,
                OrganizationId = objSendEmail.OrganizationId,
                StepStatus = objSendEmail.StepStatus,
                StatusEmail = objSendEmail.StatusEmail,
                IsConfirm = objSendEmail.IsConfirm,
                AttachFile = objSendEmail.AttachFile,
                CreateDate = DateTime.Now,

            });
            #endregion
            try
            {
                SmtpClient client = CommonFunction.InstanceSendClient();
                string[] emailCC = objSendEmail.Cc != null ? objSendEmail.Cc.Split(';') : null;

                using (var emailMessage = new MailMessage())
                {

                    emailMessage.To.Add(new MailAddress(objSendEmail.To));
                    if (emailCC != null)
                    {
                        foreach (var item in emailCC)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                emailMessage.CC.Add(new MailAddress(item));
                            }
                        }
                    }

                    emailMessage.From = new MailAddress(emailConfig.UserName);
                    emailMessage.Subject = objSendEmail.Subject;
                    //Models.Common.CommonFunction.strDomainCV + 
                    string imgUrl = AccountManagement.Common.CommonFunction.strDomainCV + emailConfig.Signature;
                    string img = "<p></br><img src='" + imgUrl + "'></img></p>";
                    emailMessage.Body = objSendEmail.EmailContents + img;
                    emailMessage.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(objSendEmail.AttachFile))
                    {
                        string[] attached = objSendEmail.AttachFile.Split(';');
                        if (attached != null)
                        {
                            for (int i = 0; i < attached.Count(); i++)
                            {
                                string filepath = attached[i];
                                FileStream fs = new FileStream(AccountConstant.PathURL + filepath, FileMode.Open, FileAccess.Read);
                                StreamReader s = new StreamReader(fs);
                                s.Close();
                                fs = new FileStream(AccountConstant.PathURL + filepath, FileMode.Open, FileAccess.Read);
                                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(MediaTypeNames.Text.Plain);
                                Attachment attachment = new Attachment(fs, ct);
                                attachment.ContentDisposition.Inline = true;
                                System.Net.Mime.ContentDisposition disposition = attachment.ContentDisposition;
                                disposition.CreationDate = File.GetCreationTime(AccountConstant.PathURL + filepath);
                                disposition.ModificationDate = File.GetLastWriteTime(AccountConstant.PathURL + filepath);
                                disposition.ReadDate = File.GetLastAccessTime(AccountConstant.PathURL + filepath);
                                disposition.FileName = Path.GetFileName(AccountConstant.PathURL + filepath);
                                disposition.Size = new FileInfo(AccountConstant.PathURL + filepath).Length;
                                disposition.DispositionType = DispositionTypeNames.Attachment;
                                emailMessage.Attachments.Add(attachment);
                            }
                        }
                    }
                    try
                    {
                        client.Send(emailMessage);

                    }
                    catch (SmtpFailedRecipientException ex)
                    {
                        SmtpStatusCode statusCode = ex.StatusCode;
                        errorEmail = ex.StatusCode.ToString();
                        if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                        {
                            // wait 5 seconds, try a second time
                            Thread.Sleep(5000);
                            client.Send(emailMessage);
                        }
                        else
                        {

                            throw;

                        }
                    }
                    finally
                    {
                        if (lstRecieveEmail.Count > 0)
                        {
                            db.TblRecieveEmail.AddRange(lstRecieveEmail);
                            db.SaveChanges();
                            if (errorEmail == SmtpStatusCode.MailboxBusy.ToString() || errorEmail == SmtpStatusCode.MailboxUnavailable.ToString() || errorEmail == SmtpStatusCode.TransactionFailed.ToString())
                            {
                                response = new { Code = 2 };
                            }
                            else
                            {
                                response = new { Code = 1 };
                            }

                        }
                        emailMessage.Dispose();
                    }


                }
                #region lưu các email đã gửi đi
                #endregion
                return response;
            }
            catch (Exception ex)
            {
                return response;
                throw ex;
            }
        }

       /* public void SendEmailMeeting(TblRecieveEmail objSendEmail)
        {
            TblEmailConfigs emailConfig = db.TblEmailConfigs.FirstOrDefault<TblEmailConfigs>();
            double minute = double.Parse(objSendEmail.ReferenCode);
            DateTime dateSent = DateTime.Parse(objSendEmail.SendDate.Value.ToString("yyyy/MM/dd") + " " + objSendEmail.References).AddHours(-7);
            DateTime dateEnd = dateSent.AddMinutes(minute);
            try
            {

                using (var client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = emailConfig.UserName,
                        Password = emailConfig.Password
                    };

                    client.Credentials = credential;
                    client.Host = emailConfig.ServerPush;
                    client.Port = int.Parse(emailConfig.PortPush);
                    client.EnableSsl = true;
                    string[] emailTo = objSendEmail.To.Split(';');
                    using (var msg = new MailMessage())
                    {
                        if (emailTo != null)
                        {
                            foreach (var item in emailTo)
                            {
                                msg.To.Add(new MailAddress(item));
                            }
                        }

                        string html = System.Web.HttpUtility.HtmlDecode(objSendEmail.EmailContents);
                        html = html.Replace("<p>", "").Replace("</strong>", "").Replace("</p>", "").Replace("<strong>", "");
                        msg.From = new MailAddress(emailConfig.UserName);
                        msg.Subject = objSendEmail.Subject;
                        msg.Body = html;

                        //msg.IsBodyHtml = true;
                        StringBuilder str = new StringBuilder();
                        str.AppendLine("BEGIN:VCALENDAR");
                        str.AppendLine("PRODID:-//Schedule a Meeting");
                        str.AppendLine("VERSION:2.0");
                        str.AppendLine("METHOD:REQUEST");
                        str.AppendLine("BEGIN:VEVENT");
                        str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", dateSent));
                        str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                        str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", dateEnd));
                        str.AppendLine("LOCATION:'" + objSendEmail.StepStatus + "'");
                        str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                        string contens = msg.Body;
                        contens = contens.Replace("\n", "\\n");
                        str.AppendLine(string.Format("DESCRIPTION:{0}", contens));
                        str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));
                        str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

                        str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

                        str.AppendLine("BEGIN:VALARM");
                        str.AppendLine("TRIGGER:-PT15M");
                        str.AppendLine("ACTION:DISPLAY");
                        str.AppendLine("DESCRIPTION:Reminder");
                        str.AppendLine("END:VALARM");
                        str.AppendLine("END:VEVENT");
                        str.AppendLine("END:VCALENDAR");

                        System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
                        contype.Parameters.Add("method", "REQUEST");
                        contype.Parameters.Add("name", "Meeting.ics");

                        //msg.Attachments.Add(new Attachment(str.ToString(), contype));
                        AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);
                        msg.AlternateViews.Add(avCal);

                        client.Send(msg);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }*/

    }
}
