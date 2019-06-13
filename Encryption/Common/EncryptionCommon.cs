using Encryption.Constant;
using Encryption.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Common
{
    public class EncryptionCommon
    {
        public string CreateKey(int length = 16)
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
        public string EncryptStringAES(string text, string keyString)
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
        public string DecryptStringAES(string cipherText, string keyString)
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
        public string GetTimeExecuteEncrpytion(DateTime? _dateTime)
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            var executeTime = string.Format("{0:hh\\:mm\\:ss}", dateTime.Subtract(_dateTime.HasValue ? _dateTime.Value : _dateTime.Value));
            return executeTime;
        }

        public List<List<dynamic>> GetAllModule()
        {
            return ExecuteMultipleResults(EncryptionConstant.MASTER_STORE_PROC, null, EncryptionConstant.SQL_MASTER_CONNECTION, typeof(TblMasterMenu));
        }      
        /// <summary>
        /// Executes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnTypes">The return types.</param>
        /// <returns></returns>
        public List<List<dynamic>> ExecuteMultipleResults(string spName, SqlParameter[] parameters, string connection, params Type[] types)
        {
            List<List<dynamic>> results = new List<List<dynamic>>();
            SqlConnection sqlConnection = new SqlConnection(connection);
            var command = sqlConnection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Any())
            {
                command.Parameters.AddRange(parameters);
            }

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            int counter = 0;
            using (var reader = command.ExecuteReader())
            {
                do
                {
                    var innerResults = new List<dynamic>();

                    if (counter > types.Length - 1) { break; }

                    while (reader.Read())
                    {
                        var item = Activator.CreateInstance(types[counter]);

                        for (int inc = 0; inc < reader.FieldCount; inc++)
                        {
                            Type type = item.GetType();
                            string name = reader.GetName(inc);
                            PropertyInfo property = type.GetProperty(name);

                            if (property != null && name == property.Name)
                            {
                                var value = reader.GetValue(inc);
                                if (value != null && value != DBNull.Value)
                                {
                                    property.SetValue(item, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType), null);
                                }
                            }
                        }
                        innerResults.Add(item);
                    }
                    results.Add(innerResults);
                    counter++;
                }
                while (reader.NextResult());
                reader.Close();
                command.Connection.Close();
            }
            return results;
        }
    }
}
