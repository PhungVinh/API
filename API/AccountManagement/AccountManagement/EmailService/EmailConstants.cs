using System;

namespace AccountManagement.EmailService
{
    public static class EmailConstants
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime? ToNullableDateTime(this DateTime value)
        {
            if(value == DateTime.MinValue)
            {
                return null;
            }
            else
            {
                return value;
            }
        }

        public static string RemoveSpecificHTMLTags(string input)
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, "<u.*?>", String.Empty);
            input = System.Text.RegularExpressions.Regex.Replace(input, "<li style.*?>", String.Empty);
            input = System.Text.RegularExpressions.Regex.Replace(input, "<img.*?>", String.Empty);
            return System.Text.RegularExpressions.Regex.Replace(input, "</ul>", String.Empty);
        }

        public static string RemoveALLTags(string input)
        {
            if (!string.IsNullOrEmpty(input))
                return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
            return string.Empty;
        }
    }
}
