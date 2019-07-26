using AccountManagement.Constant;
using AccountManagement.Models;
using AccountManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AccountManagement.Common
{
    public class AccountCommon
    {
        /// <summary>
        /// Function hash md5
        /// CreatedBy: System
        /// CreatedDate: xx/xx/2019
        /// </summary>
        /// <param name="text">input</param>
        /// <returns>MD5 of input</returns>
        public string MD5Hash(string text)
        {
            if (!string.IsNullOrEmpty(text))
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
            else
                return null;
        }

        /// <summary>
        /// Function generate sale (using lib BCrypt.Net-Next)
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/05/2019
        /// </summary>
        /// <returns>string random salt</returns>
        private string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(8);
        }

        /// <summary>
        /// Function hashPassword (using lib BCrypt.Net-Next)
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/05/2019
        /// </summary>
        /// <param name="password"> input string</param>
        /// <returns>string hash</returns>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        /// <summary>
        /// Function ValidatePassword (using lib BCrypt.Net-Next)
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/05/2019
        /// </summary>
        /// <param name="password"></param>
        /// <param name="correctHash"></param>
        /// <returns>true or false</returns>
        public bool ValidatePassword(string password, string correctHash)
        {

            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }

        /// <summary>
        /// Function CreateRandomPassword Default length = 15
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <param name="length">input length</param>
        /// <returns>random character</returns>
        public string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public string CreateRandomStringNotSpecial(int length = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        /// <summary>
        /// Function CreateRandomPassword with length random
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <returns>string random charactor</returns>
        public string CreateRandomPasswordWithRandomLength()
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Minimum size 8. Max size is number of all allowed chars.  
            int size = random.Next(8, validChars.Length);

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        /// <summary>
        /// Function Encrypt string using AES
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keyString"></param>
        /// <returns>string encrypt</returns>
        public string EncryptStringAES_NOT_USE(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        /// <summary>
        /// Function Decrypt string using AES
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public string DecryptStringAES_HMH_USE(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - 16);

            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Function check email
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false</returns>
        public bool ValidateEmail(string email)
        {
            bool check = false;
            Regex regex = new Regex(@"^([a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4})$");
            // string regex = @"^([a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4})$";
            Match match = regex.Match(email);
            if (match.Success)
                check = true;
            else
                check = false;

            return check;
        }

        /// <summary>
        /// Tiêu đề email khi tạo tài khoản
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019 
        /// </summary>
        /// <returns></returns>
        public string SubjectCreate()
        {
            return @"Thông tin tài khoản CRM";
        }

        /// <summary>
        /// Tiêu đề email khi reset khoản
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <returns></returns>
        public string SubjectReset()
        {
            return @"Thông tin reset tài khoản CRM";
        }

        /// <summary>
        /// Content send email created user
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pass"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public string HtmlMailCreatedAccount(string fullname, string username, string pass, string link)
        {
            return @"Xin Chào <b>" + fullname + @" </b> <br >
                    Tài khoản CRM của bạn đã được kích hoạt thành công trên hệ thống. Bạn có thể đăng nhập vào hệ thống tại địa chỉ: " + link + @" <br >
                     Dưới đây là tài khoản đăng nhập của bạn: <br >
                    -Tên đăng nhập: <b>" + username + @" </b> <br >
                    -Mật khẩu: <b>" + pass + @"</b><br >
                    <i>Lưu ý</i>: Mật khẩu này chỉ tồn tại trong thời hạn 30 ngày <br >
                    Trân trọng!";
        }

        /// <summary>
        /// Content send email reset
        /// CreatedBy: HaiHM
        /// CreatedDated: xx/04/2019
        /// </summary>
        /// <param name="account"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public String HtmlMailResetAccount(string account, string link)
        {
            string str = @"Xin Chào <b>" + account + @" </b> <br >
                    Hệ thống CRM nhận được yêu cầu reset mật khẩu đăng nhập của bạn. Bạn có thể truy cập theo link để thực hiện đổi mật khẩu đăng nhập: " + link + @" <br >
                    Lưu ý: Link truy cập chỉ tồn tại trong 30 ngày. <br >
                    Trân trọng!";
            return str;
        }

        /// <summary>
        /// CreatedBy: HaiHM
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


        //".JPE", ".BMP", ".GIF", ".JPEG"
        public List<string> ImageExtensions = new List<string> { ".JPG", ".PNG", ".GIF", ".JPEG" };
        public bool IsRoleFalse(RoleDTO role1, RoleDTO role2)
        {
            if (role1.IsEncypt == role2.IsEncypt &&
                role1.IsShow == role2.IsShow &&
                role1.IsShowAll == role2.IsShowAll &&
                role1.IsAdd == role2.IsAdd &&
                role1.IsEdit == role2.IsEdit &&
                role1.IsEditAll == role2.IsEditAll &&
                role1.IsDelete == role2.IsDelete &&
                role1.IsDeleteAll == role2.IsDeleteAll &&
                role1.IsImport == role2.IsImport &&
                role1.IsExport == role2.IsExport &&
                role1.IsPrint == role2.IsPrint &&
                role1.IsApprove == role2.IsApprove &&
                role1.IsEnable == role2.IsEnable &&
                role1.IsPermission == role2.IsPermission &&
                role1.IsFirstExtend == role2.IsFirstExtend &&
                role1.IsSecondExtend == role2.IsSecondExtend &&
                role1.IsThirdExtend == role2.IsThirdExtend &&
                role1.IsFouthExtend == role2.IsFouthExtend)
            {
                return true;
            }
            return false;
        }
        public RoleDTO JoinPermission(RoleDTO role1, TblRole role2)
        {
            role1.IsEncypt = role2.IsEncypt == true ? role2.IsEncypt : role1.IsEncypt;
            role1.IsShow = role2.IsShow == true ? role2.IsShow : role1.IsShow;
            role1.IsShowAll = role2.IsShowAll == true ? role2.IsShowAll : role1.IsShowAll;
            role1.IsAdd = role2.IsAdd == true ? role2.IsAdd : role1.IsAdd;
            role1.IsEdit = role2.IsEdit == true ? role2.IsEdit : role1.IsEdit;
            role1.IsEditAll = role2.IsEditAll == true ? role2.IsEditAll : role1.IsEditAll;
            role1.IsDelete = role2.IsDelete == true ? role2.IsDelete : role1.IsDelete;
            role1.IsDeleteAll = role2.IsDeleteAll == true ? role2.IsDeleteAll : role1.IsDeleteAll;
            role1.IsImport = role2.IsImport == true ? role2.IsImport : role1.IsImport;
            role1.IsExport = role2.IsExport == true ? role2.IsExport : role1.IsExport;
            role1.IsPrint = role2.IsPrint == true ? role2.IsPrint : role1.IsPrint;
            role1.IsApprove = role2.IsApprove == true ? role2.IsApprove : role1.IsApprove;
            role1.IsEnable = role2.IsEnable == true ? role2.IsEnable : role1.IsEnable;
            role1.IsPermission = role2.IsPermission == true ? role2.IsPermission : role1.IsPermission;
            role1.IsFirstExtend = role2.IsFirstExtend == true ? role2.IsFirstExtend : role1.IsFirstExtend;
            role1.IsSecondExtend = role2.IsSecondExtend == true ? role2.IsSecondExtend : role1.IsSecondExtend;
            role1.IsThirdExtend = role2.IsThirdExtend == true ? role2.IsThirdExtend : role1.IsThirdExtend;
            role1.IsFouthExtend = role2.IsFouthExtend == true ? role2.IsFouthExtend : role1.IsFouthExtend;
            return role1;
        }

        /// <summary>
        /// Custom key
        /// HaiHM
        /// 11/6/2019
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string value)
        {
            byte[] bytes = new byte[16];
            byte[] strByte = Encoding.UTF8.GetBytes(value);
            for (int i = 0; i < 16; i++)
            {
                if (i < strByte.Length)
                {
                    bytes[i] = strByte[i];
                }
                else
                {
                    bytes[i] = 0;
                }
            }
            return bytes;
        }

        /// <summary>
        /// GenerateEncryptionKey
        /// HaiHM
        /// /// 11/6/2019
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public string GenerateEncryptionKey(string orgCode)
        {
            string result = string.Empty;
            result = orgCode + AccountConstant.ENCRYPTIONKEY;
            return result;
        }

        /// <summary>
        /// EncryptStringAES
        /// HaiHM
        /// 11/06/2019
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public string EncryptStringAES(string text, string keyString)
        {
            var key = ToByteArray(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        /// <summary>
        /// DecryptStringAES
        /// HaiHM
        /// 11/06/2019
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public string DecryptStringAES(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - 16);

            var key = ToByteArray(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public List<string> FieldTblUserExtensions = new List<string>
        {   "FullName",
            "Address"
        };
    }
}
