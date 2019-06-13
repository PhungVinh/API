using AccountManagement.Constant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.Models.DataAccess
{
    public class EmailDA
    {
        CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());

        /// <summary>
        /// Demo - chưa check
        /// thông tin template Email
        /// CreatedBy: HaiHM
        /// CreatedDate: 10/04/2019
        /// </summary>
        /// <param name="CanId"></param>
        /// <returns></returns>
        public object GetEmailTemplate(int templateId)
        {
            try
            {
                var objEmailCandidate = from T in db.TblEmailTemplate
                                        where (T.IsDelete == false && T.TeamplateId == templateId)
                                        select new
                                        {
                                            EmailNames = T.EmailName,
                                            EmailTitile = T.EmailHeader,
                                            Contents = T.EmailContents,
                                            AttachFiles = T.AttachedFile
                                        };
                return objEmailCandidate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Demo - Chưa check
        /// Function remove all html tag in string input
        /// CreatedBy: HaiHM
        /// CreatedDate: 10/04/2019
        /// </summary>
        /// <param name="input">string</param>
        /// <returns>string not has html tag</returns>
        public static string RemoveAllHTMLTags(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", String.Empty);
            return string.Empty;
        }

        /// <summary>
        /// Demo - chưa check
        /// lấy ra các thông tin email của tuyển dụng gửi và ứng viên reply
        /// CreatedBy: HaiHM
        /// CreatedDate: 10/04/2019
        /// </summary>
        /// <param name="canId"></param>
        /// <returns></returns>
        public object GetRecieveEmail(int canId)
        {
            try
            {
                var objRecieveEmail = from r in db.TblRecieveEmail
                                      where (r.UserId == canId)
                                      orderby r.CreateDate descending
                                      select new
                                      {
                                          Froms = r.From,
                                          Tos = r.To,
                                          Subjects = r.Subject,
                                          Contents = r.EmailContents,
                                          EmailStatus = r.StatusEmail,
                                          Dates = r.CreateDate.Value.ToString(AccountConstant.DateFormat)
                                      };
                return objRecieveEmail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Demo - chưa check
        /// Cập nhật lại trạng thái đã đọc Email
        /// CreatedBy: HaiHM
        /// CreatedDate: 10/04/2019
        /// </summary>
        /// <param name="canId"></param>
        /// <returns></returns>
        public int UpdateRecieveEmail(int canId)
        {
            int result = 0;
            try
            {
                List<TblRecieveEmail> recieveEmail = db.TblRecieveEmail.Where(r => r.UserId == canId && r.StatusEmail == true && r.IsReadEmail == false).ToList();
                if (recieveEmail.Count > 0)
                {
                    foreach (TblRecieveEmail item in recieveEmail)
                    {
                        item.IsReadEmail = true;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    result = 1;
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }
    }
}
