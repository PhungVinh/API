using OrganizationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationManagement.Common
{
    public class OrganizationCommon
    {
        CRM_MASTERContext db = new CRM_MASTERContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MASTERContext>());

        public string MD5Hash(string text)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(text));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        public string HtmlMailCreatedAccount(string name, string account, string pass, string link)
        {
            string str = @"Kính gửi "+ name + @"<br >
                Cảm ơn quý công ty đã đăng ký sử dụng dịch vụ CRM. Chúng tôi gửi tài khoản admin của quý công ty như sau: <br >
                Tên tài khoản : "+ account +@" <br >
                Mật khẩu : "+ pass +@" <br >
                Chúc quý công ty có những trải nghiệm tuyệt vời trên dịch vụ của chúng tôi! <br >
                Trân trọng!";
            return str;
        }



        public string subjectCreate()
        {
            return "Thông tin tài khoản CRM";
        }

        private string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(8);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        public bool ValidatePassword(string password, string correctHash)
        {

            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }


        /// <summary>
        /// CreatedBy: HaiHM
        /// /// CreatedDate: 2019/6/1
        /// </summary>
        private readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        /// <summary>
        /// CreatedBy: HaiHM
        /// CreatedDate: 2019/6/1
        /// </summary>
        /// <returns></returns>
        public string ConvertStringToVNChar(string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return str;
        }

        /// <summary>
        /// CreatedBy: HaiHM
        /// CreatedDate: 2019/6/1
        /// </summary>
        public List<string> ImageExtensions = new List<string> { ".JPG", ".PNG", ".GIF", ".JPEG" };
    }
}
