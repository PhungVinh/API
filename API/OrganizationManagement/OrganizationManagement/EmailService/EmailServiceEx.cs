using Microsoft.EntityFrameworkCore;
using OrganizationManagement.Common;
using OrganizationManagement.Constant;
using OrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrganizationManagement.EmailService
{
    public class EmailServiceEx : IEmailService
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());

        /// <summary>
        /// HaiHM
        /// Send email, create user, edit user
        /// ModifiedBy: HaiHM
        /// ModifiedDate: 06/06/2019
        /// ModifiedContent: sửa tiêu đề trả về là tiếng việt
        /// </summary>
        /// <returns></returns>
        public void SendEmail(TblRecieveEmail objSendEmail)
        {
            var response = new { Code = 0 };

            try
            {
                //SmtpClient client = CommonFunction.InstanceSendClient();
                //MailMessage message = new MailMessage("super88mp@gmail.com", objSendEmail.To);
                //message.Subject = objSendEmail.Subject;
                //message.Body = objSendEmail.EmailContents;
                //message.IsBodyHtml = true;
                //client.SendAsync(message, "Create");
                //return objSendEmail;
                SmtpClient client = CommonFunction.InstanceSendClient();
                MailMessage newMail = new MailMessage();
                newMail.To.Add(new MailAddress(objSendEmail.To));
                newMail.From = new MailAddress("super88mp@gmail.com");
                newMail.Subject = objSendEmail.Subject;
                newMail.Body = objSendEmail.EmailContents;
                newMail.IsBodyHtml = true;
                newMail.SubjectEncoding = Encoding.UTF8;
                newMail.HeadersEncoding = Encoding.UTF8;

                new Thread(() => { client.Send(newMail); }).Start();

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
                    string imgUrl = OrganizationManagement.Common.CommonFunction.strDomainCV + emailConfig.Signature;
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
                                FileStream fs = new FileStream(OrganizationConstant.PathURL + filepath, FileMode.Open, FileAccess.Read);
                                StreamReader s = new StreamReader(fs);
                                s.Close();
                                fs = new FileStream(OrganizationConstant.PathURL + filepath, FileMode.Open, FileAccess.Read);
                                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(MediaTypeNames.Text.Plain);
                                Attachment attachment = new Attachment(fs, ct);
                                attachment.ContentDisposition.Inline = true;
                                System.Net.Mime.ContentDisposition disposition = attachment.ContentDisposition;
                                disposition.CreationDate = File.GetCreationTime(OrganizationConstant.PathURL + filepath);
                                disposition.ModificationDate = File.GetLastWriteTime(OrganizationConstant.PathURL + filepath);
                                disposition.ReadDate = File.GetLastAccessTime(OrganizationConstant.PathURL + filepath);
                                disposition.FileName = Path.GetFileName(OrganizationConstant.PathURL + filepath);
                                disposition.Size = new FileInfo(OrganizationConstant.PathURL + filepath).Length;
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
    }
}
