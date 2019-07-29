using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Common
{
    public class CommonFunction
    {
        /// <summary>
        /// Function hash md5
        /// CreatedBy: HaiHM
        /// CreatedDate: 17/07/2019
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
    }
}
